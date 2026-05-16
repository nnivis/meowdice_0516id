using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.SceneLoad
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _runner;
        private float _followSpeed = 0.9f;
        private float _finishSpeed = 1.8f;


        public SceneLoader(ICoroutineRunner runner) => _runner = runner;

        public void Load(string sceneName, ILoadingScreen loadingScreen = null, Action onLoaded = null) =>
            _runner.StartCoroutine(LoadRoutine(sceneName, loadingScreen, onLoaded));

        private IEnumerator LoadRoutine(string sceneName, ILoadingScreen loadingScreen, Action onLoaded)
        {
            var op = SceneManager.LoadSceneAsync(sceneName);
            if (op != null)
            {
                op.allowSceneActivation = false;

                float shown = 0f;

                while (op.progress < 0.9f)
                {
                    float target = Mathf.Clamp01(op.progress / 0.9f);
                    shown = Mathf.MoveTowards(shown, target, Time.unscaledDeltaTime * _followSpeed);
                    loadingScreen?.SetProgress(shown);
                    yield return null;
                }

                while (shown < 1f)
                {
                    shown = Mathf.MoveTowards(shown, 1f, Time.unscaledDeltaTime * _finishSpeed);
                    loadingScreen?.SetProgress(shown);
                    yield return null;
                }

                op.allowSceneActivation = true;

                while (!op.isDone)
                    yield return null;
            }

            onLoaded?.Invoke();
            loadingScreen?.Hide();
        }
    }
}