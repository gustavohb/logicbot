using System;
using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static Coroutine Wait(this MonoBehaviour monoBehaviour, float delay, Action action)
    {
        return monoBehaviour.StartCoroutine(ExecuteAction(delay, action));
    }

    private static IEnumerator ExecuteAction(float delay, Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action?.Invoke();
        yield break;
    }
}
