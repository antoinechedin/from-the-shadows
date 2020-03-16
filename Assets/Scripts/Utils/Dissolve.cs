using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dissolve : MonoBehaviour
{
    public Component component;
    public Material mat;
    public Texture mainTex;
    public Vector2 ratio = new Vector2(1f, 1f);

    public bool dissolving = false;
    [Range(0f, 1f)] public float dissolveAmount;
    public bool disableOnce;

    private float dissolve;

    private void Awake()
    {
        if (component.GetType().Equals(typeof(TextMeshProUGUI)))
        {
            ((TextMeshProUGUI)component).fontMaterial = Instantiate(mat);
            ((TextMeshProUGUI)component).fontMaterial.SetVector("_Ratio", ratio);
            ((TextMeshProUGUI)component).fontMaterial.mainTexture = mainTex;
        }
        else
        {
            ((MaskableGraphic)component).material = Instantiate(mat);
            ((MaskableGraphic)component).material.SetVector("_Ratio", ratio);
            ((MaskableGraphic)component).material.mainTexture = mainTex;
        }
        dissolve = -3f;
    }

    public IEnumerator DissolveOut()
    {
        dissolving = true;
        while (dissolve > -3)
        {
            dissolve = Mathf.Clamp(dissolve - dissolveAmount, -6f, 2f);
            if (component.GetType().Equals(typeof(TextMeshProUGUI)))
                ((TextMeshProUGUI)component).fontMaterial.SetFloat("_Dissolution", dissolve);
            else
                ((MaskableGraphic)component).material.SetFloat("_Dissolution", dissolve);
            yield return null;
        }
        dissolving = false;
    }

    public IEnumerator DissolveIn()
    {
        if (disableOnce)
            disableOnce = false;
        else
        {
            dissolving = true;
            while (dissolve < 1)
            {
                dissolve = Mathf.Clamp(dissolve + dissolveAmount, -6f, 2f);
                if (component.GetType().Equals(typeof(TextMeshProUGUI)))
                    ((TextMeshProUGUI)component).fontMaterial.SetFloat("_Dissolution", dissolve);
                else
                    ((MaskableGraphic)component).material.SetFloat("_Dissolution", dissolve);
                yield return null;
            }
            dissolving = false;
        }
    }
}