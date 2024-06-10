using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class WallBuildingManager : MonoBehaviour
{
    [SerializeField] List<BuildingData> buildingPrefabs;
    private GameObject walls;
    GameManager gameManager;
    private HexCell cell;
    private float offsetX = 8.39f+0.008f;
    private float offsetY = -1.0f-0.25f;
    private List<Vector2> positions = new List<Vector2>();
    private List<int> availableIndexes = new List<int>();
    private List<Building> preBuildings = new List<Building>();
    private List<Building> builtWalls = new List<Building>();
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material outlineMaterial;
    [SerializeField] Material buildingMaterial;
    List<HexCell> neigbours = new List<HexCell>();
    List<Vector2> truePos = new List<Vector2>();
    private const int maxOrder = 32767;

    public List<Vector2> GetTruePos()
    {
        return truePos;
    }
    public List<int> GetAvailableIndexes()
    {
        return availableIndexes;
    }
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        walls = GameObject.FindGameObjectWithTag("Walls");
        cell = GetComponentInParent<HexCell>();
        for (int i = 0; i < HexMetrics.corners.Length; i++)
        {
            int j;
            if (i < 5)
                j = i+1;
            else
                j = 0;

            positions.Add(GetCenter(HexMetrics.corners[i], HexMetrics.corners[j]));
            Building newBuilding = Instantiate<Building>(buildingPrefabs[0].list[0]);
            Destroy(newBuilding.gameObject);
            preBuildings.Add(newBuilding);

        }
        for (int i = 0; i < positions.Count; i++)
        {
            availableIndexes.Add(i);
        }

    }

    private void Start()
    {
        for (int i = 0; i < positions.Count; i++)
        {
            truePos.Add(new Vector2(positions[i].x + cell.transform.position.x + offsetX, positions[i].y + cell.transform.position.y + offsetY));
        }
        neigbours = cell.GetNeighbours();

    }
    Vector2 closestPos = new Vector3();
    Vector3 newPos = new Vector3();
    Vector3 prePos = new Vector3();

    public void SetPrePos(Vector3 pos)
    {
        prePos = pos;
    }
    int index = -1;
    void Update()
    {
  
        if (cell.GetIsSelected() && gameManager.Toggles[2].isOn && cell.GetIsVisible() && gameManager.CurrentPlayer.ActionCount>0) //If cell and action is selected
        {

           
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition += new Vector2(offsetX, offsetY);
            closestPos = FindClosestBuildablePlace(mousePosition, truePos);
            newPos = new Vector3(closestPos.x, closestPos.y, -5f);


            if (prePos != newPos) //If same position
            {
                for (int i = 0; i < positions.Count; i++)
                {

                    if (truePos[i] == closestPos) //If closest place
                    {
                        if (availableIndexes.Contains(i)) //If closest place is buildable
                        {
                            
                            
                            switch(i) //Check if there are built TWO towers in direction, to place wall between
                            {
                                case 0:
                                    {
                                        if (!cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(0) && !cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(1))
                                        {
                                            BuildPreWall(i, 0,4);
                                        }
                                        else
                                            index = -1;
                                    }
                                    break;
                                case 1:
                                    {
                                        if (!cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(1) && !cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(2))
                                        {
                                            BuildPreWall(i, 1,2);
                                        }
                                        else
                                            index = -1;
                                    }
                                    break;
                                case 2:
                                    {
                                        if (!cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(2) && !cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(3))
                                        {
                                            BuildPreWall(i, 2,0);
                                        }
                                        else
                                            index = -1;
                                    }
                                    break;
                                case 3:
                                    {
                                        if (!cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(3) && !cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(4))
                                        {
                                            BuildPreWall(i, 0,0);
                                        }
                                        else
                                            index = -1;
                                    }
                                    break;
                                case 4:
                                    {
                                        if (!cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(4) && !cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(5))
                                        {
                                           BuildPreWall(i, 1,2);
                                        }
                                        else
                                            index = -1;
                                    }
                                    break;
                                case 5:
                                    {
                                        if (!cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(0) && !cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(5))
                                        {
                                            BuildPreWall(i, 2,4);
                                        }
                                        else
                                            index = -1;
                                    }
                                    break;
                            }
                            
                            
                            prePos = newPos;
                        }
                    }
                    else //If closest place moved
                    {

                        if (preBuildings[i] != null)
                        {
                            Destroy(preBuildings[i].gameObject);
                        }

                    }
                }



            }

            if (index != -1 && Input.GetKeyDown(KeyCode.Space) && availableIndexes.Contains(index) && gameManager.CurrentPlayer.Wood >= 20 && gameManager.CurrentPlayer.Mineral >= 10) //Check if user wanna build
            {
                builtWalls.Add(preBuildings[index]);
                if (preBuildings[index] != null)
                {
                    Destroy(preBuildings[index].gameObject);
                }
                switch (index) //Check if there are built TWO towers in direction, to place wall between
                {
                    case 0:
                        {
                            if (!cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(0) && !cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(1))
                            {
                                BuildWall(0,4);
                            }
                        }
                        break;
                    case 1:
                        {
                            if (!cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(1) && !cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(2))
                            {
                                BuildWall(1,2);
                            }
                        }
                        break;
                    case 2:
                        {
                            if (!cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(2) && !cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(3))
                            {
                                BuildWall(2,0);
                            }
                        }
                        break;
                    case 3:
                        {
                            if (!cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(3) && !cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(4))
                            {
                                BuildWall(0,0);
                            }
                        }
                        break;
                    case 4:
                        {
                            if (!cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(4) && !cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(5))
                            {
                                BuildWall(1,2);
                            }
                        }
                        break;
                    case 5:
                        {
                            if (!cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(0) && !cell.GetComponent<TowerBuildingManager>().GetAvailableIndexes().Contains(5))
                            {
                                BuildWall(2,4);
                            }
                        }
                        break;
                }
                closestPos = FindClosestBuildablePlace(mousePosition, truePos);
                newPos = new Vector3(closestPos.x, closestPos.y, -5f);
            }

            for (int i = 0; i < neigbours.Count; i++) //This is incase we prebuilding is on same position when selected a neighbour
            {
                neigbours[i].GetComponent<WallBuildingManager>().SetPrePos(new Vector3(-100f, -100f, -100f));
            }

        }
        else //If selected another cell
        {
            SetPrePos(new Vector3(-100f, -100f, -100f));
            for (int i = 0; i < positions.Count; i++)
            {
                if (preBuildings[i] != null)
                {
                    Destroy(preBuildings[i].gameObject);
                }
            }
        }
            
        
    }

    private void BuildPreWall(int i, int type, int value)
    {
        preBuildings[i] = Instantiate<Building>(buildingPrefabs[0].list[type]);
        preBuildings[i].transform.SetParent(walls.transform, false);
        preBuildings[i].transform.position = newPos;
        SpriteRenderer renderer = preBuildings[i].GetComponentInChildren<SpriteRenderer>();
        renderer.material = buildingMaterial;
        if(gameManager.CurrentPlayer.Wood<20 || gameManager.CurrentPlayer.Mineral<10)
            renderer.material.SetColor("_Color", Color.red);
        renderer.sortingOrder = maxOrder - 1 - value - cell.GetRow() * 4;

        index = i;
    }

    private void BuildWall(int type, int value)
    {
        builtWalls[builtWalls.Count - 1] = Instantiate<Building>(buildingPrefabs[0].list[type]);
        builtWalls[builtWalls.Count - 1].transform.SetParent(walls.transform, false);
        builtWalls[builtWalls.Count - 1].transform.position = newPos;
        gameManager.CurrentPlayer.BuildBuilding(builtWalls[builtWalls.Count - 1], 20, 10);
        SpriteRenderer renderer = builtWalls[builtWalls.Count - 1].GetComponentInChildren<SpriteRenderer>();
        renderer.sortingOrder= maxOrder-1 - value - cell.GetRow() * 4;
        truePos[index] = new Vector2(-100f, -100f);
        availableIndexes.Remove(index);
        cell.HasWall[index] = builtWalls[builtWalls.Count - 1];
        RemoveAvailableBuildablePlaceFromNeighbour(index);
        if(gameManager.CurrentPlayer.ActionCount==0)
        {
            gameManager.HideButtons();
        }
    }

    public Vector2 GetCenter(Vector3 a, Vector3 b)
    {
        float centerX = (a.x + b.x) / 2f;
        float centerY = (a.y + b.y) / 2f;

        return new Vector2(centerX, centerY);
    }


    private void RemoveAvailableBuildablePlaceFromNeighbour(int index)
    {
        if(cell.FindNeighbour(index) != null)
        {
            switch(index)
            {
                case 0:
                    {
                        cell.FindNeighbour(index).GetComponent<WallBuildingManager>().GetAvailableIndexes().Remove(3);
                        cell.FindNeighbour(index).GetComponent<WallBuildingManager>().GetTruePos()[3] = new Vector2(-100f, -100f);
                        cell.FindNeighbour(index).HasWall[3] = builtWalls[builtWalls.Count - 1];
                    }
                    break;
                case 1:
                    {
                        cell.FindNeighbour(index).GetComponent<WallBuildingManager>().GetAvailableIndexes().Remove(4);
                        cell.FindNeighbour(index).GetComponent<WallBuildingManager>().GetTruePos()[4] = new Vector2(-100f, -100f);
                        cell.FindNeighbour(index).HasWall[4] = builtWalls[builtWalls.Count - 1];
                    }
                    break;
                case 2:
                    {
                        cell.FindNeighbour(index).GetComponent<WallBuildingManager>().GetAvailableIndexes().Remove(5);
                        cell.FindNeighbour(index).GetComponent<WallBuildingManager>().GetTruePos()[5] = new Vector2(-100f, -100f);
                        cell.FindNeighbour(index).HasWall[5] = builtWalls[builtWalls.Count - 1];
                    }
                    break;
                case 3:
                    {
                        cell.FindNeighbour(index).GetComponent<WallBuildingManager>().GetAvailableIndexes().Remove(0);
                        cell.FindNeighbour(index).GetComponent<WallBuildingManager>().GetTruePos()[0] = new Vector2(-100f, -100f);
                        cell.FindNeighbour(index).HasWall[0] = builtWalls[builtWalls.Count - 1];
                    }
                    break;
                case 4:
                    {
                        cell.FindNeighbour(index).GetComponent<WallBuildingManager>().GetAvailableIndexes().Remove(1);
                        cell.FindNeighbour(index).GetComponent<WallBuildingManager>().GetTruePos()[1] = new Vector2(-100f, -100f);
                        cell.FindNeighbour(index).HasWall[1] = builtWalls[builtWalls.Count - 1];
                    }
                    break;
                case 5:
                    {
                        cell.FindNeighbour(index).GetComponent<WallBuildingManager>().GetAvailableIndexes().Remove(2);
                        cell.FindNeighbour(index).GetComponent<WallBuildingManager>().GetTruePos()[2] = new Vector2(-100f, -100f);
                        cell.FindNeighbour(index).HasWall[2] = builtWalls[builtWalls.Count - 1];
                    }
                    break;
            }
        }

    }


    private Vector2 FindClosestBuildablePlace(Vector2 mouse, List<Vector2> list)
    {
        float minDistance = float.MaxValue;
        Vector2 closestPos = Vector2.zero;

        foreach (Vector2 pos in list)
        {
            float distance = Vector2.Distance(mouse, pos);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPos = pos;
            }
        }
        return closestPos;
    }
}
