using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuStartButton : MonoBehaviour
{
    public Material mat;

    Text text;

    float dissolve;
    [Range(0f, 1f)]public float dissolveAmount;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        text.material = Instantiate(mat);
    }

    public IEnumerator FadeOut()
    {
        while(dissolve > 0)
        {
            dissolve = Mathf.Clamp01(dissolve - dissolveAmount);
            text.material.SetFloat("_Dissolution", dissolve);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator FadeIn()
    {
        while (dissolve < 1)
        {
            dissolve = Mathf.Clamp01(dissolve + dissolveAmount);
            text.material.SetFloat("_Dissolution", dissolve);
            yield return new WaitForSeconds(0.1f);
        }
    }
}        