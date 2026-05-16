using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Services.StateMachine
{
    public interface IStateMachine 
    {
        void Init(StateMachine stateMachine);
        void Enter();
        void Exit();
        IReadOnlyList<GameObject> GetViews();
    }
}
