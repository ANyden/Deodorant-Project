using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deoderant : MonoBehaviour
{
    private Rigidbody rb;
    public bool _selected;
    public SelectionManager selectionManager;

    public enum State { shelved, inHand, dropped };
    public State stickState;

    //public int menuPromptID;
    public int stickID;
    public int anxietyID;
    //private int totalStickNum;

    //to do:
    //Put deoderants back on the shelf
    //Selecting one triggers menu

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        stickState = State.shelved;



        //set an int to be sent in doTheMenuCall. For now, I guess just set a random range. Later I should make a spawner.
        anxietyID = Random.Range(0, 5);
    }

    void Update()
    { 
        //highlight system
        if (_selected)
        {
            gameObject.GetComponent<Renderer>().material = selectionManager.highlightMaterial;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = selectionManager.defaultMaterial[anxietyID];
        }
    }

    //This controls how gravity effects the deoderant stick
    void doIFall()
    {
        switch (stickState)
        {
            case State.shelved:
                rb.useGravity = true;
                break;
            case State.inHand:
                rb.useGravity = true;
                rb.isKinematic = true;
                break;
            case State.dropped:
                rb.useGravity = true;
                rb.isKinematic = false;
                break;
        }
    }

    //This function is called by Hands "drop" function
    public void iveBeenDropped()
    {
        stickState = State.dropped;
        //Debug.Log(gameObject.name + "was dropped");
        doIFall();
    }

    //This function is called by Hands "pick up" function
    public void beingHeld()
    {
        stickState = State.inHand;
        doIFall();
    }

}
