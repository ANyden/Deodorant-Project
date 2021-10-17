using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeGameModeSequence : MonoBehaviour
{
    private HeadTurning playerLook;
    private Hands handController;
    private RaycastSelector raySelector;
    public GameObject KeyPromptManager;
    private KeyInputSpawner keySpawner;
    private GameObject keyInputInterface;
    //public bool beginReadyUpSequence;
    [Range(-1,7)]
    public int gamePhase;
    void Start()
    {
        //beginReadyUpSequence = false;
        playerLook = GetComponent<HeadTurning>();
        handController = GetComponent<Hands>();
        raySelector = GetComponent<RaycastSelector>();
        keySpawner = KeyPromptManager.GetComponent<KeyInputSpawner>();
        keyInputInterface = KeyPromptManager.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        //if (beginReadyUpSequence)
        //{
        //    lockMouseControls();
        //    turnPlayer180();
        //    beginWalking(); //this will call another script
        //}

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            gamePhase++;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            gamePhase--;
        }

        switch (gamePhase)
        {
            case -1:
                phase_testing();
                break;
            case 0:         //At main menu
                break;
            case 1:         //walking to shelf
                break;
            case 2:         //choosing deoderant
                phase02_choosing();
                break;
            case 3:         //turning around, interface comes on, spawning is clear to start
                phase03_transition();
                break;
            case 4:         //rhythm game while walking
                phase04_rhythm();
                break;
            case 5:         //reset, move player back to shelf
                break;
            case 6:         //check out
                break;
            case 7:         //ending
                break;
        }
    }

    void phase_testing()
    {
        handController.activeInPhase = true;
        keySpawner.activeInPhase = true;

        keySpawner.mouseHeldDown = true;    //for testing

        if (keySpawner.mouseHeldDown)
        {
            if (handController.stickRight)
            {
                keySpawner.startSpawnfunction();    //starts spawn
                //gamePhase++;
            }

        }

        if (!keySpawner.mouseHeldDown)
        {
            if (keySpawner.startSpawn)
            {
                keySpawner.stopSpawning();
                //gamePhase++;
            }
        }

        

    }

    void phase02_choosing()
    {
        if (keySpawner.mouseHeldDown)
        {
            if (handController.stickRight)
            {
                keySpawner.startSpawnfunction();    //starts spawn
                gamePhase++;
            }
            
        }

        handController.activeInPhase = true;
        raySelector.activeInPhase = true;
        playerLook.activeInPhase = true;

    }
    public bool p03_ready, p03_start;
    void phase03_transition()
    {
        if (!p03_start)
        {

        }
        //Head turning is off
        //Selection Lazer is off
        //turn player around
        //turn Key Interface on
        //
        handController.activeInPhase = false;   //choice controls get locked (e no longer picks up or drops deoderant)
        playerLook.activeInPhase = false;       //head turning is off
        raySelector.activeInPhase = false;      //selection laser is off
        keyInputInterface.SetActive(true);      //KeyInputColumns are on
        keySpawner.activeInPhase = true;        //Spawner is ready to activate Spawning IENumerator
        //do a bunch of stuff then set p03_ready to true;

    }

    IEnumerator p03_setup()
    {
        //turn player 180 degrees
        //turn key Input Interface on
        yield return null;
    }

    void phase04_rhythm()
    {
        if (!keySpawner.mouseHeldDown)
        {
            if (keySpawner.startSpawn)
            {
                keySpawner.stopSpawning();
                gamePhase++;
            }
        }
        
        //keyInputInterface.SetActive(true);
        


        //Difficulty manager is on
        //camera walks to designated points.
    }

}
