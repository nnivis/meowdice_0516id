using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Dice;
using CodeBase.Domain.Field.Cell;
using UnityEngine;

namespace CodeBase.Domain.Field
{
    public class Field
    {
        private const int Rows = 3;
        private const int Columns = 3;

        private readonly FieldColumn[] _columns;

        public PlayerId OwnerId { get; }

        public int RowsCount => Rows;
        public int ColumnsCount => Columns;
        

        public Field(PlayerId ownerId)
        {
            OwnerId = ownerId;
            _columns = new FieldColumn[Columns];

            for (int i = 0; i < Columns; i++)
                _columns[i] = new FieldColumn(i, Rows);
        }
        
        public DiceStateType GetColumnState(int column)
        {
            if (column < 0 || column >= ColumnsCount)
                throw new ArgumentOutOfRangeException(nameof(column));

            return _columns[column].GetState();
        }

        public bool TryGetColumnCombinationValue(int column, out int value)
        {
            if (column < 0 || column >= ColumnsCount)
                throw new ArgumentOutOfRangeException(nameof(column));

            return _columns[column].TryGetCombinationValue(out value);
        }
        
        public bool TryGetFullColumnUniformValue(int column, out int value)
        {
            if (column < 0 || column >= ColumnsCount)
                throw new ArgumentOutOfRangeException(nameof(column));

            return _columns[column].TryGetUniformValue(out value);
        }

        public void ClearColumn(int column)
        {
            if (column < 0 || column >= ColumnsCount)
                return;

            _columns[column].ClearAll();
        }
        

        public bool TryPlaceDice(Dice.Dice dice, CellPosition pos)
        {
            if (!IsInside(pos))
                return false;

            return _columns[pos.Col].TryPlaceDice(dice, pos.Row);
        }

        public bool TryGetDice(CellPosition pos, out Dice.Dice dice)
        {
            dice = null;

            if (!IsInside(pos))
                return false;

            return _columns[pos.Col].TryGetDice(pos.Row, out dice);
        }

        public void RemoveAllInColumnByValue(int column, int value)
        {
            if (column < 0 || column >= ColumnsCount)
                return;

            _columns[column].RemoveAllByValue(value);
        }

        public IReadOnlyList<int> GetColumnDiceValues(int column)
        {
            if (column < 0 || column >= ColumnsCount)
                throw new ArgumentOutOfRangeException(nameof(column));

            return _columns[column].GetDiceValues();
        }

        public bool IsFull()
        {
            for (int i = 0; i < _columns.Length; i++)
            {
                if (!_columns[i].IsFull())
                    return false;
            }
            return true;
        }

        private bool IsInside(CellPosition pos)
        {
            return pos.Row >= 0 && pos.Row < RowsCount &&
                   pos.Col >= 0 && pos.Col < ColumnsCount;
        }
    }
}