using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;


public class Unit : MonoBehaviour
{
    private float moveSpeed =10f;
    public int pathIndex =0;
    private List<HexCell> shortestPath=new List<HexCell>();
    private List<HexCell> visibleCells=new List<HexCell>();
    private bool isUpdated=false;
    private int power = 0;
    private TextMeshPro powerText;
    private Color c;
    private Player owner;
    private ColorData colorData;
    private List<Player> playerList;
    private GameManager gameManager;
    private HexGrid hexGrid;

    private void Awake()
    {
        PowerText = GetComponentInChildren<TextMeshPro>();
        gameManager=FindObjectOfType<GameManager>();
        hexGrid=FindObjectOfType<HexGrid>();
    }

    private void Start()
    {
        PowerText.color = new Color(C.r, C.g, C.b, 1);
        colorData = hexGrid.ColorData;
        playerList=gameManager.PlayerList;
    }
    public List<HexCell> GetPath()
    { return shortestPath; }

    public void SetPath(List<HexCell> path)
    { shortestPath = path; }
    private bool isMoving=false;

    public List<HexCell> VisibleCells { get => visibleCells; set => visibleCells = value; }
    public bool IsUpdated { get => isUpdated; set => isUpdated = value; }
    public int Power { get => power; set => power = value; }
    public TextMeshPro PowerText { get => powerText; set => powerText = value; }
    public Color C { get => c; set => c = value; }
    public Player Owner { get => owner; set => owner = value; }
    public List<Player> PlayerList { get => playerList; set => playerList = value; }
    public ColorData ColorData { get => colorData; set => colorData = value; }

    public bool GetIsMoving() {  return isMoving; }
    public void SetIsMoving(bool m)
    { isMoving = m; }

   public void IncreasePower()
   {
        power++;
        powerText.text=power.ToString();
     
   }

    private void Fight(Unit u)
    {
        int originalPower = power;
        power -= u.power; 
        u.power -= originalPower; 
        powerText.text = power.ToString();
        u.powerText.text = u.power.ToString();
    }

    private void Merge(Unit u)
    {
        u.power += power;
        power = 0;
        HandleUnitDestruction(this);
        u.powerText.text = u.power.ToString();
    }

    public void Move(List<HexCell> path)
    {
        pathIndex = 0;
        shortestPath=path;
        isMoving = true;
        StartCoroutine(MoveCoroutine());
    

     
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isMoving) return;

        Unit u = collision.gameObject.GetComponent<Unit>();
        if (u != null) //Fight
        {

            if(u.owner != this.owner)
            {
                Fight(u);
                HandleUnitDestruction(this);
                HandleUnitDestruction(u);
            }
            else //Merge
            {
                Merge(u);
            }

        }
   
    }

    private void HandleUnitDestruction(Unit unit)
    {
     
       if(unit.power<=0 && unit != null)
       {
            
            if (unit.GetComponentInParent<HexCell>().GetComponentInChildren<UnitManager>().List.ContainsKey(unit.owner))
            {
                unit.GetComponentInParent<HexCell>().GetComponentInChildren<UnitManager>().List.Remove(unit.owner);
                unit.owner.UnitList.Remove(unit);
            }
            Destroy(unit.gameObject);
       }

    }

    private IEnumerator MoveCoroutine()
    {
      
        while (Vector3.Distance(transform.position, shortestPath[shortestPath.Count-1].transform.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, shortestPath[pathIndex].transform.position, moveSpeed/50f);
            if(Vector3.Distance(transform.position, shortestPath[pathIndex].transform.position)<0.01f) //If reached next cell, change nextcell
            {
                for (int i = 0; i < shortestPath.Count - 1; i++)
                {
                    int index = shortestPath[i].FindDirectionOfNeighbor(shortestPath[i + 1]); //If there is a wall in the path, find a new path
                    if (shortestPath[i].HasWall[index] != null && shortestPath[i].HasWall[index].gameObject.activeSelf && !owner.buildingList.Contains(shortestPath[i].HasWall[index]))
                    {
                        float cost = 0;
                        List<HexCell> prevPath = shortestPath;
                        shortestPath = PathFinding.ReconstructPath(shortestPath[pathIndex], shortestPath.Last(), PathFinding.AStarSearch(shortestPath[pathIndex], shortestPath.Last(), ColorData.list, PlayerList, owner));
                        for (int j = 0; j < shortestPath.Count - 1; j++)
                        {
                            cost += PathFinding.Cost(shortestPath[j], shortestPath[j + 1], colorData.list, PlayerList, owner);
                        }

                        if(cost<=200) //TODO FIX if long wall stop going in circles!!!!!
                            Move(shortestPath);
                        else
                        {
                            prevPath.Last().GetComponentInChildren<UnitManager>().List.Remove(owner);
                            prevPath[pathIndex].GetComponentInChildren<UnitManager>().List[owner] = this;
                            isMoving=false;
                        }
                        yield break;
                    }
                }
                pathIndex++;

               
            }
            yield return null;
          
        }
       
      
    }
}


