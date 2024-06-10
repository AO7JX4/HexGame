using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexRoad : Renderable
{

    private const int verticeAmount = 4;
    private const int trianglesAmount = 6;
  
    public HexRoad(): base(verticeAmount,trianglesAmount){}
    public void SetAwake(Vector3[] v)
    {
        vertices = v;
        GenerateTriangles();
        mainColor = Color.red;
        colors = new List<Color>();
        for (int i = 0; i < verticeAmount; i++)
        {
            colors.Add(mainColor);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        Render("HexRoad");
    }

    protected override void GenerateVertices()
    {
        throw new System.NotImplementedException();
    }
    protected override void GenerateTriangles()
    {
        triangles[0] = 3;
        triangles[1] = 2;
        triangles[2] = 0;

        triangles[3] = 2;
        triangles[4] = 1;
        triangles[5] = 0;
    }

    protected override void Render(string name)
    {
        Mesh mesh = new Mesh();
        mesh.name = name;
        // Define UVs
        Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new Vector2(vertices[i].x, vertices[i].y);
        }

        // Assign vertices, UVs, colors, and triangles to the mesh
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.colors = colors.ToArray();
        mesh.triangles = triangles;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // Assign the mesh to the MeshCollider
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }
}
