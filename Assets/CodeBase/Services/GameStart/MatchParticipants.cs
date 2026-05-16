using CodeBase.Data.PlayerDataComponents;

namespace CodeBase.Services.GameStart
{
    public readonly struct MatchParticipants
    {
        public MatchParticipants(PlayerId local, PlayerId opponent)
        {
            Local = local;
            Opponent = opponent;
        }

        public PlayerId Local { get; }
        public PlayerId Opponent { get; }
    }
}