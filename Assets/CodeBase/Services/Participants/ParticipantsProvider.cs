using System;
using CodeBase.Infrastructure.DataProvider;
using CodeBase.Services.GameStart;

namespace CodeBase.Services.Participants
{
    public class ParticipantsProvider : IParticipantsProvider
    {
        private readonly ILocalPlayerDataProvider _local;
        private readonly IOpponentIdSource _opponent;

        public ParticipantsProvider(ILocalPlayerDataProvider local, IOpponentIdSource opponent)
        {
            _local = local;
            _opponent = opponent;
        }

        public MatchParticipants GetParticipants()
        {
            var localId = _local.GetLocalPlayerId();
            var opponentId = _opponent.GetOpponentId();

            if (localId.Equals(opponentId))
                throw new InvalidOperationException("Local and opponent PlayerId must be different.");

            return new MatchParticipants(localId, opponentId);
        }
    }
}