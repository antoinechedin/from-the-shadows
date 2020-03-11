using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(Rigidbody2D))]
[RequireComponent(typeof(LightCollider))]
public class NewLightSource : MonoBehaviour
{
    public float lightRadius = 3.5f;
    private Material lightMaterial = null;

    private void Awake()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    private void Start()
    {
        //lightMaterial = new Material(Shader.Find("Shader Graphs/Ripple2"));
        //lightMaterial.SetFloat("Vector1_DF611158", lightRadius - 0.1f);
        //lightMaterial.SetColor("Color_95D64D8A", new Color(1, 0.7589114f, 0.3066038f, 0.09019608f));
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
        //lightMaterial = new Material(Shader.Find("Shader Graphs/Ripple2"));
        //lightMaterial.SetFloat("Vector1_DF611158", lightRadius - 0.1f);
        //lightMaterial.SetColor("Color_95D64D8A", new Color(1, 0.7589114f, 0.3066038f, 0.09019608f));
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

