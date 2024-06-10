using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UnitManager : MonoBehaviour
{

    [SerializeField] Unit unitPrefab;
    Dictionary<Player, Unit> list = new Dictionary<Player, Unit>();

    public Dictionary<Player, Unit> List { get => list; set => list = value; }

    public Unit SpawnUnit(HexCell cell, Player p)
    {
        Unit u;
        if (!cell.GetComponentInChildren<UnitManager>().List.ContainsKey(p))
        {
            u= Instantiate(unitPrefab);
            u.transform.SetParent(cell.transform, false);
            u.IncreasePower();
            u.Owner = p;
            cell.GetComponentInChildren<UnitManager>().List[p]=u;
        }
        else
        {
            u = cell.GetComponentInChildren<UnitManager>().List[p];
            u.IncreasePower();
        }

        return u;
    }
}
