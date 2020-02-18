using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(Rigidbody2D))]
public class NewLightSource : MonoBehaviour
{
    public float lightRadius = 3.5f;
    public Material lightMaterial;

    private void Awake()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    private void LateUpdate()
    {
        transform.localScale = new Vector3(lightRadius * 2, lightRadius * 2, 1);
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        Mesh m = Utils.CreateMesh2DFromPolyCollider(GetComponent<PolygonCollider2D>());
        GetComponent<MeshFilter>().sharedMesh = m;
        GetComponent<MeshRenderer>().material = lightMaterial;
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
