using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexEdge : Renderable
{
    private static int verticeAmount = 16+2;
    private static int trianglesAmount = verticeAmount*3;
    private int dir=0;



    public Color GetMainColor()
    {
        return mainColor;
    }


    public HexEdge() : base(verticeAmount, trianglesAmount) { }

    public void SetAwake(int d)
    {
        dir = d;
        GenerateVertices();
        GenerateTriangles();
        mainColor = Color.red;
        colors = new List<Color>();
        for (int i = 0; i < verticeAmount; i++)
        {
            colors.Add(mainColor);
        }
        Render("HexEdge");

    }

    private Vector3 FindHalfVector(Vector3 vec1, Vector3 vec2)
    {
        return (vec1 + vec2) * 0.5f;
    }

    private void InitBreakPoints(int index, int phase)
    {
        float amplitude1 = Random.Range(-0.2f,0.2f);
        float amplitude2 = Random.Range(-0.2f, 0.2f);


        vertices[index - phase] = FindHalfVector(vertices[index], vertices[index-2*phase]);
        vertices[index + phase] = FindHalfVector(vertices[index], vertices[index+2*phase]);

        Vector3 offset = FindHalfVector(vertices[0] * 2f, vertices[verticeAmount-2] * 2f).normalized; //Maybe fix!!!!!

        vertices[index - phase] += offset*amplitude1;
        vertices[index + phase] += offset*amplitude2;


    }


    protected override void GenerateVertices()
    {
        //Init edgepoints
        vertices[0] = HexMetrics.corners[0] + ((HexMetrics.corners[0] -HexMetrics.corners[5]).normalized*0.1f);
        vertices[verticeAmount-2] = HexMetrics.corners[1]+ ((HexMetrics.corners[1] - HexMetrics.corners[2]).normalized * 0.1f);
        vertices[verticeAmount - 1] = FindHalfVector(vertices[0],vertices[verticeAmount-2]);

        //Init first breakpoint in middle*amplitude with bit offset sideways
        float amplitude = Random.Range(0.6f, 0.7f);
        float sideamplitude = Random.Range(-1.0f, 1.0f);
        vertices[(verticeAmount-1)/2] = FindHalfVector(vertices[0] * 2f, vertices[verticeAmount-2] * 2f)*amplitude;
        Vector3 offset=new Vector3(vertices[(verticeAmount - 1) / 2].y, -vertices[(verticeAmount - 1) / 2].x, 0f).normalized;
        vertices[(verticeAmount - 1) / 2] += offset*sideamplitude;


        InitBreakPoints((verticeAmount - 1) / 2,4); //Phase1

        InitBreakPoints(4,2); //Phase2
        InitBreakPoints(12,2);


        InitBreakPoints(2,1); //Phase3
        InitBreakPoints(6,1);
        InitBreakPoints(10,1);
        InitBreakPoints(14,1);



        //Rotate vertices around Z axis, to generate in different direction
        for (int i = 0; i < vertices.Length; i++) {
            vertices[i] = Quaternion.Euler(0, 0, -60 * dir) * vertices[i];
        }
       

    }
    protected override void GenerateTriangles()
    {

        for (int i = 0; i < vertices.Length-1; i++)
        {
            int index = i * 3;
            triangles[index] = i;
            triangles[index + 1] = i + 1;
            triangles[index + 2] = verticeAmount-1;
        }
    }

    protected override void Render(string name)
    {
        Mesh mesh = new Mesh();
        mesh.name = name;
        // Define UVs
        Vector2[] uv = new Vector2[vertices.Length];
        float maxX = float.NegativeInfinity;
        float maxY = float.NegativeInfinity;
        float minX = float.PositiveInfinity;
        float minY = float.PositiveInfinity;

       
        foreach (Vector3 point in vertices)
        {
            
            if (point.x > maxX)
            {
                maxX = point.x; 
            }

           
            if (point.y > maxY)
            {
                maxY = point.y; 
            }

            if (point.x < minX)
            {
                minX = point.x;
            }

            if (point.y < minY)
            {
                minY = point.y;
            }
        }
        for (int i = 0; i < uv.Length; i++)
        {
           if(dir==0 || dir == 1)
           {
                uv[i] = new Vector2(Mathf.Abs(vertices[i].x / maxX), Mathf.Abs(vertices[i].y / maxY));
           }
           else if(dir==3 || dir == 4)
           {
                uv[i] = new Vector2(Mathf.Abs(vertices[i].x / minX), Mathf.Abs(vertices[i].y / minY));
           }
           else if(dir ==2)
           {
                uv[i] = new Vector2(Mathf.Abs(vertices[i].x / maxX), Mathf.Abs(vertices[i].y / minY));
           }
           else
           {
                uv[i] = new Vector2(Mathf.Abs(vertices[i].x / minX), Mathf.Abs(vertices[i].y / maxY));
           }
            
          
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
