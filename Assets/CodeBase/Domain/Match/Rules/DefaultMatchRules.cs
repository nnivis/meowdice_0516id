using System;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;
using CodeBase.Domain.Match.Module;
using CodeBase.Domain.Match.Rules.ScoreCalculator;

namespace CodeBase.Domain.Match.Rules
{
    public class DefaultMatchRules : IMatchRules
    {
        private readonly IFieldScoreCalculator _scoreCalculator;

        public DefaultMatchRules(IFieldScoreCalculator scoreCalculator)
        {
            _scoreCalculator = scoreCalculator;
        }

        public void ResolveAfterPlacement(MatchState state, PlayerId placedBy, Dice.Dice placedDice, CellPosition pos)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (placedDice == null)
                return;

            var playerField = state.GetField(placedBy);
            var opponentField = state.GetField(state.GetOpponent(placedBy));
            var column = pos.Col;

            switch (playerField.GetColumnState(column))
            {
                case Dice.DiceStateType.MatchedPair:
                    ResolveMatchedPair(playerField, opponentField, column);
                    break;

                case Dice.DiceStateType.FullColumn:
                    ResolveFullColumn(playerField, opponentField, column);
                    break;
            }
        }

        public MatchResult? TryGetResult(MatchState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (!AreAllFieldsFull(state))
                return null;

            if (state.Players.Count != 2)
                throw new InvalidOperationException("DefaultMatchRules assumes exactly 2 players.");

            var firstPlayer = state.Players[0];
            var secondPlayer = state.Players[1];

            var firstScore = _scoreCalculator.ComputeTotalScore(state.GetField(firstPlayer));
            var secondScore = _scoreCalculator.ComputeTotalScore(state.GetField(secondPlayer));

            if (firstScore > secondScore)
                return MatchResult.Win(firstPlayer);

            if (secondScore > firstScore)
                return MatchResult.Win(secondPlayer);

            return MatchResult.Draw();
        }

        public int GetPlayerScore(MatchState state, PlayerId playerId)
        {
            return _scoreCalculator.ComputeTotalScore(state.GetField(playerId));
        }

        private void ResolveMatchedPair(Field.Field playerField, Field.Field opponentField, int column)
        {
            if (playerField.GetColumnState(column) != Dice.DiceStateType.MatchedPair)
                return;

            if (opponentField.GetColumnState(column) != Dice.DiceStateType.MatchedPair)
                return;

            if (!playerField.TryGetColumnCombinationValue(column, out var playerValue))
                return;

            if (!opponentField.TryGetColumnCombinationValue(column, out var opponentValue))
                return;

            if (playerValue != opponentValue)
                return;

            opponentField.RemoveAllInColumnByValue(column, opponentValue);
        }

        private void ResolveFullColumn(Field.Field playerField, Field.Field opponentField, int column)
        {
            if (playerField.GetColumnState(column) != Dice.DiceStateType.FullColumn)
                return;

            if (opponentField.GetColumnState(column) != Dice.DiceStateType.FullColumn)
                return;

            if (!playerField.TryGetFullColumnUniformValue(column, out var playerValue))
                return;

            if (!opponentField.TryGetFullColumnUniformValue(column, out var opponentValue))
                return;

            if (playerValue != opponentValue)
                return;

            opponentField.ClearColumn(column);
        }
        
        

        private bool AreAllFieldsFull(MatchState state)
        {
            foreach (var playerId in state.Players)
            {
                if (!state.GetField(playerId).IsFull())
                    return false;
            }

            return true;
        }
    }
}