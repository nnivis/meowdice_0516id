using System.Collections.Generic;
using CodeBase.Data.PlayerDataComponents;
using CodeBase.Domain.Match;
using CodeBase.Services.GameStart;

namespace CodeBase.Services.Turn.Controllers
{
    public interface IControllerFactory
    {
        IReadOnlyDictionary<PlayerId, IPlayerController> CreateControllers(GameStartConfig config, IMatchReadModel matchReadModel);
    }
}