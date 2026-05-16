using CodeBase.Domain.Dice;
using CodeBase.Domain.Field.Cell;
using CodeBase.Domain.Field.View;
using UnityEngine;

namespace CodeBase.Domain.Field
{
    public class FieldPanel : MonoBehaviour
    {
        [SerializeField] private FieldView _localPlayerView;
        [SerializeField] private FieldView _opponentPlayerView;
        [SerializeField] private FieldColumnViewFactory _fieldColumnFactory;

        public void EnsureCreated(int columnsCount, int rowsCount)
        {
            if (_localPlayerView != null && !_localPlayerView.IsCreated)
            {
                _localPlayerView.Build(
                    _fieldColumnFactory,
                    columnsCount,
                    rowsCount,
                    ScoreTextPosition.Top);
            }

            if (_opponentPlayerView != null && !_opponentPlayerView.IsCreated)
            {
                _opponentPlayerView.Build(
                    _fieldColumnFactory,
                    columnsCount,
                    rowsCount,
                    ScoreTextPosition.Bottom);
            }
        }
        
        public FieldView Get(PlayerSlot slot) => slot == PlayerSlot.Local ? _localPlayerView : _opponentPlayerView;
        
        public void PlaceDice(
            PlayerSlot slot,
            DiceStateType stateType,
            DicePointType pointType,
            CellPosition pos)
        {
            var fieldView = Get(slot);
            fieldView.PlaceDice(stateType, pointType, pos);
        }
        
        public void SetColumnScore(PlayerSlot slot, int columnIndex, int score)
        {
            Get(slot).SetColumnScore(columnIndex, score);
        }

        public void ClearAll()
        {
            if (_localPlayerView != null)
                _localPlayerView.Clear();

            if (_opponentPlayerView != null)
                _opponentPlayerView.Clear();
        }
        
    }
    
    
    public enum ScoreTextPosition
    {
        Top,
        Bottom
    }
}