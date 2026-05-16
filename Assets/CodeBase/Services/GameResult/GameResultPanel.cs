using System;
using CodeBase.Domain.Character.CatSkin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Services.GameResult
{
    public class GameResultPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resultLabel;
        [SerializeField] private Button _exitButton;

        [Space]
        [SerializeField] private CatSkinView _catSkinViewPrefab;
        [SerializeField] private RectTransform _localPlayerRoot;
        [SerializeField] private RectTransform _opponentPlayerRoot;

        [Space]
        [SerializeField] private Color _winnerScoreColor;
        [SerializeField] private Color _loserScoreColor;

        private CatSkinView _localPlayerView;
        private CatSkinView _opponentPlayerView;

        public event Action OnExitClicked;

        public void Show(MatchResult result)
        {
            _localPlayerView = Instantiate(_catSkinViewPrefab, _localPlayerRoot);
            _localPlayerView.Apply(result.LocalPlayer.SkinId, result.LocalPlayer.Name);

            _opponentPlayerView = Instantiate(_catSkinViewPrefab, _opponentPlayerRoot);
            _opponentPlayerView.Apply(result.OpponentPlayer.SkinId, result.OpponentPlayer.Name);

            _resultLabel.text = BuildResultText(result);

            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        public void Hide()
        {
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);

            if (_localPlayerView != null)
                Destroy(_localPlayerView.gameObject);

            if (_opponentPlayerView != null)
                Destroy(_opponentPlayerView.gameObject);

            _localPlayerView = null;
            _opponentPlayerView = null;
        }

        private string BuildResultText(MatchResult result)
        {
            if (!result.WinnerId.HasValue)
                return "~ DRAW ~";

            var winner = result.LocalPlayer.Winner ? result.LocalPlayer : result.OpponentPlayer;
            var loser = result.LocalPlayer.Winner ? result.OpponentPlayer : result.LocalPlayer;

            var winnerHex = ColorUtility.ToHtmlStringRGB(_winnerScoreColor);
            var loserHex = ColorUtility.ToHtmlStringRGB(_loserScoreColor);

            return $"~ {winner.Name} WINS <color=#{winnerHex}>{winner.FinalScore}</color>-<color=#{loserHex}>{loser.FinalScore}</color> ~";
        }

        private void OnExitButtonClicked() => OnExitClicked?.Invoke();
    }
}
