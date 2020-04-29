using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(Rigidbody2D))]
[RequireComponent(typeof(LightCollider))]
public class NewLightSource : MonoBehaviour
{
    public float lightRadius = 3.5f;
    public Material lightMaterial;

    private void Awake()
    {
        lightMaterial = GetComponent<MeshRenderer>().material;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    private void Start()
    {
        UpdateMesh();
    }

    public void GoStatic()
    {
        GetComponent<LightCollider>().SetStatic(true);
        UpdateMesh();
    }

    private void Update()
    {
        if (!GetComponent<LightCollider>().GetStatic())
            UpdateMesh();
    }

    public void UpdateMesh()
    {
        Mesh m = GetComponent<LightCollider>().CreateMeshFromCollider();
        GetComponent<MeshFilter>().sharedMesh = m;
        GetComponent<MeshRenderer>().material = lightMaterial;
        // RippleDistance
        lightMaterial.SetFloat("Vector1_22436CA3", lightRadius - 0.1f);
        // Color
        lightMaterial.SetColor("Color_5C96105C", new Color(1, 0.75f, 0.30f, 0.15f));
        //lightMaterial.SetColor("Color_5C96105C", new Color(0.9433962f, 0.6325984f, 0.1662958f, 0.1f));
        // MinDistanc
        lightMaterial.SetFloat("Vector1_A69BC7A9", lightRadius - 1.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("LayeredSolid") || other.gameObject.layer == LayerMask.NameToLayer("DisLayeredSolid"))
        {
            NewLayeredObstacle newLayeredObstacle = other.GetComponent<NewLayeredObstacle>();
            if (newLayeredObstacle != null)
                newLayeredObstacle.AddLightSource(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("LayeredSolid") || other.gameObject.layer == LayerMask.NameToLayer("DisLayeredSolid"))
        {
            NewLayeredObstacle newLayeredObstacle = other.GetComponent<NewLayeredObstacle>();
            if (newLayeredObstacle != null)
                newLayeredObstacle.RemoveLightSource(this);
        }
    }
}

