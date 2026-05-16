using System;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Match;
using CodeBase.Domain.Match.Content;
using CodeBase.Domain.Match.Data;
using CodeBase.Infrastructure.DataProvider;
using CodeBase.Services.Flow;
using CodeBase.Services.GameResult;
using CodeBase.Services.GameStart;
using CodeBase.Services.Interaction;
using CodeBase.Services.Turn;
using CodeBase.Services.Turn.Controllers;
using UnityEngine;
using VContainer;

namespace CodeBase.Services
{
    public class GameCoordinator : MonoBehaviour
    {
        public event Action<MatchResult> OnResultReady;
        
        [SerializeField] private MatchPresenter _matchPresenter;
        [SerializeField] private DiceDragDropController _diceDragDropController;

        private MatchFactory _matchFactory;
        private Match _match;
        private MatchPlayerContext _playerContext;
        
        private IControllerFactory _controllerFactory;
        private IOpponentFactory _opponentFactory;
        
        //private LocalHumanPlayerController _localController;
        
        private GameFlow _gameFlow;

        private IPersistentData _persistentData;
        private IDataProvider _dataProvider;

        [Inject]
        public void Construct(MatchFactory matchFactory, IControllerFactory controllerFactory, IOpponentFactory opponentFactory)
        {
            _matchFactory = matchFactory;
            _controllerFactory = controllerFactory;
            _opponentFactory = opponentFactory;
        }
        
        public void InitData(IDataProvider dataProvider, IPersistentData persistentData)
        {
            _persistentData = persistentData;
            _dataProvider = dataProvider;
        }

        public void StartGame(GameStartConfig config)
        {

            StopGame();
            
            var players = new[] { config.Participants.Local, config.Participants.Opponent };

            _match = _matchFactory.Create(players,  config.FirstTurnPlayer);
            _match.GameEnded += OnGameEnded;
            
            
            var localPlayerData = _persistentData.PlayerData;
            var localInfo = new MatchPlayerInfo(
                localPlayerData.Id,
                localPlayerData.Name,
                localPlayerData.SkinId);
            
            var opponentInfo = _opponentFactory.Create(
                config.Participants.Opponent,
                localInfo.SkinId);

            _playerContext = new MatchPlayerContext(localInfo, opponentInfo);
            _matchPresenter.StartMatch(_match, _playerContext); // наверное, оптимальней было бы сделать обертку
            
            var controllers = _controllerFactory.CreateControllers(config, _match);
            var localController = controllers[config.Participants.Local] as LocalHumanPlayerController;
            
            _diceDragDropController.Init(localController);
            
            var turnSystem = new TurnSystem(_match, controllers, 1);
            _gameFlow = new GameFlow(_match, turnSystem);
            _gameFlow.Start();
            
        }

        public void StopGame()
        {
            _gameFlow?.Stop();
            _gameFlow = null;

            if (_match != null)
                _match.GameEnded -= OnGameEnded;
            
            _matchPresenter.StopMatch();
            _match = null;
            
            _playerContext = null;
        }

        private void OnGameEnded(PlayerId? winner, int  firstPlayerFinalScore, int secondPlayerFinalScore)
        {
            var result = new MatchResult(_playerContext, winner, firstPlayerFinalScore, secondPlayerFinalScore);
            OnResultReady?.Invoke(result);

            _gameFlow?.Stop();
        }
    }
}