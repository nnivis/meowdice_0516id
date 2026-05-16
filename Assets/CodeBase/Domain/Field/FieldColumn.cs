using System;
using System.Collections.Generic;
using CodeBase.Domain.Dice;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Domain.Field
{
    public class FieldColumn
    {
        private readonly Cell.Cell[] _cells;

        public int Index { get; }
        public int RowsCount => _cells.Length;

        public DiceStateType GetState() => EvaluateMatch().State;

        public FieldColumn(int index, int rows)
        {
            if (rows <= 0)
                throw new ArgumentOutOfRangeException(nameof(rows));

            Index = index;
            _cells = new Cell.Cell[rows];

            for (int row = 0; row < rows; row++)
                _cells[row] = new Cell.Cell(new CellPosition(index, row));
        }

        public Cell.Cell GetCell(int row)
        {
            if (!IsInside(row))
                throw new ArgumentOutOfRangeException(nameof(row));

            return _cells[row];
        }

        public bool TryPlaceDice(Dice.Dice dice, int row)
        {
            if (!IsInside(row))
                return false;

            if (!_cells[row].TryPlaceDice(dice))
                return false;

            RecalculateDiceStates();
            return true;
        }

        public bool TryGetDice(int row, out Dice.Dice dice)
        {
            dice = null;

            if (!IsInside(row))
                return false;

            dice = _cells[row].Dice;
            return dice != null;
        }

        public bool TryGetCombinationValue(out int value)
        {
            var match = EvaluateMatch();

            if (match.Value.HasValue)
            {
                value = match.Value.Value;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryGetUniformValue(out int value)
        {
            var match = EvaluateMatch();

            if (match.IsUniformFullColumn && match.Value.HasValue)
            {
                value = match.Value.Value;
                return true;
            }

            value = default;
            return false;
        }

        public void Remove(int row)
        {
            if (!IsInside(row))
                return;

            _cells[row].Clear();
            RecalculateDiceStates();
        }

        public void ClearAll()
        {
            bool changed = false;

            for (int row = 0; row < _cells.Length; row++)
            {
                if (_cells[row].IsEmpty)
                    continue;

                _cells[row].Clear();
                changed = true;
            }

            if (changed)
                RecalculateDiceStates();
        }

        public void RemoveAllByValue(int value)
        {
            bool changed = false;

            for (int row = 0; row < _cells.Length; row++)
            {
                var dice = _cells[row].Dice;
                if (dice != null && dice.Value == value)
                {
                    _cells[row].Clear();
                    changed = true;
                }
            }

            if (changed)
                RecalculateDiceStates();
        }

        public IReadOnlyList<int> GetDiceValues()
        {
            var result = new List<int>(_cells.Length);

            for (int row = 0; row < _cells.Length; row++)
            {
                var dice = _cells[row].Dice;
                if (dice != null)
                    result.Add(dice.Value);
            }

            return result;
        }

        public bool IsFull()
        {
            for (int row = 0; row < _cells.Length; row++)
            {
                if (_cells[row].IsEmpty)
                    return false;
            }

            return true;
        }

        private void RecalculateDiceStates()
        {
            var state = EvaluateMatch().State;

            for (int row = 0; row < _cells.Length; row++)
            {
                var dice = _cells[row].Dice;
                if (dice != null)
                    dice.SetState(state);
            }
        }

        private ColumnMatchInfo EvaluateMatch()
        {
            var diceValues = GetDiceValues();

            if (diceValues.Count == 0)
                return new ColumnMatchInfo(DiceStateType.Normal, null, false);

            if (diceValues.Count == _cells.Length)
            {
                int first = diceValues[0];
                bool allEqual = true;

                for (int i = 1; i < diceValues.Count; i++)
                {
                    if (diceValues[i] != first)
                    {
                        allEqual = false;
                        break;
                    }
                }

                return new ColumnMatchInfo(
                    DiceStateType.FullColumn,
                    allEqual ? first : null,
                    allEqual);
            }

            if (diceValues.Count == 2 && diceValues[0] == diceValues[1])
                return new ColumnMatchInfo(DiceStateType.MatchedPair, diceValues[0], false);

            return new ColumnMatchInfo(DiceStateType.Normal, null, false);
        }

        private bool IsInside(int row) => row >= 0 && row < _cells.Length;

        private readonly struct ColumnMatchInfo
        {
            public DiceStateType State { get; }
            public int? Value { get; }
            public bool IsUniformFullColumn { get; }

            public ColumnMatchInfo(DiceStateType state, int? value, bool isUniformFullColumn)
            {
                State = state;
                Value = value;
                IsUniformFullColumn = isUniformFullColumn;
            }
        }
    }
}