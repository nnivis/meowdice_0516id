using System;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;
using CodeBase.Domain.Match;
using CodeBase.Domain.Match.Data;
using UnityEngine;

namespace CodeBase.Domain.Field
{
    public class FieldPresenter : MonoBehaviour
    {
        [SerializeField] private FieldPanel _fieldPanel;

        private IMatchReadModel _match;
        private MatchPlayerContext _playerContext;

        public void Bind(IMatchReadModel match, MatchPlayerContext playerContext)
        {
            Unbind();

            _match = match;
            _playerContext = playerContext;

            if (_match.Players == null || _match.Players.Count == 0)
                throw new InvalidOperationException("Match does not contain players.");

            var field = _match.GetField(_playerContext.LocalPlayer.PlayerId);

            _fieldPanel.ClearAll();
            _fieldPanel.EnsureCreated(field.ColumnsCount, field.RowsCount);

            _match.DicePlaced += OnDicePlaced;
        }

        public void Unbind()
        {
            if (_match != null)
                _match.DicePlaced -= OnDicePlaced;

            _match = null;
            _playerContext = null;
        }

        private void OnDicePlaced(PlayerId playerId, Domain.Dice.Dice dice, CellPosition pos)
        {
            if (dice == null)
                return;

            var localId = _playerContext.LocalPlayer.PlayerId;
            var opponentId = _playerContext.OpponentPlayer.PlayerId;

            RefreshColumnVisual(PlayerSlot.Local, localId, pos.Col);
            RefreshColumnVisual(PlayerSlot.Opponent, opponentId, pos.Col);
            RefreshColumnScores(pos.Col);
        }
        
        private void RefreshColumnVisual(PlayerSlot slot, PlayerId playerId, int columnIndex)
        {
            var field = _match.GetField(playerId);
            var fieldView = _fieldPanel.Get(slot);
            
            for (int row = 0; row < field.RowsCount; row++)
            {
                var pos = new CellPosition(columnIndex, row);

                // удалить текущий visual если он есть
                var cellView = fieldView.GetCell(pos);
                cellView.ClearDiceView();

                // если в домене есть dice — создать новый visual
                if (field.TryGetDice(pos, out var dice))
                {
                    fieldView.PlaceDice(dice.DiceStateType, dice.DicePointType, pos);
                }
            }
        }
        
        private void RefreshColumnScores(int columnIndex)
        {
            var localId = _playerContext.LocalPlayer.PlayerId;
            var opponentId = _playerContext.OpponentPlayer.PlayerId;

           var localScore = _match.GetColumnScore(localId, columnIndex);
           var opponentScore = _match.GetColumnScore(opponentId, columnIndex);

            _fieldPanel.SetColumnScore(PlayerSlot.Local, columnIndex, localScore);
            _fieldPanel.SetColumnScore(PlayerSlot.Opponent, columnIndex, opponentScore);
        }

        private void OnDestroy() => Unbind();
    }
}