using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Segment
{
    public Vector2 pt1;
    public Vector2 pt2;

    public Segment(Vector2 p1, Vector2 p2)
    {
        if(p1.x < p2.x)
        {
            pt1 = p1;
            pt2 = p2;
        }
        else if (p1.x > p2.x)
        {
            pt1 = p2;
            pt2 = p1;
        }
        else
        {
            if (p1.y < p2.y)
            {
                pt1 = p1;
                pt2 = p2;
            }
            else if(p1.y > p2.y)
            {
                pt1 = p2;
                pt2 = p1;
            }
            else
            {
                pt1 = p1;
                pt2 = p2;
            }
        }
        
    }

    public Vector2 GetIntersectionPointCoordinates(Segment s2)
    {
        float tmp = (s2.pt2.x - s2.pt1.x) * (pt2.y - pt1.y) - (s2.pt2.y - s2.pt1.y) * (pt2.x - pt1.x);

        if (tmp == 0)
            return Vector2.zero;

        float mu = ((pt1.x - s2.pt1.x) * (pt2.y - pt1.y) - (pt1.y - s2.pt1.y) * (pt2.x - pt1.x)) / tmp;

        return new Vector2(
            s2.pt1.x + (s2.pt2.x - s2.pt1.x) * mu,
            s2.pt1.y + (s2.pt2.y - s2.pt1.y) * mu
        );
    }
}

[RequireComponent(typeof(PolygonCollider2D))]
public class LightCollider : MonoBehaviour
{
    public bool debug = false;
    private SortedSet<Vector2> points = new SortedSet<Vector2>(new VectorComparer());
    public float vectorModifier = 0.001f;
    public int numberPoints = 16;
    public LayerMask type;

    // Start is called before the first frame update
    void Awake()
    {

    }

    public void SetPolygonFromCircle()
    {
        GetComponent<PolygonCollider2D>().pathCount = 1;
        float radius = GetComponent<NewLightSource>().lightRadius;
        List<Vector2> circlePolygon = new List<Vector2>();
        
        for (int i = 0; i < numberPoints; i++)
            circlePolygon.Add(new Vector2(Mathf.Cos(2 * i * Mathf.PI / (float)numberPoints), Mathf.Sin(2 * i * Mathf.PI / (float)numberPoints)));

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

    public void SetColliderOnOverlap2()
    {
        // Todo : 2 points per points, tri when added
        points.Clear();

        float radius = GetComponent<NewLightSource>().lightRadius;

        List<List<Segment>> listSegments = new List<List<Segment>>();
        List<Segment> listPolyBase = new List<Segment>();

        for (int i = 0; i < numberPoints; i++)
        {
            Vector2 prec = (Vector2)(transform.localToWorldMatrix * new Vector2(Mathf.Cos(2 * i * Mathf.PI / (float)numberPoints), Mathf.Sin(2 * i * Mathf.PI / (float)numberPoints)) / 2) + (Vector2)transform.position;
            Vector2 suiv = (Vector2)(transform.localToWorldMatrix * new Vector2(Mathf.Cos(2 * (i + 1) * Mathf.PI / (float)numberPoints), Mathf.Sin(2 * (i + 1) * Mathf.PI / (float)numberPoints)) / 2) + (Vector2)transform.position;

            if (debug) Debug.DrawLine(prec, suiv, Color.red);
            listPolyBase.Add(new Segment(prec, suiv));

            Add1Point((Vector2)(transform.localToWorldMatrix * new Vector2(Mathf.Cos(2 * i * Mathf.PI / (float)numberPoints), Mathf.Sin(2 * i * Mathf.PI / (float)numberPoints)) / 2) + (Vector2)transform.position);
        }   

        Collider2D[] tabColliders = Physics2D.OverlapCircleAll((Vector2)transform.position, radius, type);

        foreach (Collider2D col in tabColliders)
        {
            List<Segment> listColliderCol = new List<Segment>();

            if (col.GetComponent<BoxCollider2D>() != null)
            {
                BoxCollider2D box = col.GetComponent<BoxCollider2D>();

                Vector2 corner1 = (Vector2)(box.bounds.center + box.bounds.extents);
                Vector2 corner2 = (Vector2)(box.bounds.center - box.bounds.extents);
                Vector2 corner3 = (Vector2)box.bounds.center + new Vector2(box.bounds.extents.x, -box.bounds.extents.y);
                Vector2 corner4 = (Vector2)box.bounds.center + new Vector2(-box.bounds.extents.x, +box.bounds.extents.y);

                if (debug) Debug.DrawLine(corner1, corner3, Color.red);
                if (debug) Debug.DrawLine(corner1, corner4, Color.red);
                if (debug) Debug.DrawLine(corner2, corner3, Color.red);
                if (debug) Debug.DrawLine(corner2, corner4, Color.red);

                listColliderCol.Add(new Segment(corner1, corner3));
                listColliderCol.Add(new Segment(corner1, corner4));
                listColliderCol.Add(new Segment(corner2, corner3));
                listColliderCol.Add(new Segment(corner2, corner4));

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

                    Vector2 prec = (Vector2)(col.transform.localToWorldMatrix * col.GetComponent<PolygonCollider2D>().points[i]) + (Vector2)col.transform.position;
                    Vector2 suiv = (Vector2)(col.transform.localToWorldMatrix * col.GetComponent<PolygonCollider2D>().points[(i+1)% col.GetComponent<PolygonCollider2D>().points.Length]) + (Vector2)col.transform.position;

                    if (debug) Debug.DrawLine(prec, suiv, Color.blue);
                    listColliderCol.Add(new Segment(prec, suiv));
                }    
            }
            listSegments.Add(listColliderCol);
        }

        listSegments.Add(listPolyBase);

        for (int i = 0; i < tabColliders.Length; i++)
        {
            foreach (Segment s1 in listSegments[i])
            {
                foreach (Segment s2 in listSegments[listSegments.Count-1])
                {
                    Vector2 vec = s1.GetIntersectionPointCoordinates(s2);
                    if (vec != Vector2.zero && Vector2.Distance((Vector2)transform.position, vec) <= GetComponent<NewLightSource>().lightRadius)
                    {
                        Add2Points(vec);
                    }
                }
            }

            for (int j = 0; j < tabColliders.Length; j++)
            {
                if (i != j)
                {
                    foreach (Segment s1 in listSegments[i])
                    {
                        foreach (Segment s2 in listSegments[j])
                        {
                            Vector2 vec = s1.GetIntersectionPointCoordinates(s2);
                            if (vec != Vector2.zero && Vector2.Distance((Vector2)transform.position, vec) <GetComponent<NewLightSource>().lightRadius) Add2Points(vec);
                        }
                    }
                }
            }    
        }

        List<Vector2> circlePolygon = new List<Vector2>();

        foreach (Vector2 pt in points)
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position, (GetPosFromPoint(pt) - (Vector2)transform.position).normalized, GetComponent<NewLightSource>().lightRadius, type);
            if (ray)
                circlePolygon.Add(transform.worldToLocalMatrix * (ray.point - (Vector2)transform.position));
            else
                circlePolygon.Add(transform.worldToLocalMatrix * (((Vector2)transform.position + (GetPosFromPoint(pt) - (Vector2)transform.position).normalized * GetComponent<NewLightSource>().lightRadius) - (Vector2)transform.position));
        }

        GetComponent<PolygonCollider2D>().SetPath(0, circlePolygon.ToArray());
    }

    public Mesh CreateMeshFromCollider()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();


        for (int i = 0; i < poly.points.Length; i++)
            vertices.Add(poly.points[i]);

        vertices.Add(Vector3.zero);

        for (int i = 0; i < vertices.Count - 1; i++)
        {
            triangles.Add(i + 1);
            triangles.Add(i);
            triangles.Add(vertices.Count - 1);
        }

        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);
        triangles.Add(0);

        // Create the mesh
        var mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray()
        };

        mesh.RecalculateNormals();

        return mesh;
    }


    // Use to get a point from the sorted list
    Vector2 GetPosFromPoint(Vector2 pt)
    {
        return -(pt - (Vector2)transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        SetColliderOnOverlap2();
    }

    // Gizmos for Debug
    #if UNITY_EDITOR
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, GetComponent<NewLightSource>().lightRadius);
        if (debug)
        {
            int i = 0;
            foreach (Vector2 pt in points)
            {
                i++;
                //Gizmos.color = new Color((float)i / (float)points.Count, (float)i / (float)points.Count, (float)i / (float)points.Count, 1);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, GetPosFromPoint(pt));

                RaycastHit2D ray2 = Physics2D.Raycast(transform.position, (GetPosFromPoint(pt) - (Vector2)transform.position).normalized, GetComponent<NewLightSource>().lightRadius, type);
                if (ray2)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(transform.position, ray2.point);
                }
                else
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(transform.position, (Vector2)transform.position + (GetPosFromPoint(pt) - (Vector2)transform.position).normalized * GetComponent<NewLightSource>().lightRadius);
                }
            }
        }
    }
    
    #endif
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
