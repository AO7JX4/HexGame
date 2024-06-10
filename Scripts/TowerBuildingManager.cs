using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TowerBuildingManager : MonoBehaviour
{
    [SerializeField] List<BuildingData> buildingPrefabs;
    private GameObject towers;
    GameManager gameManager;
    private HexCell cell;
    private float offsetX = 8.39f;
    private float offsetY = -1.0f;
    private List<Vector2> positions = new List<Vector2>();
    private List<int> availableIndexes = new List<int>();
    private List<Building> preBuildings = new List<Building>();
    private List<Building> builtTowers = new List<Building>();
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
        towers= GameObject.FindGameObjectWithTag("Towers");
        cell = GetComponentInParent<HexCell>();
        for (int i = 0; i < HexMetrics.corners.Length; i++)
        {
            positions.Add(HexMetrics.corners[i]);
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
        neigbours=cell.GetNeighbours();
        
    }
    Vector2 closestPos=new Vector3();
    Vector3 newPos=new Vector3();
    Vector3 prePos = new Vector3();

    public void SetPrePos(Vector3 pos)
    {
        prePos = pos;
    }
    int index = -1;
    void  Update()
    {
        if (cell.GetIsSelected() && gameManager.Toggles[1].isOn && cell.GetIsVisible() && gameManager.CurrentPlayer.ActionCount > 0) //If panel is selected
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
                           BuildPreTower(i);
                           index = i;
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

           if (index != -1 && Input.GetKeyDown(KeyCode.Space) && availableIndexes.Contains(index) && gameManager.CurrentPlayer.Wood>=10 && gameManager.CurrentPlayer.Mineral>=5)
           {
               builtTowers.Add(preBuildings[index]);
               if (preBuildings[index] != null)
               {
                   Destroy(preBuildings[index].gameObject);
               }
               if(index==3)
               {
                   BuildTower(0);
               }
               else if(index==4 || index==2)
               {
                   BuildTower(2);
               }
               else if (index == 5 || index == 1)
               {
                   BuildTower(4);
               }
               else if (index == 0)
               {
                   BuildTower(6);
               }

               closestPos = FindClosestBuildablePlace(mousePosition, truePos);
               newPos = new Vector3(closestPos.x, closestPos.y, -5f);
           }

           for (int i = 0; i < neigbours.Count; i++) //This is incase we prebuilding is on same position when selected a neighbour
           {
               neigbours[i].GetComponent<TowerBuildingManager>().SetPrePos(new Vector3(-100f, -100f, -100f));
           }
        }
        else //If selected another panel
        {
            SetPrePos(new Vector3(-100f, -100f, -100f));
            for (int i=0; i<positions.Count;i++)
            {
                if (preBuildings[i] != null)
                {
                    Destroy(preBuildings[i].gameObject);
                }
            }
        }
    }

    private void BuildPreTower(int i)
    {
        int value = 0;
        if (i == 3)
        {
            value = 0;
        }
        else if (i == 4 || i == 2)
        {
            value =2;
        }
        else if (i == 5 || i == 1)
        {
            value = 4;
        }
        else if (i == 0)
        {
            value = 6;
        }

        preBuildings[i] = Instantiate<Building>(buildingPrefabs[0].list[0]);
        preBuildings[i].transform.SetParent(towers.transform, false);
        preBuildings[i].transform.position = newPos;
        SpriteRenderer renderer = preBuildings[i].GetComponentInChildren<SpriteRenderer>();
        renderer.material = buildingMaterial;
        if (gameManager.CurrentPlayer.Wood < 10 || gameManager.CurrentPlayer.Mineral < 5)
            renderer.material.SetColor("_Color", Color.red);
        renderer.sortingOrder = maxOrder - value - cell.GetRow() * 4;

    }

    private void BuildTower(int value)
    {
        builtTowers[builtTowers.Count - 1] = Instantiate<Building>(buildingPrefabs[0].list[0]);
        builtTowers[builtTowers.Count - 1].transform.SetParent(towers.transform, false);
        builtTowers[builtTowers.Count - 1].transform.position = newPos;
        gameManager.CurrentPlayer.BuildBuilding(builtTowers[builtTowers.Count - 1], 10, 5);
        SpriteRenderer renderer = builtTowers[builtTowers.Count - 1].GetComponentInChildren<SpriteRenderer>();
        renderer.sortingOrder = maxOrder - value - cell.GetRow() * 4;
        truePos[index] = new Vector2(-100f, -100f);
        availableIndexes.Remove(index);
        cell.HasTower[index] = builtTowers[builtTowers.Count - 1];
        RemoveAvailableBuildablePlaceFromNeighbour(index);
        if (gameManager.CurrentPlayer.ActionCount == 0)
        {
            gameManager.HideButtons();
        }
    }


    private void RemoveAvailableBuildablePlaceFromNeighbour(int index)
    {
        int nIndex;
        if (index != 0)
            nIndex = index - 1;
        else
            nIndex = 5;


        int bIndex;
        if (index > 1)
            bIndex = index - 2;
        else
            bIndex = index + 4;

        int aIndex;
        if (index < 4)
            aIndex = index + 2;
        else
            aIndex = index - 4;

        if (cell.FindNeighbour(nIndex) != null)
        {
            cell.FindNeighbour(nIndex).GetComponent<TowerBuildingManager>().GetAvailableIndexes().Remove(aIndex);
            cell.FindNeighbour(nIndex).GetComponent<TowerBuildingManager>().GetTruePos()[aIndex] = new Vector2(-100f, -100f);
            cell.FindNeighbour(nIndex).HasTower[aIndex]= builtTowers[builtTowers.Count - 1];
        }
        if (cell.FindNeighbour(index) != null)
        {
            cell.FindNeighbour(index).GetComponent<TowerBuildingManager>().GetAvailableIndexes().Remove(bIndex);
            cell.FindNeighbour(index).GetComponent<TowerBuildingManager>().GetTruePos()[bIndex] = new Vector2(-100f, -100f);
            cell.FindNeighbour(index).HasTower[bIndex] = builtTowers[builtTowers.Count - 1];
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
