using System;
using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void Wait(this MonoBehaviour monoBehaviour, float delay, Action action)
    {
        monoBehaviour.StartCoroutine(ExecuteAction(delay, action));
    }

    private static IEnumerator ExecuteAction(float delay, Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action?.Invoke();
        yield break;
    }
}
