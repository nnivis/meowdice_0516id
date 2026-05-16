
namespace CodeBase.Services.GameResult
{
    public class MatchResultStorage
    {
        public MatchResult Current { get; private set; }

        public void Set(MatchResult result)
        {
            Current = result;
        }

        public void Clear()
        {
            Current = null;
        }
    }
}