using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    private const float START_OFFSET = 0f;
    private UIDissolve[] dissolves;
    public bool startDissolved = true;

    private void Awake()
    {
        dissolves = GetComponentsInChildren<UIDissolve>();
        if (startDissolved) foreach (UIDissolve dissolve in dissolves)
            {
                dissolve.effectFactor = 1;
            }
    }

    /// <summary>
    /// Set the dissolve effect factor for all UIDissovle children.
    /// </summary>
    /// <param name="value">The new effect factor value.</param>
    public void SetEffectFactor(float value)
    {
        foreach (UIDissolve dissolve in dissolves)
        {
            dissolve.effectFactor = value;
        }
    }

    /// <summary>
    /// Make the Gameobject appear with a Coroutine 
    /// </summary>
    /// <param name="duration">Effect duration in seconds</param>
    /// <returns></returns>
    public IEnumerator DissolveInCoroutine(float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            timer = timer >= duration ? duration : timer;

            foreach (UIDissolve dissolve in dissolves)
            {
                dissolve.effectFactor = 1f - (timer - timer * START_OFFSET) / duration;
            }
            yield return null;
        }
    }

    /// <summary>
    /// Make the Gameobject disappear with a Coroutine 
    /// </summary>
    /// <param name="duration">Effect duration in seconds</param>
    /// <returns></returns>
    public IEnumerator DissolveOutCoroutine(float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            timer = timer >= duration ? duration : timer;

            foreach (UIDissolve dissolve in dissolves)
            {
                dissolve.effectFactor = START_OFFSET + (timer - timer * START_OFFSET) / duration;
            }
            yield return null;
        }
    }
}
