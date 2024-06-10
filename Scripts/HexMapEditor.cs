using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{
    [SerializeField] ColorData colorData;
    private Color[] colors;
    [SerializeField] HexGrid hexGrid;


  
    
    public int GetAction2()
    {
        return 0; //TODO MANAGERS
    }
    public int GetActivePanel()
    {
        return 0; //TODO MANAGERS
    }




    private void Awake()
    {
        colors = new Color[colorData.list.Count];
        for (int i = 0; i<colorData.list.Count; i++)
        {
            colors[i] = colorData.list[i];
        }
        
    }

    public Color[] GetColors()
    {
        return colors;
    }

   //public void DoAction()
   //{
   //    hexGrid.DoAction(action);
   //}



    void Update()
    {

       //if (Input.GetKeyDown(KeyCode.Space)) 
       //{
       //    DoAction();
       //}
    }






}
