using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClipperLib;

[RequireComponent(typeof(PolygonCollider2D))]
public class NewLayeredObstacle : MonoBehaviour
{
    public Material mat;
    public NewLayeredObstacleType type;
    private List<Vector2> baseCollider;
    private const long ClipperScale = 10000;

    [HideInInspector] public PolygonCollider2D childPolyCollider;
    [HideInInspector] public List<NewLightSource> lightSources;

    private void Awake()
    {
        // Add Real Collider to Children
        GameObject go = new GameObject();
        go.AddComponent<PolygonCollider2D>();
        go.AddComponent<MeshRenderer>();
        go.AddComponent<MeshFilter>();
        go.AddComponent<DrawPolygonGizmos>();
        go.name = "RealCollider";
        go.layer = LayerMask.NameToLayer("LayeredSolid");
        go.transform.SetParent(this.transform, false);

        // Setting up Gizmos Color
        DrawPolygonGizmos dpg = go.GetComponent<DrawPolygonGizmos>();
        if (type == NewLayeredObstacleType.Light)
            dpg.c = Color.yellow;
        else if (type == NewLayeredObstacleType.Shadow)
            dpg.c = Color.blue;

        // Initialize BaseCollider
        baseCollider = new List<Vector2>();
        childPolyCollider = transform.GetChild(0).GetComponent<PolygonCollider2D>();
        childPolyCollider.SetPath(0, new List<Vector2>().ToArray());
        for (int i = 0; i < GetComponent<PolygonCollider2D>().GetPath(0).Length; i++)
            baseCollider.Add(GetComponent<PolygonCollider2D>().GetPath(0)[i]);

        // This Object is Transparent, his Child is the real Collider
        gameObject.layer = LayerMask.NameToLayer("DisLayeredSolid");

        // Initialize LightSource & SpriteRenderer
        lightSources = new List<NewLightSource>();

        float width = GetComponent<PolygonCollider2D>().bounds.extents.x / 4;
        float height = GetComponent<PolygonCollider2D>().bounds.extents.y / 4;
        float depth = GetComponent<PolygonCollider2D>().bounds.extents.z / 4;
        mat.SetFloat("Vector1_A50F5B04", width);
        mat.SetFloat("Vector1_2568B206", height);
        mat.SetFloat("Vector1_6FEEDC3A", 0.5f);

        GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    private void Start() 
    {
        UpdateCollider();

    }

    private void Update()
    {
        foreach(NewLightSource nls in lightSources)
        {
            if (!nls.GetComponent<LightCollider>().GetStatic())
            {
                UpdateCollider();
                break;
            }
        }
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
        List<List<IntPoint>> solution = new List<List<IntPoint>>();
        Clipper clipper = new Clipper();

        List<IntPoint> subj = new List<IntPoint>();
        for (int j = 0; j < baseCollider.Count; j++)
            subj.Add(ConvertToIntPoint(transform.localToWorldMatrix * baseCollider[j] + (Vector4)transform.position));       

        foreach (PolygonCollider2D cl in clippers)
        {
            List<IntPoint> clip = new List<IntPoint>();
            for (int j = 0; j < cl.points.Length; j++)
            {
                clip.Add(ConvertToIntPoint(cl.transform.localToWorldMatrix * cl.points[j] + (Vector4)cl.transform.position));
            }
            clipper.AddPath(clip, PolyType.ptClip, true);
        }      
 
        clipper.AddPath(subj, PolyType.ptSubject, true);

        if(ct == ClipType.ctDifference)
            clipper.Execute(ct, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);

        if (ct == ClipType.ctIntersection)
            clipper.Execute(ct, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);

        childPolyCollider.pathCount = solution.Count;

        for (int i = 0; i < solution.Count; i++)
        {
            List<Vector2> unitedPolygon = new List<Vector2>();

            foreach (IntPoint point in solution[i])
                unitedPolygon.Add(transform.worldToLocalMatrix * (ConvertToVector2(point) - (Vector2)transform.position));

            childPolyCollider.SetPath(i, unitedPolygon.ToArray());
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
                    childPolyCollider.pathCount = 0;
                    childPolyCollider.SetPath(0, new List<Vector2>().ToArray());
                }
                // If there is Light
                else
                {
                    // Collider is Intersection between Light & baseCollider
                    childPolyCollider.pathCount = 1;
                    childPolyCollider.SetPath(0, baseCollider.ToArray());

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
                    childPolyCollider.pathCount = 1;
                    childPolyCollider.SetPath(0, baseCollider.ToArray());
                }
                // If there is Light
                else
                {
                    // Collider is Difference between baseCollider & Light
                    childPolyCollider.pathCount = 0;
                    childPolyCollider.SetPath(0, new List<Vector2>().ToArray());

                    List<PolygonCollider2D> listLight = new List<PolygonCollider2D>();
                    foreach (NewLightSource nls in lightSources)
                        listLight.Add(nls.GetComponent<PolygonCollider2D>());
                    SetColliderAs(ClipType.ctDifference, listLight);
                }
                break;
        }

        UpdateMesh();
    }
    private void UpdateMesh()
    {
        Mesh m = Utils.CreateMesh3DFromPolyCollider(transform.GetChild(0).GetComponent<PolygonCollider2D>(), -0.5f, +0.5f);
        GetComponentInChildren<MeshFilter>().sharedMesh = m;
        GetComponentInChildren<MeshRenderer>().material = mat;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (type == NewLayeredObstacleType.Light)
            Gizmos.color = new Color(1, 0.9f, 0.4f, 1f);
        else if (type == NewLayeredObstacleType.Shadow)
            Gizmos.color = new Color(0.045f, 0.045f, 0.12f, 1f);

        Gizmos.DrawCube(GetComponent<PolygonCollider2D>().bounds.center, GetComponent<PolygonCollider2D>().bounds.size + 3*Vector3.forward);
    }
    #endif

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
}

public enum NewLayeredObstacleType
{
    Light, Shadow
}
