using System;
using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;

namespace CodeBase.Domain.Match.Module
{
    public class TurnOrder
    {
        private readonly IReadOnlyList<PlayerId> _players;
        private int _index;

        public PlayerId ActivePlayer => _players[_index];
        public IReadOnlyList<PlayerId> Players => _players;

        public TurnOrder(IReadOnlyList<PlayerId> players, PlayerId first)
        {
            _players = players ?? throw new ArgumentNullException(nameof(players));
            if (_players.Count == 0) throw new ArgumentException("Players empty.");

            _index = 0;
            for (int i = 0; i < _players.Count; i++)
                if (_players[i].Equals(first))
                    _index = i;
        }

        public void Next() => _index = (_index + 1) % _players.Count;
    }
}