using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(Rigidbody2D))]
[RequireComponent(typeof(LightCollider))]
public class NewLightSource : MonoBehaviour
{
    public float lightRadius = 3.5f;
    public Material lightMaterial;

    private void Awake()
    {
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
        if(!GetComponent<LightCollider>().GetStatic())
            UpdateMesh();
    }

    public void UpdateMesh()
    {
        Mesh m = GetComponent<LightCollider>().CreateMeshFromCollider();
        GetComponent<MeshFilter>().sharedMesh = m;
        GetComponent<MeshRenderer>().material = lightMaterial;

        if(GetComponent<MeshRenderer>().material.HasProperty("_RippleDistance"))
            GetComponent<MeshRenderer>().material.SetFloat("_RippleDistance", lightRadius - 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("LayeredSolid") || other.gameObject.layer == LayerMask.NameToLayer("DisLayeredSolid"))
        {
            other.GetComponent<NewLayeredObstacle>().AddLightSource(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("LayeredSolid") || other.gameObject.layer == LayerMask.NameToLayer("DisLayeredSolid"))
        {
            other.GetComponent<NewLayeredObstacle>().RemoveLightSource(this);
        }
    }
}
