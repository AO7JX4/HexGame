using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapCamera : MonoBehaviour
{
    

    [SerializeField]
    float zoomFactor = 1.0f;

    [SerializeField]
    float zoomSpeed = 5.0f;

    [SerializeField]
    float maxZoom = 3.0f;

    [SerializeField]
    float minZoom = 0.5f;

    [SerializeField]
    float edge = 0.975f;

    [SerializeField]
    float scrollSpeed = 0.05f;

    [SerializeField] GameObject menu;

    private float originalSize = 0f;

    private Camera thisCamera;

    
    void Start()
    {
        thisCamera = GetComponentInChildren<Camera>();
        originalSize = thisCamera.orthographicSize;
    }


    void Update()
    {
        float targetSize = originalSize * zoomFactor;
        if (targetSize != thisCamera.orthographicSize)
        {
            thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize,
            targetSize, Time.deltaTime * zoomSpeed);
        }

        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDelta != 0f)
        {
            
            if(zoomDelta > 0f)
            {
                if(zoomFactor>minZoom)
                zoomIn(zoomDelta);
            }
            else
            {
                if (zoomFactor < maxZoom)
                    zoomOut(zoomDelta);
            }
        }
        
        if(!StaticGameOptions.isMenuOpen)
        {
            //Up
            if (Input.mousePosition.y >= Screen.height * edge)
            {
                transform.Translate(Vector3.up * scrollSpeed, Space.World);
            }

            //Down
            if (Input.mousePosition.y <= Screen.height * (1 - edge))
            {
                transform.Translate(Vector3.down * scrollSpeed, Space.World);
            }

            //Left
            if (Input.mousePosition.x <= Screen.width * (1 - edge))
            {
                transform.Translate(Vector3.left * scrollSpeed, Space.World);
            }

            //Right
            if (Input.mousePosition.x >= Screen.width * edge)
            {
                transform.Translate(Vector3.right * scrollSpeed, Space.World);
            }
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(true);
            StaticGameOptions.isMenuOpen = true;
        }

    }

    void zoomOut(float delta)
    {
       zoomFactor-=delta;
    }

    void zoomIn(float delta)
    {
        zoomFactor-=delta;
    }

}
