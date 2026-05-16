using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Infrastructure.SceneLoad
{
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private Image _progressBar;
         private float _fadeDuration = 0.45f;

        private Coroutine _fade;

        public void Show()
        {
            if (_fade != null) StopCoroutine(_fade);

            gameObject.SetActive(true);

            _group.alpha = 1f;
            _group.blocksRaycasts = true;
            _group.interactable = true;

            SetProgress(0f);
        }

        public void SetProgress(float value)
        {
            if (_progressBar != null)
                _progressBar.fillAmount = Mathf.Clamp01(value);
        }

        public void Hide()
        {
            if (_fade != null) StopCoroutine(_fade);
            _fade = StartCoroutine(FadeOutAndDisable());
        }

        private IEnumerator FadeOutAndDisable()
        {
            float start = _group.alpha;
            float t = 0f;

            while (t < _fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                float k = _fadeDuration <= 0f ? 1f : t / _fadeDuration;
                _group.alpha = Mathf.Lerp(start, 0f, k);
                yield return null;
            }

            _group.alpha = 0f;
            _group.blocksRaycasts = false;
            _group.interactable = false;

            gameObject.SetActive(false);
        }
    }
}