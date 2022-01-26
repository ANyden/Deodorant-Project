using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PathClass;


public class HeadTurning : MonoBehaviour
{
    public float lookSpeed = 200f;

    private float rotY = 0.0f;
    private float rotX = 0.0f;

    private float clampAngle = 90.0f;

    public bool cursorLockedToCenterScreen;
    public bool activeInPhase;

    public Path_MovePlayer pathMovement;
    public changeGameModeSequence gamePhase;
    public Canvas rotTracker;
    //private bool lookLock;
    public bool clampActive;

    //public bool playerIsLooking;

    void Start()
    {
        //SelectionManager = SelectionManager.GetComponent<SelectionManager>();

        //playerIsLooking = true;
        cursorLockedToCenterScreen = true;
        //Debug.LogWarning("cursorLock is " + cursorLockedToCenterScreen);
        //lookLock = false;

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }
    public bool mouseCentered = false;

    

    void Update()
    {
        
        //when cursorLocked is true, the player can turn their head and look around using the mouse.
        if (cursorLockedToCenterScreen)
        {
            //playerCanTurnHead = true;
            Cursor.lockState = CursorLockMode.Locked;

        }
        //Allows the player to turn their head and look around.
        if (activeInPhase)
        {
            //rotTracker.transform.Find("DEBUG(rot tracker)").transform.GetChild(0).GetComponent<Text>().text = rotX.ToString() + ", " + rotY.ToString();

            Cursor.lockState = CursorLockMode.Locked;
            float mouseX, mouseY;
            mouseX = Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
            mouseY = -Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;
            Quaternion endOfTurnRot; int endOfTurnRotInt = pathMovement.currentPathNum - 1;
            //int selectedPath_FinalTurn; 
            //selectedPath_FinalTurn = pathMovement.path[selectedPath()].turns.Length - 1;
 
            endOfTurnRot = Quaternion.LookRotation(pathMovement.path[selectedPath()].turns[selectedPath_FinalTurn()].rotateTo - pathMovement.path[selectedPath()].point_B);
            //Debug.LogWarning("Will turn towards Path " + selectedPath() + " ,Turn " + selectedPath_FinalTurn());


            

            //rotTracker.transform.GetChild(1).GetComponent<Text>().text = endOfTurnRot.eulerAngles.ToString();                 The Rot Tracker needs to be the same for both test scenes.
            if (pathMovement.turning)
            {
                rotX = transform.rotation.x;
                rotY = transform.rotation.y;
                mouseCentered = false;
            }
            else
            {
                if (!mouseCentered)
                {
                    if (pathMovement.path[selectedPath()].turns[selectedPath_FinalTurn()].rotateSpeed > 0)
                    {
                        rotX = endOfTurnRot.eulerAngles.x;
                        rotY = endOfTurnRot.eulerAngles.y;
                        mouseX = 0;
                        mouseY = 0;
                        mouseCentered = true;
                    }
                    else
                    {
                        rotX = transform.rotation.eulerAngles.x;
                        rotY = transform.rotation.eulerAngles.y;
                        mouseX = 0;
                        mouseY = 0;
                        mouseCentered = true;
                    }
                }

                
            }


            //print("HEAD TURNING ON");

            if (clampActive)
            {
                

                rotY += mouseX;
                rotX += mouseY;

                rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
                //rotY = Mathf.Clamp(rotY, -clampAngle, clampAngle);

                Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);

                

                transform.rotation = localRotation;

                


            }
            else
            {
                rotY += mouseX;
                rotX += mouseY;




                Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
                transform.rotation = localRotation;
            }
            


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
    int backAmount; turnArounds lastSuccessfulTurn;
    int selectedPath()
    {
        int prevPath;
        if (pathMovement.currentPathNum == 0)           //If on Path 0...
        {
            prevPath = 0;

        }
        else
        {

            if (pathMovement.path[pathMovement.currentPathNum].turns.Length != 0)       //If current path has 0 turns...
            {
                backAmount = 1;
                prevPath = pathMovement.currentPathNum - backAmount;                    //...Use the Path that is the currentPathNum - 1

            }
            else
            {
                backAmount++;                                                           
                prevPath = pathMovement.currentPathNum - backAmount;

            }
        }
        return prevPath;
    }

    int selectedPath_FinalTurn()
    {
        int finalTurn;
        
        
        finalTurn = pathMovement.path[selectedPath()].turns.Length - 1;
        


        return finalTurn;
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
