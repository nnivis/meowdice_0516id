using CodeBase.Services.GameResult;
using UnityEngine;
using VContainer;

namespace CodeBase.Services.StateMachine.States
{
    public class GameResultState : StateMachineBehavior
    {
        [SerializeField] private MainSceneMode _mainSceneMode;
        [SerializeField] private GameResultPanel _gameResultPanel;

        [Inject] private MatchResultStorage _matchResultStorage;

        protected override void OnEnter()
        {
            var result = _matchResultStorage.Current;

            if (result == null)
            {
                Debug.LogError("GameResultState entered without MatchResult.");
                return;
            }

            _gameResultPanel.OnExitClicked += OnExitClicked;
            _gameResultPanel.Show(result);
        }

        private void OnExitClicked() => _mainSceneMode.GoToMain();

        protected override void OnExit()
        {
            _gameResultPanel.OnExitClicked -= OnExitClicked;
            _gameResultPanel.Hide();
            _matchResultStorage.Clear();
        }
    }
}
