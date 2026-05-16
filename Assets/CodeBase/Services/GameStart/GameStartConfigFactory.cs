using System;
using CodeBase.Services.FirstTurn;
using CodeBase.Services.Participants;

namespace CodeBase.Services.GameStart
{
    public class GameStartConfigFactory
    {
        private readonly IParticipantsProvider _participantsProvider;
        private readonly IFirstTurnSelector _firstTurnSelector;

        public GameStartConfigFactory(
            IParticipantsProvider participantsProvider,
            IFirstTurnSelector firstTurnSelector)
        {
            _participantsProvider = participantsProvider;
            _firstTurnSelector = firstTurnSelector;
        }

        public GameStartConfig CreateConfig(GameMode mode)
        {
            var participants = _participantsProvider.GetParticipants();
            var firstTurn = _firstTurnSelector.SelectFirstTurn(participants);

            // Валидация на дубли
            if (participants.Local.Equals(participants.Opponent))
                throw new InvalidOperationException("Local and Opponent must be different PlayerIds.");

            return new GameStartConfig(mode, participants, firstTurn);
        }
    }
}