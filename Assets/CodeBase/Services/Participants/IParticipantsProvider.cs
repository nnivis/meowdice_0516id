using CodeBase.Services.GameStart;

namespace CodeBase.Services.Participants
{
    public interface IParticipantsProvider
    {
        MatchParticipants GetParticipants();
    }
}