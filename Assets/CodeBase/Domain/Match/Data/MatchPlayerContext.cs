using System;
using CodeBase.Data.PlayerDataComponents;

namespace CodeBase.Domain.Match.Data
{
    public sealed class MatchPlayerContext
    {
        public MatchPlayerInfo LocalPlayer { get; }
        public MatchPlayerInfo OpponentPlayer { get; }

        public MatchPlayerContext(MatchPlayerInfo localPlayer, MatchPlayerInfo opponentPlayer)
        {
            LocalPlayer = localPlayer ?? throw new ArgumentNullException(nameof(localPlayer));
            OpponentPlayer = opponentPlayer ?? throw new ArgumentNullException(nameof(opponentPlayer));
        }

        public PlayerSlot ResolveSlot(PlayerId playerId)
        {
            if (playerId.Equals(LocalPlayer.PlayerId))
                return PlayerSlot.Local;

            if (playerId.Equals(OpponentPlayer.PlayerId))
                return PlayerSlot.Opponent;

            throw new ArgumentException($"Unknown player id: {playerId.Value}", nameof(playerId));
        }
        
    }
}