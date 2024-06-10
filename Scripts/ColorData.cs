using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Color")]
public class ColorData : ScriptableObject
{
    public List<Color> list = new List<Color>();
}
