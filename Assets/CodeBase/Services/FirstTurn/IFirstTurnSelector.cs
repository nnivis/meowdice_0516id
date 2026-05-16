using CodeBase.Data.PlayerDataComponents;
using CodeBase.Services.GameStart;

namespace CodeBase.Services.FirstTurn
{
    public interface IFirstTurnSelector
    {
        PlayerId SelectFirstTurn(MatchParticipants participants);
    }
}