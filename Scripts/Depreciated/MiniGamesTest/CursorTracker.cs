using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTracker : MonoBehaviour
{
    public float horInput;
    public float vertInput;
    public bool still;
    public float MouseSpeed;
    public Vector3 screenPoint;
    public Vector3 CursorLoc;

    private Renderer myColor;
    void Start()
    {
        myColor = GetComponent<Renderer>();
    }

    public void Update()
    {
        screenPoint = Input.mousePosition;
        screenPoint.z = 15f;
        CursorLoc = Camera.main.ScreenToWorldPoint(screenPoint);
        transform.position = CursorLoc;

        horInput = Input.GetAxis("Mouse X");
        vertInput = Input.GetAxis("Mouse Y");

        MouseSpeed = horInput + vertInput;

        if (MouseSpeed == 0)
        {
            still = true;
        }
        else
        {
            still = false;
        }

        if (still)
        {
            myColor.material.SetColor("_Color", Color.red);
        }
        else
        {
            myColor.material.SetColor("_Color", Color.blue);
        }

    }

    public void lateUpdate()
    {
        
    }


}
