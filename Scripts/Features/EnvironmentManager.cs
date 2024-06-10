using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] List<EnvironmentData> environmentPrefabs;
    HexMapEditor hexMapEditor;
    private HexCell cell;
    private float offsetX = 8.22f;
    private float offsetY = -0.6f;
    private List<Vector2> positions = new List<Vector2>();
    private List<Environment> environmentList = new List<Environment>();
    private List<int> availableIndexes = new List<int>();
    private List<int> resetIndexes = new List<int>();
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material outlineMaterial;

    private void Awake()
    {
        float envFactor = 0.75f;
        hexMapEditor=FindObjectOfType<HexMapEditor>();
        cell = GetComponentInParent<HexCell>();

        for (int i = 0; i < HexMetrics.corners.Length; i++)
        {
            int j = i + 1;
            if (i == 5)
            {
                j = 0;
            }
            positions.Add(GetCenter(HexMetrics.corners[i], HexMetrics.corners[j]) * envFactor);
            positions.Add(GetCenter(HexMetrics.corners[i], HexMetrics.corners[j], HexMetrics.GetHexagonCenter()) * envFactor);
            positions.Add(GetCenter(HexMetrics.corners[i], GetCenter(HexMetrics.corners[i], HexMetrics.corners[j]), GetCenter(HexMetrics.corners[i], HexMetrics.GetHexagonCenter())) * envFactor);
            positions.Add(GetCenter(HexMetrics.corners[j], GetCenter(HexMetrics.corners[i], HexMetrics.corners[j]), GetCenter(HexMetrics.corners[j], HexMetrics.GetHexagonCenter())) * envFactor);
            positions.Add(GetCenter(HexMetrics.GetHexagonCenter(), GetCenter(HexMetrics.corners[i], HexMetrics.GetHexagonCenter()), GetCenter(HexMetrics.corners[j], HexMetrics.GetHexagonCenter())) * envFactor);
            positions.Add(HexMetrics.corners[i] * envFactor);
            positions.Add(GetCenter(HexMetrics.corners[i], HexMetrics.GetHexagonCenter()) * envFactor);
        }
        positions.Add(HexMetrics.GetHexagonCenter() * envFactor);

        for(int i = 0; i<positions.Count; i++)
        {
            availableIndexes.Add(i);
            resetIndexes.Add(i);
        }
       


    }

 
    private void LoadEnvirontment()
    {
        if (cell.GetEnvLevel() <= positions.Count)
        {
            for (int i = 0; i < cell.GetEnvLevel(); i++)
            {

                int randomIndex = UnityEngine.Random.Range(0, availableIndexes.Count);
                int selectedIndex = availableIndexes[randomIndex];

                if (cell.GetMainColor() == hexMapEditor.GetColors()[0])
                {
                    EnvironmentData forestData = environmentPrefabs[0];
                    if (forestData != null)
                    {
                        int randomEnvIndex = UnityEngine.Random.Range(0, forestData.list.Count);
                        Forest forest = Instantiate<Forest>((Forest)forestData.list[randomEnvIndex]);
                        forest.transform.SetParent(transform, false);
                        forest.transform.position = new Vector3(cell.transform.position.x + offsetX + positions[selectedIndex].x, cell.transform.position.y + offsetY + positions[selectedIndex].y, -5f);
                        EnvironmentList.Add(forest);

                    }
                }
                else if (cell.GetMainColor() == hexMapEditor.GetColors()[1])
                {
                    EnvironmentData waterData = environmentPrefabs[1];
                    if (waterData != null)
                    {
                        int randomEnvIndex = UnityEngine.Random.Range(0, waterData.list.Count);
                        Water water = Instantiate<Water>((Water)waterData.list[randomEnvIndex]);
                        water.transform.SetParent(transform, false);
                        water.transform.position = new Vector3(cell.transform.position.x + offsetX + positions[selectedIndex].x, cell.transform.position.y + offsetY + positions[selectedIndex].y, -5f);
                        EnvironmentList.Add(water);

                    }
                }
                else if (cell.GetMainColor() == hexMapEditor.GetColors()[2])
                {
                    EnvironmentData mountainData = environmentPrefabs[2];
                    if (mountainData != null)
                    {
                        int randomEnvIndex = UnityEngine.Random.Range(0, mountainData.list.Count);
                        Mountain mountain = Instantiate<Mountain>((Mountain)mountainData.list[randomEnvIndex]);
                        mountain.transform.SetParent(transform, false);
                        mountain.transform.position = new Vector3(cell.transform.position.x + offsetX + positions[selectedIndex].x, cell.transform.position.y + offsetY + positions[selectedIndex].y, -5f);
                        EnvironmentList.Add(mountain);

                    }
                }
                else if(cell.GetMainColor() == hexMapEditor.GetColors()[3])
                {
                    EnvironmentData desertData = environmentPrefabs[3];
                    if (desertData != null)
                    {
                        int randomEnvIndex = UnityEngine.Random.Range(0, desertData.list.Count);
                        Beach desert = Instantiate<Beach>((Beach)desertData.list[randomEnvIndex]);
                        desert.transform.SetParent(transform, false);
                        desert.transform.position = new Vector3(cell.transform.position.x + offsetX + positions[selectedIndex].x, cell.transform.position.y + offsetY + positions[selectedIndex].y, -5f);
                        EnvironmentList.Add(desert);

                    }
                }
                

                availableIndexes.RemoveAt(randomIndex);

            }
        }
        else
            Debug.LogWarning("EnviromentLevel > positions.Count() !!!");



    }

    
    public void ResetEnvironment()
    {
        DestroyEnvironment();
        LoadEnvirontment ();
    }

    private void DestroyEnvironment()
    {
        for (int i = EnvironmentList.Count - 1; i >= 0; i--)
        {
            if (EnvironmentList[i].gameObject != null)
            {
                Destroy(EnvironmentList[i].gameObject);
            }
            EnvironmentList.RemoveAt(i);
        }
        availableIndexes = new List<int>(resetIndexes);
    }


    public Vector2 GetCenter(Vector3 a, Vector3 b, Vector3 c)
    {
        float centerX = (a.x + b.x + c.x) / 3f;
        float centerY = (a.y + b.y + c.y) / 3f;

        return new Vector2(centerX, centerY);
    }

    public Vector2 GetCenter(Vector3 a, Vector3 b)
    {
        float centerX = (a.x + b.x) / 2f;
        float centerY = (a.y + b.y) / 2f;

        return new Vector2(centerX, centerY);
    }
    public List<Environment> EnvironmentList { get => environmentList; set => environmentList = value; }


}
