using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dissolve : MonoBehaviour
{
    public Material mat;
    public Texture2D texture;
    public Component component;

    public Vector2 ratio = new Vector2(1f, 1f);

    public bool dissolving = false;
    [Range(0f, 1f)] public float dissolveAmount;
    public bool disableOnce;

    private float dissolve;    

    private void Start()
    {        
        ((MaskableGraphic)component).material = Instantiate(mat);
        dissolve = ((MaskableGraphic)component).material.GetFloat("_Dissolution");
        ((MaskableGraphic)component).material.SetVector("_Ratio", ratio);
    }

    public IEnumerator DissolveOut()
    {
        dissolving = true;
        while (dissolve > 0)
        {
            dissolve = Mathf.Clamp01(dissolve - dissolveAmount);
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
                dissolve = Mathf.Clamp01(dissolve + dissolveAmount);
                ((MaskableGraphic)component).material.SetFloat("_Dissolution", dissolve);
                yield return null;
            }
            dissolving = false;
        }
    }
}