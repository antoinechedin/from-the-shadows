using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    public Material mat;
    public MeshRenderer mesh;
    public int materialIndex;
    public Color outlineColor;
    [Range(0, 0.5f)]public float alphaChange;

    private Material instanceMat;
    private float alpha;
    private bool animating = false;
    
    private void Awake()
    {
        List<Material> list = new List<Material>();
        mesh.GetMaterials(list);
        instanceMat = list[materialIndex];
        instanceMat.SetColor("_Color", outlineColor);
        instanceMat.SetFloat("_Alpha", 0);
        alpha = instanceMat.GetFloat("_Alpha");
        StartCoroutine(HideOutline());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(DisplayOutline());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine(HideOutline());
    }

    public IEnumerator DisplayOutline()
    {
        if (!animating)
        {
            animating = true;
            while(alpha < 1)
            {
                alpha = Mathf.Clamp01(alpha + alphaChange);
                instanceMat.SetFloat("_Alpha", alpha);
                yield return null;
            }
            animating = false;
        }
    }

    public IEnumerator HideOutline()
    {
        if (!animating)
        {
            animating = true;
            while (alpha > 0)
            {
                alpha = Mathf.Clamp01(alpha - alphaChange);
                instanceMat.SetFloat("_Alpha", alpha);
                yield return null;
            }
            animating = false;
        }
    }
}
