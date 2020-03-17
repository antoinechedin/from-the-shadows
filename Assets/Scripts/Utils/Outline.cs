using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    public Material mat;
    public MeshRenderer mesh;
    public int materialIndex;
    public Color outlineColor;

    private Material instanceMat;

    private void Awake()
    {
        List<Material> list = new List<Material>();
        mesh.GetMaterials(list);
        instanceMat = list[materialIndex];
        instanceMat.SetColor("_Color", outlineColor);
        HideOutline();
    }

    public void DisplayOutline()
    {
        instanceMat.SetFloat("_Alpha", 1);
    }

    public void HideOutline()
    {
        instanceMat.SetFloat("_Alpha", 0);
    }
}
