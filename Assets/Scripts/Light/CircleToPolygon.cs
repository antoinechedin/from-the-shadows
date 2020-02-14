using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class CircleToPolygon : MonoBehaviour
{
    private SortedSet<Vector2> points = new SortedSet<Vector2>(new VectorComparer());
    public float vectorModifier = 0.01f;
    public int numberPoints;
    public LayerMask type;
    // Start is called before the first frame update
    void Awake()
    {
        SetPolygonFromCircle();
    }

    public void SetPolygonFromCircle()
    {
        GetComponent<PolygonCollider2D>().pathCount = 1;
        float radius = GetComponent<NewLightSource>().lightRadius;
        List<Vector2> circlePolygon = new List<Vector2>();
        for (int i = 0; i < numberPoints; i++)
        {
            circlePolygon.Add(new Vector2(Mathf.Cos(2 * i * Mathf.PI / (float)numberPoints), Mathf.Sin(2 * i * Mathf.PI / (float)numberPoints)) / 2);
        }

        GetComponent<PolygonCollider2D>().SetPath(0, circlePolygon.ToArray());
    }

    // Add two Points with offset to the Sorted List
    void Add2Points(Vector2 pointToAdd)
    {
        Vector3 direction = (Vector2)pointToAdd - (Vector2)transform.position;
        Vector3 dirOffset = Vector3.Cross(direction, new Vector3(0, 0, 1));

        points.Add((Vector2)transform.position - (pointToAdd + (Vector2)dirOffset * vectorModifier));
        points.Add((Vector2)transform.position - (pointToAdd - (Vector2)dirOffset * vectorModifier));
    }

    // Add a Point to the Sorted List
    void Add1Point(Vector2 pointToAdd)
    {
        points.Add((Vector2)transform.position - pointToAdd);
    }

    // Set the Collider of the GameObject from raycast Circle to Polygon and surronding plateforms
    public void SetColliderOnOverlap()
    {
        // Todo : 2 points per points, tri when added
        points.Clear();

        float radius = GetComponent<NewLightSource>().lightRadius;
        for (int i = 0; i < numberPoints; i++)
            Add1Point((Vector2)(transform.localToWorldMatrix * new Vector2(Mathf.Cos(2 * i * Mathf.PI / (float)numberPoints), Mathf.Sin(2 * i * Mathf.PI / (float)numberPoints)) / 2) + (Vector2)transform.position);

        Collider2D[] tabColliders = Physics2D.OverlapCircleAll((Vector2)transform.position, radius * 1.5f, type);

        foreach (Collider2D col in tabColliders)
        {
            if (col.GetComponent<BoxCollider2D>() != null)
            {
                BoxCollider2D box = col.GetComponent<BoxCollider2D>();
                Add2Points((Vector2)(box.bounds.center + box.bounds.extents));
                Add2Points((Vector2)(box.bounds.center - box.bounds.extents));
                Add2Points((Vector2)box.bounds.center + new Vector2(box.bounds.extents.x, -box.bounds.extents.y));
                Add2Points((Vector2)box.bounds.center + new Vector2(-box.bounds.extents.x, +box.bounds.extents.y));
            }
            else if (col.GetComponent<PolygonCollider2D>() != null)
            {
                for (int i = 0; i < col.GetComponent<PolygonCollider2D>().points.Length; i++)
                {
                    Add2Points((Vector2)(col.transform.localToWorldMatrix * col.GetComponent<PolygonCollider2D>().points[i]) + (Vector2)col.transform.position);
                }  
            }     
        }

        List<Vector2> circlePolygon = new List<Vector2>();

        foreach (Vector2 pt in points)
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position, (GetPosFromPoint(pt) - (Vector2)transform.position).normalized, GetComponent<NewLightSource>().lightRadius * 1.5f, type);
            if (ray)
                circlePolygon.Add(transform.worldToLocalMatrix * (ray.point - (Vector2)transform.position));
            else
                circlePolygon.Add(transform.worldToLocalMatrix * (((Vector2)transform.position + (GetPosFromPoint(pt) - (Vector2)transform.position).normalized * GetComponent<NewLightSource>().lightRadius * 1.5f) - (Vector2)transform.position));
        }

        GetComponent<PolygonCollider2D>().SetPath(0, circlePolygon.ToArray());
    }

    // Use to get a point from the sorted list
    Vector2 GetPosFromPoint(Vector2 pt)
    {
        return -(pt - (Vector2)transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        SetColliderOnOverlap();
    }

    /*
    // Gizmos for Debug
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, GetComponent<NewLightSource>().lightRadius * 1.5f);
        int i = 0;
        foreach (Vector2 pt in points)
        {
            i++;
            //Gizmos.color = new Color((float)i / (float)points.Count, (float)i / (float)points.Count, (float)i / (float)points.Count, 1);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, GetPosFromPoint(pt));

            RaycastHit2D ray2 = Physics2D.Raycast(transform.position, (GetPosFromPoint(pt) - (Vector2)transform.position).normalized, GetComponent<NewLightSource>().lightRadius * 1.5f, type);
            if (ray2)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, ray2.point);
            }
            else
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, (Vector2)transform.position + (GetPosFromPoint(pt) - (Vector2)transform.position).normalized * GetComponent<NewLightSource>().lightRadius * 1.5f);
            }
        }
    }
    #endif
    */
}

// Comparer for the Sorted List
public class VectorComparer : IComparer<Vector2>
{
    float xExt, yExt;

    public int Compare(Vector2 x, Vector2 y)
    {
        // Parse the extension from the file name. 
        xExt = Vector2.SignedAngle(Vector2.right, x);
        yExt = Vector2.SignedAngle(Vector2.right, y);

        // Compare the file extensions. 
        if (xExt < yExt)
            return -1;
        else if (xExt > yExt)
            return 1;
        else
            return 0;
    }
}
