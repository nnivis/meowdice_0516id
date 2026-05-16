using System;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Services.GameStart;

namespace CodeBase.Services.FirstTurn
{
    public class RandomFirstTurnSelector : IFirstTurnSelector
    {
        private readonly Random _random = new();

        public PlayerId SelectFirstTurn(MatchParticipants participants)
            => _random.Next(0, 2) == 0 ? participants.Local : participants.Opponent;
    }
} 