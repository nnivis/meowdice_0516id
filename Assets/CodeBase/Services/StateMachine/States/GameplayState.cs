using CodeBase.Services.GameResult;
using CodeBase.Services.GameStart;
using UnityEngine;
using VContainer;

namespace CodeBase.Services.StateMachine.States
{
    public class GameplayState : StateMachineBehavior
    {
        [SerializeField] private MainSceneMode _mainSceneMode;
   
        [Inject] private GameStartConfigFactory _gameStartGame;
        [Inject] private GameCoordinator _gameCoordinator;
        [Inject] private MatchResultStorage  _matchResultStorage;
        

        protected override void OnEnter()
        {
            _gameCoordinator.OnResultReady += OnResultReady;
            var config = _gameStartGame.CreateConfig(GameMode.VsAi);
            
            _gameCoordinator.StartGame(config);
        }

        private void OnResultReady(MatchResult matchResult)
        {
            _matchResultStorage.Set(matchResult);
            _mainSceneMode.GoToGameResult();
        }

        protected override void OnExit()
        {
            _gameCoordinator.OnResultReady -= OnResultReady;
            _gameCoordinator.StopGame();
        }
    }
}
