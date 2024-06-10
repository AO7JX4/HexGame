using System.Collections.Generic;
using UnityEngine;

public static class PathFinding
{
    public static Dictionary<HexCell, HexCell> AStarSearch(HexCell startCell, HexCell goalCell, List<Color> colors, List<Player> players, Player currentPlayer)
    {
        PriorityQueue<HexCell, float> frontier = new PriorityQueue<HexCell, float>();
        Dictionary<HexCell, HexCell> cameFrom = new Dictionary<HexCell, HexCell>();
        Dictionary<HexCell, float> costSoFar = new Dictionary<HexCell, float>();

        frontier.Enqueue(startCell, 0);
        cameFrom[startCell] = null;
        costSoFar[startCell] = 0;

        while (frontier.Count > 0)
        {
            HexCell currentCell = frontier.Dequeue();

            if (currentCell == goalCell)
            {
                break;
            }

   

            foreach (HexCell nextCell in currentCell.GetNeighbours())
            {
                float newCost = costSoFar[currentCell] + Cost(currentCell, nextCell, colors, players, currentPlayer);
                if (!costSoFar.ContainsKey(nextCell) || newCost < costSoFar[nextCell])
                {
                    costSoFar[nextCell] = newCost;
                    float priority = newCost + Heuristic(goalCell, nextCell); 
                    frontier.Enqueue(nextCell, priority);
                    cameFrom[nextCell] = currentCell;
                }
            }
        }

        return cameFrom;
    }

    public static List<HexCell> ReconstructPath(HexCell startCell, HexCell goalCell, Dictionary<HexCell, HexCell> cameFrom)
    {
        List<HexCell> path = new List<HexCell>();
        HexCell currentCell = goalCell;

        while (currentCell != startCell)
        {
            path.Add(currentCell);
            currentCell = cameFrom[currentCell];
        }

        path.Add(startCell);
        path.Reverse();

        return path;
    }

    private static float Heuristic(HexCell goalCell, HexCell nextCell)
    {
        int distance = HexCoordinates.GetDistance(nextCell, goalCell);
        return distance;
    }

    public static float Cost(HexCell current, HexCell next, List<Color> colors, List<Player> players, Player currentPlayer)
    {
        float cost = 0;
        int index = current.FindDirectionOfNeighbor(next);        
        if (next.GetMainColor() == colors[0]) //Grass
            cost += 10;
        else if (next.GetMainColor() == colors[1]) //Water
            cost += 10000;
        else if (next.GetMainColor() == colors[2]) //Mountain
            cost += 30;
        else if (next.GetMainColor() == colors[3]) //Desert
            cost += 20;
        else
            cost += 1; //White

        
        if(players!=null)
        {
            foreach (Player p in players)
            {
                if (p != currentPlayer)
                {
                    if (current.HasWall[index] != null && p.buildingList.Contains(current.HasWall[index]) && current.HasWall[index].gameObject.activeSelf) //If there is wall between the next cell
                    {
                        cost += 200;
                    }
                }
            }
        }




        return cost;
    }
}

