using System.Threading;
using System.Threading.Tasks;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;
using CodeBase.Domain.Match;

namespace CodeBase.Services.Turn.Ai
{
    public class AiController : IPlayerController
    {
        private readonly IMatchReadModel _match;
        private readonly IAiMoveStrategy _strategy;

        public AiController(IMatchReadModel match, IAiMoveStrategy strategy)
        {
            _match = match;
            _strategy = strategy;
        }

        public async Task<CellPosition> RequestPlacementAsync(PlayerId playerId, CancellationToken ct)
        {
            await Task.Delay(500, ct);

            CellPosition position = _strategy.ChoosePlacement(_match, playerId);
            return position;
        }

        public void NotifyPlacementAccepted() { }
        public void NotifyPlacementRejected() { }
    }
}