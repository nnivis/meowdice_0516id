using CodeBase.Data.PlayerDataComponents;

namespace CodeBase.Services.Participants
{
    public interface IOpponentIdSource
    {
        PlayerId GetOpponentId();
    }
}