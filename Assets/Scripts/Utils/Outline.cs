using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    public Material mat;
    public MeshRenderer mesh;
    public int materialIndex;
    [ColorUsageAttribute(true, true)] public Color outlineColor;
    [Range(0, 0.5f)] public float alphaChange;

    private Material instanceMat;
    private float alpha;
    private int nPlayerIn;

    private void Awake()
    {
        List<Material> list = new List<Material>();
        mesh.GetMaterials(list);
        instanceMat = list[materialIndex];
        instanceMat.SetColor("_Color", outlineColor);
        instanceMat.SetFloat("_Alpha", 0);
        alpha = instanceMat.GetFloat("_Alpha");

        nPlayerIn = 0;
        StartCoroutine(HideOutline());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        nPlayerIn++;
        if (nPlayerIn > 0)
        {
            StartCoroutine(DisplayOutline());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        nPlayerIn--;
        if (nPlayerIn <= 0)
        {
            StartCoroutine(HideOutline());
        }
    }

    public IEnumerator DisplayOutline()
    {
        while (alpha < 1 && nPlayerIn > 0)
        {
            alpha = Mathf.Clamp01(alpha + alphaChange);
            instanceMat.SetFloat("_Alpha", alpha);
            yield return null;
        }
    }

    public IEnumerator HideOutline()
    {
        while (alpha > 0 && nPlayerIn == 0)
        {
            alpha = Mathf.Clamp01(alpha - alphaChange);
            instanceMat.SetFloat("_Alpha", alpha);
            yield return null;
        }
    }
}
