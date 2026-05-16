using System.Collections;
using CodeBase.Infrastructure.DataProvider;
using CodeBase.Infrastructure.SceneLoad;

namespace CodeBase.Infrastructure
{
    public class StartupSequence : IStartupSequence
    {
        private readonly ILoadingScreen _loading;
        private readonly IDataBootstrap _dataBootstrap;
        private readonly ISceneLoader _sceneLoader;
        private readonly SceneCatalog _scenes;

        public StartupSequence(
            ILoadingScreen loading,
            IDataBootstrap dataBootstrap,
            ISceneLoader sceneLoader,
            SceneCatalog scenes)
        {
            _loading = loading;
            _dataBootstrap = dataBootstrap;
            _sceneLoader = sceneLoader;
            _scenes = scenes;
        }

        public IEnumerator Run()
        {
            _loading.Show();
            _loading.SetProgress(0f);

            _dataBootstrap.Initialize();
            _loading.SetProgress(0.2f);
            yield return null;
            
            _sceneLoader.Load(_scenes.Gameplay, _loading);
        }
    }
}