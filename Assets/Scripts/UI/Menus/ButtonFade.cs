using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFade : MonoBehaviour
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
        dissolve = text.material.GetFloat("_Dissolution");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        Debug.Log("IN");
        fading = true;
        while (dissolve > 0)
        {
            dissolve = Mathf.Clamp01(dissolve - dissolveAmount);
            text.material.SetFloat("_Dissolution", dissolve);
            yield return null;
        }
        fading = false;
        Debug.Log("OUT");
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