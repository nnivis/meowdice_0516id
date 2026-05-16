using System;
using CodeBase.Data.PlayerDataComponents;

namespace CodeBase.Domain
{
    public class PlayerSlotResolver
    {
        private readonly PlayerId _localPlayer;
        private readonly PlayerId _opponentPlayer;

        public PlayerId LocalPlayer => _localPlayer;
        public PlayerId OpponentPlayer => _opponentPlayer;
        public PlayerSlotResolver(PlayerId localPlayer, PlayerId opponentPlayer)
        {
            _localPlayer = localPlayer;
            _opponentPlayer = opponentPlayer;
        }

        public PlayerSlot Resolve(PlayerId id)
        {
            if (id.Equals(_localPlayer))
                return PlayerSlot.Local;

            if (id.Equals(_opponentPlayer))
                return PlayerSlot.Opponent;

            throw new ArgumentException($"Unknown player {id}");
        }
    }
}