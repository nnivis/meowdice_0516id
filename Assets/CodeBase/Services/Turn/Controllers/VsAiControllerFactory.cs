using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Match;
using CodeBase.Services.GameStart;
using CodeBase.Services.Turn.Ai;

namespace CodeBase.Services.Turn.Controllers
{
    public class VsAiControllerFactory : IControllerFactory
    {
        private readonly IAiMoveStrategy _aiMoveStrategy;

        public VsAiControllerFactory(IAiMoveStrategy aiMoveStrategy)
        {
            _aiMoveStrategy = aiMoveStrategy;
        }

        public IReadOnlyDictionary<PlayerId, IPlayerController> CreateControllers(
            GameStartConfig config,
            IMatchReadModel matchReadModel)
        {
            var result = new Dictionary<PlayerId, IPlayerController>();

            result[config.Participants.Local] = new LocalHumanPlayerController();
            result[config.Participants.Opponent] = new AiController(matchReadModel, _aiMoveStrategy);

            return result;
        }
    }
}