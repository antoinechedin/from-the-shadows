using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    public GameObject outlinedObject;
    [ColorUsageAttribute(true, true)] public Color outlineColor;
    [Range(0, 0.5f)] public float alphaChange;

    private List<Material> outlines;
    private float alpha;
    private int nPlayerIn;

    private void Awake()
    {
        outlines = FindOutlineMat();
        foreach (Material mat in outlines)
            InitMat(mat);
        
        alpha = 0;
        nPlayerIn = 0;
    }

    /// <summary>
    /// Initialize an outline mat with color and alpha = 0
    /// </summary>
    /// <param name="mat"></param>
    private void InitMat(Material mat)
    {
        mat.SetColor("_Color", outlineColor);
        mat.SetFloat("_Alpha", 0);        
    }

    /// <summary>
    /// Find and return array of each outline's material
    /// </summary>
    /// <returns></returns>
    private List<Material> FindOutlineMat()
    {
        List<Material> result = new List<Material>();
        List<MeshRenderer> meshes = new List<MeshRenderer>();
        meshes.AddRange(outlinedObject.gameObject.GetComponentsInChildren<MeshRenderer>());
        foreach (MeshRenderer meshRenderer in meshes)
        {
            List<Material> matBuffer = new List<Material>();
            meshRenderer.GetMaterials(matBuffer);

            foreach (Material mat in matBuffer)
            {
                if (mat.name.Contains("M_Outline"))
                {
                    result.Add(mat);
                }
            }
        }
        return result;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        nPlayerIn++;
        if (nPlayerIn > 0)
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(DisplayOutline());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        nPlayerIn--;
        if (nPlayerIn <= 0)
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(HideOutline());
            }
            nPlayerIn = 0;
        }
    }

    public IEnumerator DisplayOutline()
    {
        while (alpha < 1 && nPlayerIn > 0)
        {
            alpha = Mathf.Clamp01(alpha + alphaChange);
            foreach(Material mat in outlines)
                mat.SetFloat("_Alpha", alpha);
            yield return null;
        }
    }

    public IEnumerator HideOutline()
    {
        while (alpha > 0 && nPlayerIn == 0)
        {
            alpha = Mathf.Clamp01(alpha - alphaChange);
            foreach (Material mat in outlines)
                mat.SetFloat("_Alpha", alpha);
            yield return null;
        }
    }
}
