using CodeBase.Data.PlayerDataComponents;

namespace CodeBase.Services.Participants
{
    public interface IOnlineLobby
    {
        CodeBase.Data.PlayerDataComponents.PlayerId LocalPlayerId { get; }
        CodeBase.Data.PlayerDataComponents.PlayerId RemotePlayerId { get; }
    }

    public sealed class OnlineOpponentIdSource : IOpponentIdSource
    {
        private readonly IOnlineLobby _lobby;
        public OnlineOpponentIdSource(IOnlineLobby lobby) => _lobby = lobby;

        public PlayerId GetOpponentId() => _lobby.RemotePlayerId;
    }
}