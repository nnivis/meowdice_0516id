using CodeBase.Domain.Match;
using CodeBase.Domain.Match.Content;
using CodeBase.Domain.Match.Rules;
using CodeBase.Domain.Match.Rules.ScoreCalculator;
using CodeBase.Infrastructure.DataProvider;
using CodeBase.Services;
using CodeBase.Services.FirstTurn;
using CodeBase.Services.GameResult;
using CodeBase.Services.GameStart;
using CodeBase.Services.Participants;
using CodeBase.Services.StateMachine;
using CodeBase.Services.StateMachine.States;
using CodeBase.Services.Turn.Ai;
using CodeBase.Services.Turn.Controllers;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure.VContainer
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterSceneComponents(builder);
            RegisterGameStartServices(builder);
            RegisterDomain(builder);
            RegisterTurnServices(builder);
            RegisterEntryPoints(builder);
        }

        private static void RegisterSceneComponents(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<BootstrapComponents>();
            builder.RegisterComponentInHierarchy<MainSceneMode>();
            builder.RegisterComponentInHierarchy<GameCoordinator>();
            builder.RegisterComponentInHierarchy<GameplayState>();
            builder.RegisterComponentInHierarchy<GameResultState>();
        }

        private static void RegisterGameStartServices(IContainerBuilder builder)
        {
            builder.Register<ILocalPlayerDataProvider, PersistentLocalPlayerDataProvider>(Lifetime.Singleton);
            builder.Register<IOpponentIdSource, AiOpponentIdSource>(Lifetime.Singleton);
            builder.Register<IParticipantsProvider, ParticipantsProvider>(Lifetime.Singleton);

            builder.Register<IFirstTurnSelector, LocalFirstTurnSelector>(Lifetime.Singleton);
            // builder.Register<IFirstTurnSelector, RandomFirstTurnSelector>(Lifetime.Singleton);

            builder.Register<GameStartConfigFactory>(Lifetime.Singleton);
            builder.Register<MatchResultStorage>(Lifetime.Singleton);
        }

        private static void RegisterDomain(IContainerBuilder builder)
        {
            builder.Register<IFieldScoreCalculator, FieldScoreCalculator>(Lifetime.Singleton);
            builder.Register<IMatchRules, DefaultMatchRules>(Lifetime.Singleton);
            builder.Register<MatchFactory>(Lifetime.Singleton);
            builder.Register<IOpponentFactory, OpponentFactory>(Lifetime.Singleton);
            
        }

        private static void RegisterTurnServices(IContainerBuilder builder)
        {
            builder.Register<IAiMoveStrategy, RandomAiMoveStrategy>(Lifetime.Singleton);
            builder.Register<IControllerFactory, VsAiControllerFactory>(Lifetime.Singleton);
        }

        private static void RegisterEntryPoints(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayEntryPoint>();
        }
    }
}