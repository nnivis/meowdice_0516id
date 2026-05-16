using System.Collections;
using UnityEngine;

namespace CodeBase.Infrastructure.SceneLoad
{
    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        public new Coroutine StartCoroutine(IEnumerator coroutine) => base.StartCoroutine(coroutine);
    }
}