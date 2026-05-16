using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Match;
using CodeBase.Domain.Match.Data;
using CodeBase.Services.Interaction;
using UnityEngine;

namespace CodeBase.Domain.Board
{
    public class BoardsPresenter : MonoBehaviour
    {
        [SerializeField] private BoardView _playerBoardView;
        [SerializeField] private BoardView _opponentBoardView;
        
        [SerializeField] private DiceDragDropController _diceDragDropController;

        private readonly Dictionary<PlayerSlot, BoardView> _viewBySlot = new();

        private IMatchReadModel _model;
        private MatchPlayerContext _playerContext;

        public void Bind(IMatchReadModel model, MatchPlayerContext playerContext)
        {
            Unbind();

            _model = model;
            _playerContext = playerContext;

            _viewBySlot[PlayerSlot.Local] = _playerBoardView;
            _viewBySlot[PlayerSlot.Opponent] = _opponentBoardView;

            _model.DiceChanged += OnDiceChanged;

            OnDiceChanged(
                _playerContext.LocalPlayer.PlayerId,
                _model.GetDice(_playerContext.LocalPlayer.PlayerId));

            OnDiceChanged(
                _playerContext.OpponentPlayer.PlayerId,
                _model.GetDice(_playerContext.OpponentPlayer.PlayerId));
        }

        public void Unbind()
        {
            if (_model != null)
                _model.DiceChanged -= OnDiceChanged;

            _model = null;
            _playerContext = null;
            _viewBySlot.Clear();
        }

        private void OnDiceChanged(PlayerId playerId, Dice.Dice dice)
        {
            
            if (_playerContext == null)
                return;

            var slot = _playerContext.ResolveSlot(playerId);

            if (!_viewBySlot.TryGetValue(slot, out var view))
                return;

            if (dice == null)
            {
                view.ClearDiceView();
                return;
            }

            view.ShowDice(dice.DiceStateType, dice.DicePointType, _diceDragDropController, slot == PlayerSlot.Local);
        }
        private void OnDestroy() => Unbind();
    }
}