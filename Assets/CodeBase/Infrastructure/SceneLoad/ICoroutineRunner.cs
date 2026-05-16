using System.Collections; 
using UnityEngine;

namespace CodeBase.Infrastructure.SceneLoad
{
    public interface ICoroutineRunner
    {
        public Coroutine StartCoroutine(IEnumerator coroutine);
    }
}