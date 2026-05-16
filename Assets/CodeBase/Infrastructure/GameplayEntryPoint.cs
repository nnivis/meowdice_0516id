using VContainer.Unity;

namespace CodeBase.Infrastructure
{
    public sealed class GameplayEntryPoint : IStartable
    {
        private readonly BootstrapComponents _bootstrap;

        public GameplayEntryPoint(BootstrapComponents bootstrap) => _bootstrap = bootstrap;

        public void Start() => _bootstrap.Run();
    }
}