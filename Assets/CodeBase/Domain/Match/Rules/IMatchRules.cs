using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Field.Cell;
using CodeBase.Domain.Match.Module;

namespace CodeBase.Domain.Match.Rules
{
    public interface IMatchRules
    {
        void ResolveAfterPlacement(MatchState state, PlayerId placedBy, Dice.Dice placedDice, CellPosition pos);
        MatchResult? TryGetResult(MatchState state);
        int GetPlayerScore(MatchState state, PlayerId playerId);
    }
}
