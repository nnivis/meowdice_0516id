using System;

namespace CodeBase.Infrastructure.SceneLoad
{
    public interface ISceneLoader
    {
        void Load(string sceneName, ILoadingScreen loadingScreen = null, Action onLoaded = null);
    }
    
}