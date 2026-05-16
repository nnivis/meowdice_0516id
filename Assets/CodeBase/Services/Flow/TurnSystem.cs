using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;
using CodeBase.Domain.Match;
using CodeBase.Services.Turn;

namespace CodeBase.Services.Flow
{
    public class TurnSystem
    {
        private readonly Match _match;
        private readonly IReadOnlyDictionary<PlayerId, IPlayerController> _controllers;
        private readonly int _maxAttempts;

        public TurnSystem(Match match, IReadOnlyDictionary<PlayerId, IPlayerController> controllers, int maxAttempts)
        {
            _match = match ?? throw new ArgumentNullException(nameof(match));
            _controllers = controllers ?? throw new ArgumentNullException(nameof(controllers));
            _maxAttempts = Math.Max(1, maxAttempts);
        }

        public async Task PlayTurnAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                PlayerId player = _match.ActivePlayer;
                IPlayerController ctrl = _controllers[player];

                bool started = _match.IsStartTurn(player);

                if (!started)
                {
                    if (_match.IsFinished)
                        return;

                    continue;
                }

                while (!ct.IsCancellationRequested)
                {
                    CellPosition pos = await ctrl.RequestPlacementAsync(player, ct);

                    if (_match.TryPlaceDice(player, pos))
                    {
                        ctrl.NotifyPlacementAccepted();

                        if (!_match.IsFinished)
                            _match.EndTurn();

                        return;
                    }

                    ctrl.NotifyPlacementRejected();
                }
            }
        }
    }
}
