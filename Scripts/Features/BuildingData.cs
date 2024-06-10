using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Building")]
public class BuildingData : ScriptableObject
{
    public List<Building> list = new List<Building>();
}
