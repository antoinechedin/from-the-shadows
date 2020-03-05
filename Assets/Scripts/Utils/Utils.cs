using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utils is meant to be used only with static calls. It's a collection of 
/// useful functions that aren't provided by Unity.
/// </summary>
public class Utils
{
    /// <summary>
    /// Return the full name of a gameobjet in the scene hierachy.
    /// For example:
    /// |A
    /// |--B
    /// |--C
    /// |----D
    /// GetFullName(D) will return "A/C/D".
    /// This is mainly use for debug purpose.
    /// </summary>
    /// <param name="obj">the gameobject you want the full name</param>
    /// <returns></returns>
    public static string GetFullName(Transform obj)
    {
        if (obj.parent == null)
            return obj.name;
        else
            return GetFullName(obj.parent) + "/" + obj.name;
    }

    /// <summary>
    /// Create a 3D Mesh from a PolygonCollider2D
    /// </summary>
    /// <param name="polygonCollider">PolygonCollider to Mesherize</param>
    /// <returns></returns>
    public static Mesh CreateMesh3DFromPolyCollider(PolygonCollider2D polygonCollider, float frontDistance = -10, float backDistance = 10)
    {
        frontDistance = Mathf.Min(frontDistance, 0);
        backDistance = Mathf.Max(backDistance, 0);

        Mesh m = new Mesh();

        List<int> trix = new List<int>();
        List<Vector3> vertix = new List<Vector3>();
        List<Vector3> normix = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();

        int offset = 0;

        for (int j = 0; j < polygonCollider.pathCount; j++)
        {
            // convert polygon to triangles
            Triangulator triangulator = new Triangulator(polygonCollider.GetPath(j));
            int[] tris = triangulator.Triangulate();

            Vector3[] vertices = new Vector3[polygonCollider.GetPath(j).Length * 6];
            Vector3[] normals = new Vector3[polygonCollider.GetPath(j).Length * 6];
            Vector2[] localUvs = new Vector2[vertices.Length];

            for (int i = 0; i < polygonCollider.GetPath(j).Length; i++)
            {
                vertices[i].x = polygonCollider.GetPath(j)[i].x;
                vertices[i].y = polygonCollider.GetPath(j)[i].y;
                vertices[i].z = frontDistance; // front vertex
                localUvs[i] = new Vector2(vertices[i].x, vertices[i].y); //uv

                vertices[i + polygonCollider.GetPath(j).Length].x = polygonCollider.GetPath(j)[i].x;
                vertices[i + polygonCollider.GetPath(j).Length].y = polygonCollider.GetPath(j)[i].y;
                vertices[i + polygonCollider.GetPath(j).Length].z = backDistance;  // back vertex    
                localUvs[i + polygonCollider.GetPath(j).Length] = new Vector2(vertices[i + polygonCollider.GetPath(j).Length].x, vertices[i + polygonCollider.GetPath(j).Length].y); //uv

                vertices[i + 2 * polygonCollider.GetPath(j).Length].x = polygonCollider.GetPath(j)[i].x;
                vertices[i + 2 * polygonCollider.GetPath(j).Length].y = polygonCollider.GetPath(j)[i].y;
                vertices[i + 2 * polygonCollider.GetPath(j).Length].z = frontDistance; // front vertex
                localUvs[i + 2 * polygonCollider.GetPath(j).Length] = new Vector2(vertices[i + 2 * polygonCollider.GetPath(j).Length].x, vertices[i + 2 * polygonCollider.GetPath(j).Length].z); //uv
                vertices[i + 3 * polygonCollider.GetPath(j).Length].x = polygonCollider.GetPath(j)[i].x;
                vertices[i + 3 * polygonCollider.GetPath(j).Length].y = polygonCollider.GetPath(j)[i].y;
                vertices[i + 3 * polygonCollider.GetPath(j).Length].z = backDistance;  // back vertex  
                localUvs[i + 3 * polygonCollider.GetPath(j).Length] = new Vector2(vertices[i + 3 * polygonCollider.GetPath(j).Length].x, vertices[i + 3 * polygonCollider.GetPath(j).Length].z); //uv

                vertices[i + 4 * polygonCollider.GetPath(j).Length].x = polygonCollider.GetPath(j)[i].x;
                vertices[i + 4 * polygonCollider.GetPath(j).Length].y = polygonCollider.GetPath(j)[i].y;
                vertices[i + 4 * polygonCollider.GetPath(j).Length].z = frontDistance; // front vertex
                localUvs[i + 4 * polygonCollider.GetPath(j).Length] = new Vector2(vertices[i + 4 * polygonCollider.GetPath(j).Length].x, vertices[i + 4 * polygonCollider.GetPath(j).Length].z); //uv
                vertices[i + 5 * polygonCollider.GetPath(j).Length].x = polygonCollider.GetPath(j)[i].x;
                vertices[i + 5 * polygonCollider.GetPath(j).Length].y = polygonCollider.GetPath(j)[i].y;
                vertices[i + 5 * polygonCollider.GetPath(j).Length].z = backDistance;  // back vertex  
                localUvs[i + 5 * polygonCollider.GetPath(j).Length] = new Vector2(vertices[i + 5 * polygonCollider.GetPath(j).Length].x, vertices[i + 5 * polygonCollider.GetPath(j).Length].z); //uv
            }

            int[] triangles = new int[tris.Length * 2 + polygonCollider.GetPath(j).Length * 6];
            int count_tris = 0;

            for (int i = 0; i < tris.Length; i += 3)
            {
                triangles[i] = offset + tris[i];
                triangles[i + 1] = offset + tris[i + 1];
                triangles[i + 2] = offset + tris[i + 2];
            } // front vertices

            count_tris += tris.Length;

            for (int i = 0; i < tris.Length; i += 3)
            {
                triangles[count_tris + i] = offset + tris[i + 2] + polygonCollider.GetPath(j).Length;
                triangles[count_tris + i + 1] = offset + tris[i + 1] + polygonCollider.GetPath(j).Length;
                triangles[count_tris + i + 2] = offset + tris[i] + polygonCollider.GetPath(j).Length;
            } // back vertices

            count_tris += tris.Length;
            for (int i = 0; i < polygonCollider.GetPath(j).Length; i++)
            {
                // triangles around the perimeter of the object
                int n = (i + 1) % polygonCollider.GetPath(j).Length;

                triangles[count_tris] = offset + i + (2 + 2*(i%2))  * polygonCollider.GetPath(j).Length;
                triangles[count_tris + 1] = offset + n + (2 + 2 * (i % 2)) * polygonCollider.GetPath(j).Length;
                triangles[count_tris + 2] = offset + i + (3 + 2 * (i % 2)) * polygonCollider.GetPath(j).Length;

                triangles[count_tris + 3] = offset + n + (2 + 2 * (i % 2)) * polygonCollider.GetPath(j).Length;
                triangles[count_tris + 4] = offset + n + (3 + 2 * (i % 2)) * polygonCollider.GetPath(j).Length;
                triangles[count_tris + 5] = offset + i + (3 + 2 * (i % 2)) * polygonCollider.GetPath(j).Length;

                count_tris += 6;
            }

            normix.AddRange(normals);
            vertix.AddRange(vertices);
            trix.AddRange(triangles);
            uv.AddRange(localUvs);

            offset += polygonCollider.GetPath(j).Length * 6;
        }

        m.vertices = vertix.ToArray();
        m.triangles = trix.ToArray();

        m.RecalculateNormals();
        m.RecalculateBounds();

        m.uv = uv.ToArray();

        return m;
    }
}

public struct Segment
{
    public Vector2 pt1;
    public Vector2 pt2;

    public Segment(Vector2 p1, Vector2 p2)
    {
        if (p1.x < p2.x)
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
            else if (p1.y > p2.y)
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