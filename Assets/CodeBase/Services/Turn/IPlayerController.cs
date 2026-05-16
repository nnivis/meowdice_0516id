using System.Threading;
using System.Threading.Tasks;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;

namespace CodeBase.Services.Turn
{
    public interface IPlayerController
    {
        Task<CellPosition> RequestPlacementAsync(PlayerId playerId, CancellationToken ct);
        void NotifyPlacementAccepted();
        void NotifyPlacementRejected();
    }
}
