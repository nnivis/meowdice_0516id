using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;
using CodeBase.Domain.Match.Module;
using CodeBase.Domain.Match.Rules;
using CodeBase.Domain.Match.Rules.ScoreCalculator;

namespace CodeBase.Domain.Match
{
    public class Match : IMatchReadModel
    {
        public event Action<PlayerId> ActivePlayerChanged;
        public event Action<PlayerId, Dice.Dice> DiceChanged;
        public event Action<PlayerId, Dice.Dice, CellPosition> DicePlaced;
        public event Action<PlayerId?, int, int> GameEnded;

        public IReadOnlyList<PlayerId> Players => State.Players;
        public PlayerId ActivePlayer => State.TurnOrder.ActivePlayer;

        public MatchState State { get; }
        public PlayerId? Winner { get; private set; }
        public bool IsFinished { get; private set; }

        private readonly IMatchRules _rules;
        private readonly IFieldScoreCalculator _scoreCalculator;

        public Match(MatchState state, IMatchRules rules, IFieldScoreCalculator scoreCalculator)
        {
            State = state;
            _rules = rules;
            _scoreCalculator = scoreCalculator;

            foreach (var playerId in State.Players)
            {
                var board = State.GetBoard(playerId);
                board.DiceChanged += dice => DiceChanged?.Invoke(playerId, dice);
            }
        }

        public Field.Field GetField(PlayerId id) => State.GetField(id);
        public Board.Board GetBoard(PlayerId id) => State.GetBoard(id);
        public Dice.Dice GetDice(PlayerId playerId) => State.GetBoard(playerId).Dice;

        public bool IsStartTurn(PlayerId playerId)
        {
            EnsureNotFinished();
            EnsureTurnOwner(playerId);

            if (TryFinishGame())
                return false;

            if (GetField(playerId).IsFull())
            {
                EndTurn();
                return false;
            }

            State.GetBoard(playerId).RollDice();
            return true;
        }

        public void EndTurn()
        {
            EnsureNotFinished();

            State.TurnOrder.Next();
            ActivePlayerChanged?.Invoke(State.TurnOrder.ActivePlayer);
        }

        public bool TryPlaceDice(PlayerId playerId, CellPosition position)
        {
            EnsureNotFinished();
            EnsureTurnOwner(playerId);

            var board = State.GetBoard(playerId);
            var dice = board.Dice;

            if (dice == null)
                return false;

            var field = State.GetField(playerId);

            if (!field.TryPlaceDice(dice, position))
                return false;

            _rules.ResolveAfterPlacement(State, playerId, dice, position);

            DicePlaced?.Invoke(playerId, dice, position);
            board.ClearDice();

            TryFinishGame();

            return true;
        }

        private bool TryFinishGame()
        {
            var result = _rules.TryGetResult(State);

            if (!result.HasValue)
                return false;

            var firstPlayerScore = _rules.GetPlayerScore(State, Players[0]);
            var secondPlayerScore = _rules.GetPlayerScore(State, Players[1]);

            IsFinished = true;
            Winner = result.Value.Winner;
            GameEnded?.Invoke(Winner, firstPlayerScore, secondPlayerScore);
            return true;
        }

        public int GetColumnScore(PlayerId playerId, int columnIndex)
        {
            var field = State.GetField(playerId);
            return _scoreCalculator.ComputeColumnScore(field.GetColumnDiceValues(columnIndex));
        }

        private void EnsureTurnOwner(PlayerId playerId)
        {
            if (!State.TurnOrder.ActivePlayer.Equals(playerId))
                throw new InvalidOperationException(
                    $"Not your turn. Active: {State.TurnOrder.ActivePlayer.Value}");
        }

        private void EnsureNotFinished()
        {
            if (IsFinished)
                throw new InvalidOperationException("Match already finished.");
        }
    }
}
