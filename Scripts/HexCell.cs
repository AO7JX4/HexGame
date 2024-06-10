using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexCell : Renderable
{
    private bool hasRoadCenter = false;
    private const int verticeAmount=7;
    private const int  trianglesAmount = 18;
    private HexCoordinates coordinates;
    private int envLevel = 0;
    [SerializeField] bool isSelected=false;
    [SerializeField] bool isVisible=false;
    private int row;
    private bool[] hasEdge=new bool[6];
    private Building[] hasWall = new Building[6];
    private Building[] hasTower = new Building[6];
    private double elevation = 0;
    private GameObject castle=null;
    



    [SerializeField] HexRoadCenter roadCenterPrefab;
    [SerializeField] HexEdge edgePrefab;
    [SerializeField] List<HexCell> neighbours;

    public GameObject Castle { get => castle; set => castle = value; }
    public Building[] HasWall { get => hasWall; set => hasWall = value; }
    public Building[] HasTower { get => hasTower; set => hasTower = value; }

    public double GetElevation()
    { return elevation; }
    public void Elevate()
    { elevation++; }

    public void Delevate()
    { elevation--; }
    public bool GetIsVisible() {  return isVisible; }
    public void SetIsVisible(bool v)
    {
        isVisible = v;
        int intValue = v ? 1 : 0; 
        GetComponent<MeshRenderer>().material.SetInt("_IsVisible", intValue);
        SetEdgeMaterial(intValue);
        if(castle != null)
            castle.gameObject.SetActive(v);

       
    }

    public void SetHasEdge(int i, bool b)
    {
        hasEdge[i]=b; 
    }
    public int GetRow()
    {
        return row;
    }
    public void SetRow(int r)
    { row = r; }    
    public bool GetIsSelected()
    {
        return isSelected;
    }
    public Color GetMainColor()
    {
        return mainColor;
    }
    public List<HexCell> GetNeighbours()
    {
        return neighbours;
    }
    public void AddNeighbour(HexCell cell)
    {
        neighbours.Add(cell);
    }
    public EnvironmentManager GetManager()
    {
        return GetComponentInChildren<EnvironmentManager>();
    }
    public int GetEnvLevel()
    {
        return envLevel;
    }
    public void SetEnvLevel(int lvl)
    {
        envLevel = lvl;
    }
    public bool GetRoadCenter()
    {
        return hasRoadCenter;
    }
    public void SetCoordinates(HexCoordinates cord)
    {
        coordinates = cord;
    }
    public HexCoordinates GetCoordinates()
    {
        return coordinates;
    }

    public void SetEnvironmentVisible(bool b)
    {
        List<Environment> l = GetManager().EnvironmentList;
        foreach (Environment e in l)
        {
            e.gameObject.SetActive(b);
        }
    }


    public HexCell():base(verticeAmount, trianglesAmount){}



    private void Awake()
    {
       
        for(int i = 0; i < 6; i++)
        {
            hasEdge[i] = false;
        }
        GenerateVertices();
        GenerateTriangles();
        mainColor = Color.white;
        colors = new List<Color>();
        for (int i = 0; i < verticeAmount; i++)
        {
            colors.Add(mainColor);
        }
        Render("HexCell");
    }

    public void GenerateEdge(int dir)
    {
        HexEdge edge = Instantiate<HexEdge>(edgePrefab);
        edge.transform.SetParent(transform, false);
        edge.SetAwake(dir);
    }

    public void CreateRoadCenter()
    {
       
        if(!hasRoadCenter) 
        {
            HexRoadCenter roadCenter = Instantiate<HexRoadCenter>(roadCenterPrefab);
            roadCenter.transform.SetParent(transform, false);
        }
        hasRoadCenter = true;

    }

    public void DestroyRoadCenter()
    {
        if (hasRoadCenter)
        {
            Destroy(GetComponentInChildren<HexRoadCenter>().gameObject);
        }
        hasRoadCenter = false;
    }
    public HexCell FindNeighbour(int dir)
    {
        for (int i = 0; i < neighbours.Count; i++)
        {
            if (coordinates.Q + HexDirection.directionVectors[dir].Q == neighbours[i].coordinates.Q && coordinates.R + HexDirection.directionVectors[dir].R == neighbours[i].coordinates.R && coordinates.S + HexDirection.directionVectors[dir].S == neighbours[i].coordinates.S)
            {
                return neighbours[i];
            }
        }
        return null;
    }


    public int FindDirectionOfNeighbor(HexCell neighbourCell)
    {
        for (int dir = 0; dir < HexDirection.directionVectors.Length; dir++)
        {
            if (FindNeighbour(dir) == neighbourCell)
            {
                return dir;
            }
        }
        return -1;
    }

    public void SetOutlineColor(string dir, Color c)
    {
        GetComponent<MeshRenderer>().material.SetColor("_ColorDir" + dir, c);
    }

    public void SetEdgeActive(bool b)
    {
        HexEdge[] edges = GetComponentsInChildren<HexEdge>(true);
        foreach (HexEdge edge in edges)
        {
           edge.gameObject.SetActive(b);
        }
    }


    public void SetEdgeMaterial(int value)
    {
        HexEdge[] edges = GetComponentsInChildren<HexEdge>(true);
        foreach (HexEdge edge in edges)
        {
            edge.GetComponent<MeshRenderer>().material.SetColor("_Color", mainColor);
            edge.GetComponent<MeshRenderer>().material.SetTexture("_Texture2D", mainTex);
            edge.GetComponent<MeshRenderer>().material.SetInt("_IsVisible", value);
            if (StaticGameOptions.flowFog)
                edge.GetComponent<MeshRenderer>().material.SetVector("_FogVelocity", new Vector2(-0.1f, 0));
            else
                edge.GetComponent<MeshRenderer>().material.SetVector("_FogVelocity", new Vector2(0, 0));
        }
    }

    public void SetWaterFlow()
    {
        if(StaticGameOptions.flowWater && elevation==0)
            GetComponent<MeshRenderer>().material.SetVector("_WaterVelocity", new Vector2(0.15f,0));
        else
            GetComponent<MeshRenderer>().material.SetVector("_WaterVelocity", new Vector2(0, 0));
    }

    public void SetFogFlow()
    {
        if (StaticGameOptions.flowFog)
            GetComponent<MeshRenderer>().material.SetVector("_FogVelocity", new Vector2(0.1f, 0));
        else
            GetComponent<MeshRenderer>().material.SetVector("_FogVelocity", new Vector2(0, 0));

        HexEdge[] edges = GetComponentsInChildren<HexEdge>(true);
        foreach (HexEdge edge in edges)
        {

            if (StaticGameOptions.flowFog)
                edge.GetComponent<MeshRenderer>().material.SetVector("_FogVelocity", new Vector2(-0.1f, 0));
            else
                edge.GetComponent<MeshRenderer>().material.SetVector("_FogVelocity", new Vector2(0, 0));
        }
    }

    public void SetMaterial(Color c,Texture2D text, Vector2 wvelocity, Vector2 fvelocity)
    {
        mainColor = c;
        mainTex = text;
        GetComponent<MeshRenderer>().material.SetTexture("_Texture2D", text);
        GetComponent<MeshRenderer>().material.SetVector("_WaterVelocity", wvelocity);
        GetComponent<MeshRenderer>().material.SetVector("_FogVelocity", fvelocity);
        for(int i = 0; i<6; i++)
        {
            SetOutlineColor(i.ToString(), mainColor);
        }
    }

    public void Select()
    {
        isSelected = true;
        for (int i = 0; i < 6; i++)
        {
            SetOutlineColor(i.ToString(), Color.white);
        }
    }


    public void UnSelect()
    {
        isSelected = false;
        for (int i = 0;i < 6; i++)
        {
            if (hasEdge[i])
            {
                SetOutlineColor(i.ToString(), FindNeighbour(i).GetMainColor());
            }
            else
            {
                SetOutlineColor(i.ToString(), mainColor);
            }
        }
    }

    protected override void Render(string name)
    {

        Mesh mesh = new Mesh();
        mesh.name = name;
        // Define UVs
        Vector2[] uv = new Vector2[texture.Length / 2];
        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new Vector2(texture[i * 2], texture[i * 2 + 1]);
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

    protected override void GenerateVertices()
    {
        for (int i = 0; i < HexMetrics.corners.Length; i++)
        {
            vertices[i] = HexMetrics.corners[i];
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

}
