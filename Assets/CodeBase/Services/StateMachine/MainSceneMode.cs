using CodeBase.Services.StateMachine.States;

namespace CodeBase.Services.StateMachine
{
    public class MainSceneMode : StateModeBehavior
    {
        public void GoToMain() => ChangeState<MainSceneState>();
        public void GoToGame() => ChangeState<GameplayState>();
        public void GoToGameResult() => ChangeState<GameResultState>();
    }
}
