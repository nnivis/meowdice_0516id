using System;
using System.Collections.Generic;
using CodeBase.Domain.Dice;
using CodeBase.Domain.Field.Cell;
using UnityEngine;

namespace CodeBase.Domain.Field.View
{
    public class FieldView : MonoBehaviour
    {
        public PlayerSlot Slot => _slot;
        public bool IsCreated => _columns.Count > 0;
        
        [SerializeField] private DiceViewFactory _diceViewFactory;
        [SerializeField] private PlayerSlot _slot;
        
        private readonly List<FieldColumnView> _columns = new();

        public void Build(
            FieldColumnViewFactory factory,
            int columnsCount,
            int rowsCount,
            ScoreTextPosition scoreTextPosition)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            if (columnsCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(columnsCount));

            if (rowsCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(rowsCount));

            Clear();

            for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
            {
                FieldColumnView columnView = factory.Create(
                    (RectTransform)transform,
                    columnIndex,
                    rowsCount,
                    scoreTextPosition);

                _columns.Add(columnView);
            }
        }
        public void PlaceDice(
            DiceStateType stateType,
            DicePointType pointType,
            CellPosition pos)
        {
            try
            {
                int columnIndex = pos.Col;
                int rowIndex = pos.Row;

                ValidatePosition(pos);

                var diceView = _diceViewFactory.CreateDice(transform);

                diceView.Render(stateType, pointType);
                diceView.Bind(null);

                _columns[columnIndex].PlaceDiceView(diceView, rowIndex);
            }
            catch (Exception e)
            {
                Debug.LogError($"FieldView.PlaceDice failed: {e}");
            }
        }

        public void RemoveDiceView(CellPosition pos)
        {
            ValidatePosition(pos);
             _columns[pos.Col].RemoveDiceView(pos.Row);
        }

        public CellView GetCell(CellPosition pos)
        {
            ValidatePosition(pos);
            return _columns[pos.Col].GetCell(pos.Row);
        }

        public FieldColumnView GetColumn(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= _columns.Count)
                throw new ArgumentOutOfRangeException(nameof(columnIndex));

            return _columns[columnIndex];
        }

        public void Clear()
        {
            for (int i = 0; i < _columns.Count; i++)
            {
                if (_columns[i] != null)
                    Destroy(_columns[i].gameObject);
            }

            _columns.Clear();
        }

        public void SetColumnScore(int columnIndex, int score)
        {
            GetColumn(columnIndex).SetScore(score);
        }
        private void ValidatePosition(CellPosition pos)
        {
            int columnIndex = pos.Col;
            int rowIndex = pos.Row;

            if (columnIndex < 0 || columnIndex >= _columns.Count)
                throw new ArgumentOutOfRangeException(nameof(pos), 
                    $"Invalid column index: {columnIndex}, columns count: {_columns.Count}");

            if (rowIndex < 0 || rowIndex >= _columns[columnIndex].CellsCount)
                throw new ArgumentOutOfRangeException(nameof(pos), 
                    $"Invalid row index: {rowIndex}, rows count in column {columnIndex}: {_columns[columnIndex].CellsCount}");
        }
    }
}