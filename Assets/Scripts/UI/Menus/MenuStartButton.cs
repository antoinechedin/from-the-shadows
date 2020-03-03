using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuStartButton : MonoBehaviour
{
    public Material mat;

    Text text;

    public bool fading = false;
    float dissolve;
    [Range(0f, 1f)]public float dissolveAmount;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        text.material = Instantiate(mat);
    }

    public IEnumerator FadeOut()
    {
        fading = true;
        while (dissolve > 0)
        {
            dissolve = Mathf.Clamp01(dissolve - dissolveAmount);
            text.material.SetFloat("_Dissolution", dissolve);
            yield return null;
        }
        fading = false;
    }

    public IEnumerator FadeIn()
    {
        fading = true;
        while (dissolve < 1)
        {
            dissolve = Mathf.Clamp01(dissolve + dissolveAmount);
            text.material.SetFloat("_Dissolution", dissolve);
            yield return null;
        }
        fading = false;
    }
}        