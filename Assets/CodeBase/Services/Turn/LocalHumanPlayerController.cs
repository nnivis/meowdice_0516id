using System;
using System.Threading;
using System.Threading.Tasks;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Services.Turn
{
    public class LocalHumanPlayerController : IPlayerController
    {
        public event Action PlacementRejected;
        public event Action PlacementAccepted;

        public bool IsWaitingForPlacement => _placementTcs != null && !_placementTcs.Task.IsCompleted;

        private TaskCompletionSource<CellPosition> _placementTcs;

        public Task<CellPosition> RequestPlacementAsync(PlayerId playerId, CancellationToken ct)
        {
            _placementTcs = new TaskCompletionSource<CellPosition>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            ct.Register(() => _placementTcs.TrySetCanceled(ct));

            return _placementTcs.Task;
        }

        public bool TrySubmitPlacement(CellPosition position)
        {
            if (!IsWaitingForPlacement)
                return false;

            return _placementTcs.TrySetResult(position);
        }

        public void NotifyPlacementRejected() => PlacementRejected?.Invoke();
        public void NotifyPlacementAccepted() => PlacementAccepted?.Invoke();

        public void CancelPendingPlacement()
        {
            if (!IsWaitingForPlacement)
                return;

            _placementTcs.TrySetCanceled();
        }
    }
}