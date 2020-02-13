using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class DrawPolygonGizmos : MonoBehaviour
{
    public Color c;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    { 
        Gizmos.color = c;
        for (int j = 0; j < GetComponent<PolygonCollider2D>().pathCount; j++)
        {
            for (int i = 0; i < GetComponent<PolygonCollider2D>().GetPath(j).Length; i++)
            {
                Gizmos.DrawLine(transform.localToWorldMatrix * GetComponent<PolygonCollider2D>().GetPath(j)[i] + (Vector4)transform.position, transform.localToWorldMatrix * GetComponent<PolygonCollider2D>().GetPath(j)[(i + 1) % GetComponent<PolygonCollider2D>().GetPath(j).Length] + (Vector4)transform.position);
            }
        }
        
    }
}
