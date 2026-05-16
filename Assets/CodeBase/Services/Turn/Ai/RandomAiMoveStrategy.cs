using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field;
using CodeBase.Domain.Field.Cell;
using CodeBase.Domain.Match;

namespace CodeBase.Services.Turn.Ai
{
    public class RandomAiMoveStrategy : IAiMoveStrategy
    {
        public CellPosition ChoosePlacement(IMatchReadModel match, PlayerId playerId)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            Field field = match.GetField(playerId);
            List<CellPosition> availablePositions = CollectAvailablePositions(field);

            if (availablePositions.Count == 0)
                throw new InvalidOperationException(
                    $"AI has no available moves for player {playerId.Value}.");

            int randomIndex = UnityEngine.Random.Range(0, availablePositions.Count);
            CellPosition selectedPosition = availablePositions[randomIndex];

            return selectedPosition;
        }

        private List<CellPosition> CollectAvailablePositions(Field field)
        {
            var result = new List<CellPosition>();

            for (int col = 0; col < field.ColumnsCount; col++)
            {
                for (int row = 0; row < field.RowsCount; row++)
                {
                    var position = new CellPosition(col, row);

                    if (!field.TryGetDice(position, out _))
                        result.Add(position);
                }
            }

            return result;
        }
    }
}