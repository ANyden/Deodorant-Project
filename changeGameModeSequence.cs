using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeGameModeSequence : MonoBehaviour
{
    private HeadTurning playerLook;
    public bool beginReadyUpSequence;
    void Start()
    {
        beginReadyUpSequence = false;
        playerLook = GetComponent<HeadTurning>();
    }

    void Update()
    {
        if (beginReadyUpSequence)
        {
            lockMouseControls();
            turnPlayer180();
            beginWalking(); //this will call another script
        }
    }

    //Controls get locked out
    void lockMouseControls()
    {
        playerLook.playerCanTurnHead = false;
    }
    //Player turns 180 degrees
    public Quaternion playerLookCenter = Quaternion.identity;
    void turnPlayer180()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, playerLookCenter, 100f * Time.deltaTime);
    }
    //Player starts walking
    void beginWalking()
    {

    }
}
