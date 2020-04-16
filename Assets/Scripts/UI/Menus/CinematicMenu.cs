using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicMenu : MonoBehaviour
{
    private const float FADE_OUT_DURATION = 1f;

    public Image foreground;
    public CanvasGroup skipText;
    [HideInInspector] public bool canSkip = false;
    private Coroutine skipTextCoroutine;

    public void Init()
    {
        SetForegroundAlpha(1);
        skipText.alpha = 0;
    }

    public void ShowSkipText()
    {
        skipTextCoroutine = StartCoroutine(ShowSkipTextCoroutine());
        canSkip = true;
    }

    private IEnumerator ShowSkipTextCoroutine()
    {
        float fadeDuration = 0.5f;
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            if (timer > fadeDuration)  timer = fadeDuration;

            float alpha = timer / fadeDuration;
            skipText.alpha = alpha;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(3f);

        timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            if (timer > fadeDuration) timer = fadeDuration;

            float alpha = 1 - timer / fadeDuration;
            skipText.alpha = alpha;
            yield return null;
        }

        canSkip = false;
        skipTextCoroutine = null;
    }

    public IEnumerator FadeOutCinematicMenuCoroutine()
    {
        if(skipTextCoroutine != null) StopCoroutine(skipTextCoroutine);
        skipText.alpha = 0;

        float timer = 0;
        while (timer < FADE_OUT_DURATION)
        {
            timer += Time.unscaledDeltaTime;
            if (timer > FADE_OUT_DURATION) timer = FADE_OUT_DURATION;

            float alpha = 1 - timer / FADE_OUT_DURATION;
            foreground.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        gameObject.SetActive(false);
    }

    public void SetForegroundAlpha(float alpha)
    {
        foreground.color = new Color(0, 0, 0, alpha);
    }
}
