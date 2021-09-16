using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeGameModeSequence : MonoBehaviour
{
    private HeadTurning playerLook;
    private Hands handController;
    private RaycastSelector raySelector;
    public GameObject keyInputInterface;
    public bool beginReadyUpSequence;
    private int gamePhase;
    void Start()
    {
        beginReadyUpSequence = false;
        playerLook = GetComponent<HeadTurning>();
        handController = GetComponent<Hands>();
        raySelector = GetComponent<RaycastSelector>();
        keyInputInterface = keyInputInterface.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        //if (beginReadyUpSequence)
        //{
        //    lockMouseControls();
        //    turnPlayer180();
        //    beginWalking(); //this will call another script
        //}

        switch (gamePhase)
        {
            case 0:         //At main menu
                break;
            case 1:         //walking to shelf
                break;
            case 2:         //choosing deoderant
                break;
            case 3:         //rhythm game while walking
                break;
            case 4:         //reset, move player back to shelf
                break;
            case 5:         //check out
                break;
            case 6:         //ending
                break;
        }
    }

    void phase02_choosing()
    {
        handController.activeInPhase = true;
        raySelector.activeInPhase = true;
        playerLook.activeInPhase = true;
    }

    void phase03_rhythm()
    {
        handController.activeInPhase = false;   //choice controls get locked (e no longer picks up or drops deoderant)
        playerLook.activeInPhase = false;       //head turning is off
        raySelector.activeInPhase = false;      //selection laser is off
        keyInputInterface.SetActive(true);
        //Difficulty manager is on
        //camera walks to designated points.
    }

}
