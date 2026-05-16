using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Services.StateMachine
{
    public abstract class StateMachineBehavior : MonoBehaviour, IStateMachine
    {
        [SerializeField] private List<GameObject> _views;
        protected StateMachine StateMachine { private set; get; }

        void IStateMachine.Init(StateMachine stateMachine) => this.StateMachine = stateMachine;

        void IStateMachine.Enter()
        {
            gameObject.SetActive(true);
            OnEnter();
        }

        void IStateMachine.Exit()
        {
            gameObject.SetActive(false);
            OnExit();
        }

        public IReadOnlyList<GameObject> GetViews() => _views;

        protected virtual void OnExit()
        {
        }

        protected virtual void OnEnter()
        {
        }
    }
}
