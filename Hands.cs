using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    public RaycastSelector player;
    public GameObject rightHand;
    public GameObject rightShoulder;
    public float rotx, roty, rotz;

    public bool stickRight;
    public bool armUpR;
    private bool isArmUpNow;
    public Quaternion armDownRotation;
    void Start()
    {
        armDownRotation.x = rightShoulder.transform.rotation.eulerAngles.x;
        stickRight = false; //Nothing is held in the right hand on game start
        armUpR = false; //Arm is down on game start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!armUpR)
            {
                pickupR();
            }
            else
            {
                dropR();
            }
        }
    }


    void pickupR()
    {
        if (player.hitadeoderant.collider)
        {
            if (player.hitadeoderant.transform.tag == "Object")
            {
                Debug.Log("Gimme!");
                stickRight = true;
                armUpR = true;
                player.hitadeoderant.transform.position = rightHand.transform.position;
                player.hitadeoderant.transform.parent = rightHand.transform;
                //player.hitadeoderant.transform.SendMessage("beingHeld");
                player.hitadeoderant.transform.GetComponent<Deoderant>().beingHeld();
                //player.hitadeoderant.transform.GetComponent<Deoderant>().menuCallTrigger();

                liftArmR();

            }
        }
    }
    void dropR()
    {
        stickRight = false;
        rightHand.GetComponentInChildren<Deoderant>().iveBeenDropped();
        rightHand.transform.DetachChildren();

        dropArmR();
    }

    void liftArmR()
    {
        rightShoulder.transform.localRotation = Quaternion.Euler(-rotx, -roty, rotz);
        armUpR = true;
    }

    void dropArmR()
    {
        rightShoulder.transform.localRotation = Quaternion.Euler(armDownRotation.x, 0, 0);
        armUpR = false;
    }
}
