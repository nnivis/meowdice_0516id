using CodeBase.Data.PlayerDataComponents;

namespace CodeBase.Domain.Match.Module
{
    public readonly struct MatchResult
    {
        public readonly PlayerId? Winner;
        public readonly bool IsDraw;

        private MatchResult(PlayerId? winner, bool isDraw)
        {
            Winner = winner;
            IsDraw = isDraw;
        }

        public static MatchResult Win(PlayerId winner) => new MatchResult(winner, false);
        public static MatchResult Draw() => new MatchResult(null, true);
    }
}