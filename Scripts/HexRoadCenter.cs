using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexRoadCenter : Renderable
{
    private const int verticeAmount = 7;
    private const int trianglesAmount = 18;
    [SerializeField] HexRoad roadPrefab;
    private bool[] hasRoadDir=new bool[6];
    private const int scale = 10;
    public HexRoadCenter(): base(verticeAmount, trianglesAmount){}

    public void CreateRoad(int dir)
    {
        Vector3[] rvertices = new Vector3[4];
        switch (dir)
        {
            case 0:
                {
                    if (!hasRoadDir[0])
                    {
                        HexRoad road = Instantiate<HexRoad>(roadPrefab);
                        road.transform.SetParent(transform,false);
                        Vector3 offset = vertices[1]-vertices[3];
                        rvertices[0] = vertices[0];
                        rvertices[1] = vertices[1];
                        rvertices[2] = vertices[3]+offset* scale;
                        rvertices[3] = vertices[4]+offset* scale;
                        road.SetAwake(rvertices);
                        hasRoadDir[0] = true;
                    }

                }
                break;
            case 1:
                {
                    if (!hasRoadDir[1])
                    {
                        HexRoad road = Instantiate<HexRoad>(roadPrefab);
                        road.transform.SetParent(transform, false);
                        Vector3 offset = vertices[2] - vertices[4];
                        rvertices[0] = vertices[1];
                        rvertices[1] = vertices[2];
                        rvertices[2] = vertices[4] + offset * scale;
                        rvertices[3] = vertices[5] + offset * scale;
                        road.SetAwake(rvertices);
                        hasRoadDir[1] = true;
                    }

                }
                break;
            case 2:
                {
                    if (!hasRoadDir[2])
                    {
                        HexRoad road = Instantiate<HexRoad>(roadPrefab);
                        road.transform.SetParent(transform, false);
                        Vector3 offset = vertices[3] - vertices[5];
                        rvertices[0] = vertices[2];
                        rvertices[1] = vertices[3];
                        rvertices[2] = vertices[5] + offset * scale;
                        rvertices[3] = vertices[0] + offset * scale;
                        road.SetAwake(rvertices);
                        hasRoadDir[2] = true;
                    }

                }
                break;
            case 3:
                {
                    if (!hasRoadDir[3])
                    {
                        HexRoad road = Instantiate<HexRoad>(roadPrefab);
                        road.transform.SetParent(transform, false);
                        Vector3 offset = vertices[4] - vertices[0];
                        rvertices[0] = vertices[3];
                        rvertices[1] = vertices[4];
                        rvertices[2] = vertices[0] + offset * scale;
                        rvertices[3] = vertices[1] + offset * scale;
                        road.SetAwake(rvertices);
                        hasRoadDir[3] = true;
                    }

                }
                break;
            case 4:
                {
                    if (!hasRoadDir[4])
                    {
                        HexRoad road = Instantiate<HexRoad>(roadPrefab);
                        road.transform.SetParent(transform, false);
                        Vector3 offset = vertices[5] - vertices[1];
                        rvertices[0] = vertices[4];
                        rvertices[1] = vertices[5];
                        rvertices[2] = vertices[1] + offset * scale;
                        rvertices[3] = vertices[2] + offset * scale;
                        road.SetAwake(rvertices);
                        hasRoadDir[4] = true;
                    }

                }
                break;
            case 5:
                {
                    if (!hasRoadDir[5])
                    {
                        HexRoad road = Instantiate<HexRoad>(roadPrefab);
                        road.transform.SetParent(transform, false);
                        Vector3 offset = vertices[0] - vertices[2];
                        rvertices[0] = vertices[5];
                        rvertices[1] = vertices[0];
                        rvertices[2] = vertices[2] + offset * scale;
                        rvertices[3] = vertices[3] + offset * scale;
                        road.SetAwake(rvertices);
                        hasRoadDir[5] = true;
                    }

                }
                break;
        }
    }

    private void Awake()
    {
        GenerateVertices();
        GenerateTriangles();
        mainColor = Color.red;
        colors = new List<Color>();
        for (int i = 0; i < verticeAmount; i++)
        {
            colors.Add(mainColor);
        }
        transform.position=new Vector3(transform.position.x, transform.position.y,-1);
        Render("HexRoadCenter");
    }

    protected override void GenerateVertices()
    {
        for (int i = 0; i < HexMetrics.corners.Length; i++)
        {
            vertices[i] = HexMetrics.corners[i] * 0.1f;
        }
        vertices[HexMetrics.corners.Length] = HexMetrics.GetHexagonCenter();
    }
    protected override void GenerateTriangles()
    {
        for (int i = 0; i < HexMetrics.corners.Length; i++)
        {
            int index = i * 3;
            triangles[index] = i;
            triangles[index + 1] = (i + 1) % HexMetrics.corners.Length;
            triangles[index + 2] = HexMetrics.corners.Length;
        }
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
