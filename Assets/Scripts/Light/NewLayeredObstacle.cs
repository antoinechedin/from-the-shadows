using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClipperLib;

[RequireComponent(typeof(PolygonCollider2D))]
public class NewLayeredObstacle : MonoBehaviour
{
    private const long ClipperScale = 10000;

    private Vector2 ConvertToVector2(IntPoint value)
    {
        var x = (float)value.X / ClipperScale;
        var y = (float)value.Y / ClipperScale;
        return new Vector2(x, y);
    }

    private IntPoint ConvertToIntPoint(Vector2 value)
    {
        var x = (long)((double)value.x * ClipperScale);
        var y = (long)((double)value.y * ClipperScale);
        return new IntPoint(x, y);
    }

    //---------------------------------------------------------------------------------------------

    public PolygonCollider2D polyCollider;

    public NewLayeredObstacleType type;
    [HideInInspector] public List<NewLightSource> lightSources;
    public GameObject RealCollider;

    private List<Vector2> baseCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Add Real Collider to Children
        GameObject go = Instantiate(RealCollider, transform.position, Quaternion.identity);
        go.name = "RealCollider";
        go.transform.parent = transform;
        go.layer = LayerMask.NameToLayer("LayeredObstacle");

        // Setting up Gizmos Color
        DrawPolygonGizmos dpg = go.GetComponent<DrawPolygonGizmos>();
        if (type == NewLayeredObstacleType.Light)
            dpg.c = Color.yellow;
        else if (type == NewLayeredObstacleType.Shadow)
            dpg.c = Color.blue;

        // Initialize BaseCollider
        baseCollider = new List<Vector2>();
        polyCollider = transform.GetChild(0).GetComponent<PolygonCollider2D>();
        polyCollider.SetPath(0, new List<Vector2>().ToArray());
        for (int i = 0; i < GetComponent<PolygonCollider2D>().GetPath(0).Length; i++)
            baseCollider.Add(GetComponent<PolygonCollider2D>().GetPath(0)[i]);

        // This Object is Transparent, his Child is the real Collider
        gameObject.layer = LayerMask.NameToLayer("TransparentObstacle");

        // Initialize LightSource & SpriteRenderer
        lightSources = new List<NewLightSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() 
    {
        UpdateCollider();
    }

    private void Update()
    {
        if(lightSources.Count > 0)
            UpdateCollider();
    }

    public void AddLightSource(NewLightSource lightSource)
    {
        lightSources.Add(lightSource);
        UpdateCollider();
    }

    public void RemoveLightSource(NewLightSource lightSource)
    {
        lightSources.Remove(lightSource);
        UpdateCollider();
    }

    /// <summary>
    /// Sets the Collider in Children as Difference or Intersection from baseCollider with Clippers
    /// </summary>
    /// <param name="ct">ct.Intersection or ct.Difference based on Operation you want to do</param>
    /// <param name="clippers">list of Polygons to intersect/difference the baseCollider</param>
    void SetColliderAs(ClipType ct, List<PolygonCollider2D> clippers)
    {
        List<IntPoint> subj = new List<IntPoint>();
        for (int j = 0; j < baseCollider.Count; j++)
            subj.Add(ConvertToIntPoint(transform.localToWorldMatrix * baseCollider[j] + (Vector4)transform.position));       

        List<IntPoint> clip = new List<IntPoint>();
        foreach (PolygonCollider2D cl in clippers)
            for (int j = 0; j < cl.points.Length; j++)
                clip.Add(ConvertToIntPoint(cl.transform.localToWorldMatrix * cl.points[j] + (Vector4)cl.transform.position));

        List<List<IntPoint>> solution = new List<List<IntPoint>>();
        Clipper clipper = new Clipper();

        clipper.AddPath(subj, PolyType.ptSubject, true);
        clipper.AddPath(clip, PolyType.ptClip, true);
        clipper.Execute(ct, solution, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);

        polyCollider.pathCount = solution.Count;

        for (int i = 0; i < solution.Count; i++)
        {
            List<Vector2> unitedPolygon = new List<Vector2>();

            foreach (IntPoint point in solution[i])
                unitedPolygon.Add(transform.worldToLocalMatrix * (ConvertToVector2(point) - (Vector2)transform.position));

            polyCollider.SetPath(i, unitedPolygon.ToArray());
        }
    }

    private void UpdateCollider()
    {
        switch (type)
        {
            // For Light Obstacles
            case NewLayeredObstacleType.Light:
                // If there is no Light
                if (lightSources.Count == 0)
                {
                    // No Collider
                    polyCollider.pathCount = 0;
                    polyCollider.SetPath(0, new List<Vector2>().ToArray());
                }
                // If there is Light
                else
                {
                    // Collider is Intersection between Light & baseCollider
                    polyCollider.pathCount = 1;
                    polyCollider.SetPath(0, baseCollider.ToArray());

                    List<PolygonCollider2D> listLight = new List<PolygonCollider2D>();
                    foreach (NewLightSource nls in lightSources)
                        listLight.Add(nls.GetComponent<PolygonCollider2D>());
                    SetColliderAs(ClipType.ctIntersection, listLight);
                }
                break;

            // For Shadow Obstacles
            case NewLayeredObstacleType.Shadow:
                // If there is no Light
                if (lightSources.Count == 0)
                {
                    // Collider is baseCollider
                    polyCollider.pathCount = 1;
                    polyCollider.SetPath(0, baseCollider.ToArray());
                }
                // If there is Light
                else
                {
                    // Collider is Difference between baseCollider & Light
                    polyCollider.pathCount = 0;
                    polyCollider.SetPath(0, new List<Vector2>().ToArray());

                    List<PolygonCollider2D> listLight = new List<PolygonCollider2D>();
                    foreach (NewLightSource nls in lightSources)
                        listLight.Add(nls.GetComponent<PolygonCollider2D>());
                    SetColliderAs(ClipType.ctDifference, listLight);
                }
                break;
        }

        if (spriteRenderer != null)
        {
            Color newColor = spriteRenderer.color;
            if (gameObject.layer == LayerMask.NameToLayer("LayeredObstacle")) newColor.a = 1f;
            if (gameObject.layer == LayerMask.NameToLayer("TransparentObstacle")) newColor.a = 0.1f;
            spriteRenderer.color = newColor;
        }
    }
}

public enum NewLayeredObstacleType
{
    Light, Shadow
}
