using CodeBase.Data.PlayerDataComponents;


namespace CodeBase.Services.Participants
{
    public class AiOpponentIdSource : IOpponentIdSource
    {
        public PlayerId GetOpponentId() => new PlayerId(999);
    }
}