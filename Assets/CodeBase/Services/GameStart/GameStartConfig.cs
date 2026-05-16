using CodeBase.Data.PlayerDataComponents;

namespace CodeBase.Services.GameStart
{
    public readonly struct GameStartConfig
    {
        public GameStartConfig(GameMode mode, MatchParticipants participants, PlayerId firstTurnPlayer)
        {
            Mode = mode;
            Participants = participants;
            FirstTurnPlayer = firstTurnPlayer;
        }

        public GameMode Mode { get; }
        public MatchParticipants Participants { get; }
        public PlayerId FirstTurnPlayer { get; }
        
    }
}