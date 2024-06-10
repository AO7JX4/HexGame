using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Environment")]
public class EnvironmentData : ScriptableObject
{
    public List<Environment> list = new List<Environment>();
}


