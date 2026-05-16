using CodeBase.Infrastructure.DataProvider;
using CodeBase.Infrastructure.SceneLoad;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure.VContainer
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private Bootstrapper _bootstrapper;
        [SerializeField] private CoroutineRunner _coroutineRunner;
        [SerializeField] private LoadingScreen _loadingScreen;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterInfrastructure(builder);
            RegisterBootstrap(builder);
            RegisterPersistentData(builder);
        }

        protected override void Awake()
        {
            base.Awake();
            
            DontDestroyOnLoad(gameObject);
            _bootstrapper.Run();
        }

        private void RegisterInfrastructure(IContainerBuilder builder)
        {
            builder.RegisterInstance(new SceneCatalog());

            builder.RegisterComponent(_coroutineRunner).As<ICoroutineRunner>();
            builder.RegisterComponent(_loadingScreen).As<ILoadingScreen>();

            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
        }

        private void RegisterBootstrap(IContainerBuilder builder)
        {
            builder.RegisterComponent(_bootstrapper);

            builder.Register<IDataBootstrap, DataBootstrap>(Lifetime.Singleton);
            builder.Register<IStartupSequence, StartupSequence>(Lifetime.Singleton);
        }

        private static void RegisterPersistentData(IContainerBuilder builder)
        {
            builder.Register<IPersistentData, PersistentData>(Lifetime.Singleton);
            builder.Register<IDataProvider, DataLocalProvider>(Lifetime.Singleton);
        }
    }
}