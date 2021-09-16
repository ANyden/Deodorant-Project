using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButtonChase : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    public float horizontalInput;
    public float vertInput;
    private Vector3 origin;
    public GameObject Center;

    void Awake()
    {

        Center = GameObject.Find("GameObject");
        origin = Center.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Mouse X");
        vertInput = Input.GetAxis("Mouse Y");
        mOffset = origin - GetMouseWorldPos();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            mOffset = Center.transform.position - GetMouseWorldPos();

            //GameObject testCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //testCube.transform.position = GetMouseWorldPos();

        }
    }

    //tracking mousePos
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

}
