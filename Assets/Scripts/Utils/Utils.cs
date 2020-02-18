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

    public static Mesh CreateMesh2DFromPolyCollider(PolygonCollider2D polygonCollider)
    {
        List<int> indix = new List<int>();
        List<Vector3> vertix = new List<Vector3>();

        int offset = 0;

        for (int j = 0; j < polygonCollider.pathCount; j++)
        {
            var vertices2D = new List<Vector2>();
            for (int i = 0; i < polygonCollider.GetPath(j).Length; i++)
                vertices2D.Add(polygonCollider.GetPath(j)[i]);

            var vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D.ToArray(), v => v);

            // Use the triangulator to get indices for creating triangles
            var triangulator = new Triangulator(vertices2D.ToArray());
            var indices = triangulator.Triangulate();

            for (int k = 0; k < indices.Length; k++)
                indices[k] += offset;

            vertix.AddRange(vertices3D);
            indix.AddRange(indices);

            offset += polygonCollider.GetPath(j).Length;
        }

        // Create the mesh
        var mesh = new Mesh
        {
            vertices = vertix.ToArray(),
            triangles = indix.ToArray()
        };

        mesh.RecalculateNormals();

        return mesh;
    }

    public static Mesh CreateMesh3DFromPolyCollider(PolygonCollider2D polygonCollider, float frontDistance = -10, float backDistance = 10)
    {
        frontDistance = Mathf.Min(frontDistance, 0);
        backDistance = Mathf.Max(backDistance, 0);

        Mesh m = new Mesh();

        List<int> trix = new List<int>();
        List<Vector3> vertix = new List<Vector3>();
        int offset = 0;

        for (int j = 0; j < polygonCollider.pathCount; j++)
        {
            // convert polygon to triangles
            Triangulator triangulator = new Triangulator(polygonCollider.GetPath(j));
            int[] tris = triangulator.Triangulate();

            Vector3[] vertices = new Vector3[polygonCollider.GetPath(j).Length * 2];

            for (int i = 0; i < polygonCollider.GetPath(j).Length; i++)
            {
                vertices[i].x = polygonCollider.GetPath(j)[i].x;
                vertices[i].y = polygonCollider.GetPath(j)[i].y;
                vertices[i].z = frontDistance; // front vertex
                vertices[i + polygonCollider.GetPath(j).Length].x = polygonCollider.GetPath(j)[i].x;
                vertices[i + polygonCollider.GetPath(j).Length].y = polygonCollider.GetPath(j)[i].y;
                vertices[i + polygonCollider.GetPath(j).Length].z = backDistance;  // back vertex    
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
                triangles[count_tris] = offset + i;
                triangles[count_tris + 1] = offset + n;
                triangles[count_tris + 2] = offset + i + polygonCollider.GetPath(j).Length;
                triangles[count_tris + 3] = offset + n;
                triangles[count_tris + 4] = offset + n + polygonCollider.GetPath(j).Length;
                triangles[count_tris + 5] = offset + i + polygonCollider.GetPath(j).Length;
                count_tris += 6;
            }

            vertix.AddRange(vertices);
            trix.AddRange(triangles);
            offset += polygonCollider.GetPath(j).Length * 2;
        }

        m.vertices = vertix.ToArray();
        m.triangles = trix.ToArray();
        m.RecalculateNormals();
        m.RecalculateBounds();

        return m;
    }
}
