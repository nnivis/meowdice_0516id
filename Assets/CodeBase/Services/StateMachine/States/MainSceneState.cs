using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Services.StateMachine.States
{
    public class MainSceneState : StateMachineBehavior
    {
        [SerializeField] private MainSceneMode _mainSceneMode;
        [SerializeField] private Button _startButton;

        protected override void OnEnter()
        {
            _startButton.onClick.AddListener(OnStartClicked);
        }

        protected override void OnExit()
        {
            _startButton.onClick.RemoveListener(OnStartClicked);
        }

        private void OnStartClicked() => _mainSceneMode.GoToGame();
    }
}
