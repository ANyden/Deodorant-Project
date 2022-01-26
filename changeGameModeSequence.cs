using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//THIS SCRIPT CHANGES GAME PHASES
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
    private Path_MovePlayer followPaths;
    void Start()
    {
        followPaths = GetComponent<Path_MovePlayer>();
        playerLook = GetComponent<HeadTurning>();
        handController = GetComponent<Hands>();
        raySelector = GetComponent<RaycastSelector>();
        keySpawner = KeyPromptManager.GetComponent<KeyInputSpawner>();
        keyInputInterface = KeyPromptManager.transform.GetChild(0).gameObject;

        if(gamePhase == -1)
        {
            phase_testing();

        }

        playerLook.clampActive = true;
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            gamePhase++;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            gamePhase--;
        }

        if(gamePhase == -1)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                keySpawner.startSpawnfunction();
                Debug.Log("Manually starting spawn");
            }
        }

        
    }

    public void phase_testing()
    {
        handController.activeInPhase = true;
        keySpawner.activeInPhase = true;
        playerLook.activeInPhase = true;
        raySelector.activeInPhase = true;
        playerLook.clampActive = false;
        keyInputInterface.SetActive(true);

        //keySpawner.mouseHeldDown = true;    //for testing
        playerLook.activeInPhase = true;

        //keySpawner.startSpawnfunction();
        //keySpawner.startSpawn = true;
        
    }
    public bool inPhase_01;
    public void phase01_moveToShelf()
    {
        if (!inPhase_01)
        {
            followPaths.currentPathNum = 0;

            handController.activeInPhase = false;
            raySelector.activeInPhase = false;
            playerLook.activeInPhase = true;
            playerLook.clampActive = true;          //having this on makes the transition weird.

            followPaths.Run();
            inPhase_01 = true;
        }
        
    }

    public void P01_check()
    {
        
        toPhase02();
        gamePhase++;
    }

    void toPhase02()
    {
        Debug.Log("INITIATING PHASE 2");
        
        //followPaths.stop();
        //followPaths.currentTurn.turnComplete = true;
        //Move to Phase 2
        phase02_choosing();
        handController.activeInPhase = true;
        raySelector.activeInPhase = true;
        playerLook.activeInPhase = true;
        playerLook.clampActive = true;
    }

    bool inPhase_02, inPhase_03;
    public void phase02_choosing()
    {
        if (inPhase_02)
        {
            if (handController.stickRight)      //This is where the OCD_Shadow interactions will happen.
            {
                if (keySpawner.mouseHeldDown)
                {
                    if (!inPhase_03)
                    {
                        gamePhase++;
                        phase03_transition();
                        inPhase_03 = true;
                    }
                }
            }
        }
        
    }
    void phase03_transition()       //Phase 3 is getting ready. Turn forward, interface appears.
    {
        if (inPhase_03)
        {
            Debug.Log("Phase 03 starting");
            followPaths.nextPath();
            
            handController.activeInPhase = false;   //choice controls get locked (e no longer picks up or drops deoderant)
            playerLook.activeInPhase = true;       //head turning is on
            playerLook.clampActive = false;
            raySelector.activeInPhase = false;      //selection laser is off
            keyInputInterface.SetActive(true);      //KeyInputColumns are on
            keySpawner.activeInPhase = true;        //Spawner is ready to activate Spawning IENumerator
                                                    //followPaths.currentPathNum++;
                                                    //followPaths.Run();                  //Starts Movement



            //do a bunch of stuff then set p03_ready to true;
            inPhase_02 = false;
            inPhase_03 = false;
        }

    }
    bool inPhase_04;
    public void toPhase04()
    {
        if (!inPhase_04)
        {
            phase04_rhythm();

        }
    }
    bool p04_startSpawnFlag = false;

    void phase04_rhythm()
    {
        inPhase_04 = true;
        if (!p04_startSpawnFlag)
        {
            keySpawner.startSpawnfunction();    //starts spawn
            p04_startSpawnFlag = true;
        }

        if (!keySpawner.mouseHeldDown)
        {
            followPaths.missedNote.failState = true;
            Debug.Log("MOUSE RELEASED. FAILSTATE ACTIVE.");
        }

        //camera walks to designated points.
    }

    void phase05_restart()
    {
        if (keySpawner.startSpawn)
        {
            keySpawner.stopSpawning();
        }
    }

    

}
