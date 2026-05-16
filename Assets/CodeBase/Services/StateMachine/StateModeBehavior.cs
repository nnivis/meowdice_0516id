using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Services.StateMachine
{
    public class StateModeBehavior : MonoBehaviour
    {
      
        [SerializeField] private List<StateMachineBehavior> _states;
        private StateMachine _stateMachine;
        private const int DefaultState = 0;

        public void Bootstrap()
        {
            if (_stateMachine != null) return;

            InitStates();

            var index = Mathf.Clamp(DefaultState, 0, _states.Count - 1);
            if (_stateMachine != null) _stateMachine.Change(_states[index].GetType());
        }

        public void Shutdown()
        {
            _stateMachine?.Release();
            _stateMachine = null;
        }

        private void OnDestroy() => _stateMachine.Release();

        private void InitStates()
        {
            _stateMachine = new StateMachine(_states);
            _states.ForEach(InitGameState);
        }

        private void InitGameState(StateMachineBehavior state)
        {
            foreach (var view in state.GetViews())
            {
                view.SetActive(false);
            }

            state.gameObject.SetActive(false);
        }

        protected void ChangeState<T>() where T : IStateMachine => _stateMachine.Change(typeof(T));
    }
}
