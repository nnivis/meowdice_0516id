using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;
using CodeBase.Domain.Match;

namespace CodeBase.Services.Turn.Ai
{
    public interface IAiMoveStrategy
    {
        CellPosition ChoosePlacement(IMatchReadModel match, PlayerId playerId);
    }
}