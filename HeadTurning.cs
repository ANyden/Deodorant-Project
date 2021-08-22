using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeadTurning : MonoBehaviour
{
    public float lookSpeed = 200f;

    private float rotY = 0.0f;
    private float rotX = 0.0f;

    private float clampAngle = 90.0f;

    public bool cursorLockedToCenterScreen;
    public bool playerCanTurnHead;
    //private bool lookLock;

    //public bool playerIsLooking;

    void Start()
    {
        //SelectionManager = SelectionManager.GetComponent<SelectionManager>();

        //playerIsLooking = true;
        cursorLockedToCenterScreen = true;
        Debug.LogWarning("cursorLock is " + cursorLockedToCenterScreen);
        //lookLock = false;

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void Update()
    {
        //when cursorLocked is true, the player can turn their head and look around using the mouse.
        if (cursorLockedToCenterScreen)
        {
            //playerCanTurnHead = true;
            Cursor.lockState = CursorLockMode.Locked;

        }
        //Allows the player to turn their head and look around.
        if (playerCanTurnHead)
        {
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
            float mouseY = -Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;

            rotY += mouseX;
            rotX += mouseY;

            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
            rotY = Mathf.Clamp(rotY, -clampAngle, clampAngle);

            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            transform.rotation = localRotation;



            //z needs to equal zero on local at all times

        }

        //SELECTION LASER WAS MOVED TO RAYCASTSELECTOR


        //To control cursor lock state.
        //switch (cursorLock)
        //{
        //    case true:
        //        Cursor.lockState = CursorLockMode.Locked;
        //       //Debug.Log("Cursor is locked");
        //        break;
        //    case false:
        //        Cursor.lockState = CursorLockMode.Confined;
        //        //print("cursor is free");
        //        break;
        //}

    }



    //Mouse lock. When True prevents cursor movement, allows head turning. When False allows cursor movement, but locks head position.
    //public void unlockCursor()
    //{
    //    cursorLock = false;
    //   lookLock = true;
    //}

    public void unlockCursor() 
    {
        cursorLockedToCenterScreen = false;

    }

    public void lockCursor()
    {
        cursorLockedToCenterScreen = true;
    }
}
