using System;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Match.Data;

namespace CodeBase.Services.GameResult
{
    public class MatchResult
    {
        public PlayerId? WinnerId { get; }
        public MatchPlayerInfo LocalPlayer { get; }
        public MatchPlayerInfo OpponentPlayer { get; }

        public MatchResult(
            MatchPlayerContext context,
            PlayerId? winnerId,
            int localScore,
            int opponentScore)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            WinnerId = winnerId;
            LocalPlayer = context.LocalPlayer.WithResult(localScore, IsWinner(context.LocalPlayer.PlayerId));
            OpponentPlayer = context.OpponentPlayer.WithResult(opponentScore, IsWinner(context.OpponentPlayer.PlayerId));
        }

        private bool IsWinner(PlayerId playerId) =>
            WinnerId.HasValue && WinnerId.Value.Equals(playerId);
    }
}
