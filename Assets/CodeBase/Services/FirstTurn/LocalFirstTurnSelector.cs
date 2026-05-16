using CodeBase.Data.PlayerDataComponents;
using CodeBase.Services.GameStart;

namespace CodeBase.Services.FirstTurn
{
    public class LocalFirstTurnSelector : IFirstTurnSelector
    {
        public PlayerId SelectFirstTurn(MatchParticipants participants)
            => participants.Local;
    }
}