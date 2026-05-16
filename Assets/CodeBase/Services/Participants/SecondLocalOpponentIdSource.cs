using CodeBase.Data.PlayerDataComponents;

namespace CodeBase.Services.Participants
{
    public sealed class SecondLocalOpponentIdSource : IOpponentIdSource
    {
        public PlayerId GetOpponentId() => new PlayerId(2);
    }
}