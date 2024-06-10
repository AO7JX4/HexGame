using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Renderable : MonoBehaviour
{
    protected Color mainColor;
    protected Texture2D mainTex;
    protected List<Color> colors;
    protected Vector3[] vertices;
    protected int[] triangles;
    protected float[] texture = 
    {
        0.5f,1.0f,
        1.0f,0.75f,
        1.0f,0.25f,
        0.5f,0.0f,
        0.0f,0.25f,
        0.0f,0.75f,
        0.5f,0.5f
    };

    public Renderable(int verticeAmount, int triangleSize)
    {
        vertices = new Vector3[verticeAmount];
        triangles = new int[triangleSize]; 
    }
    protected abstract void Render(string name);

    protected abstract void GenerateTriangles();
    protected abstract void GenerateVertices();
    
}

