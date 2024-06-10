using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class HexChunk : MonoBehaviour
{
   [SerializeField] HexCell[] cells;

    private void Awake()
    {
        cells = new HexCell[HexMetrics.chunkSizeY * HexMetrics.chunkSizeX];
    }

  

    private void Start()
    {
        cells = GetComponentsInChildren<HexCell>();
    }
}