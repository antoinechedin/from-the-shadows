using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[ExecuteInEditMode]
public class CircleToPolygon : MonoBehaviour
{
    public int numberPoints;
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<PolygonCollider2D>().pathCount = 1;
        float radius = GetComponent<NewLightSource>().lightRadius;
        List<Vector2> circlePolygon = new List<Vector2>();

        for (int i = 0; i < numberPoints; i++)
            circlePolygon.Add(new Vector2(Mathf.Cos(2 * i * Mathf.PI/ (float)numberPoints), Mathf.Sin(2 * i * Mathf.PI / (float)numberPoints)) / 2);

        GetComponent<PolygonCollider2D>().SetPath(0, circlePolygon.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
