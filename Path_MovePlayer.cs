using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathClass;
using TMPro;
//THIS SCRIPT MOVES THE PLAYER ALONG THE PATHS
public class Path_MovePlayer : MonoBehaviour
{
    public GameObject PathManager;
    public float walkSpeed;
    public float modifiedWalkSpeed;
    [HideInInspector]
    public Player_MissedNote missedNote;
    public int currentPathNum = 0;
    public int currentTurnNum = 0;
    public pathSegment[] path;
    private int totalTurns;
    public int currentDifficulty;
    private Path_DifficultyStage difficultyStage;
    public GameObject spawner;
    private KeyInputSpawner spawnerScript;
    private changeGameModeSequence phaseManager;

    private void Awake()
    {
        phaseManager = GetComponent<changeGameModeSequence>();
        spawnerScript = spawner.GetComponent<KeyInputSpawner>();
        difficultyStage = transform.GetComponent<Path_DifficultyStage>();
        missedNote = transform.GetComponent<Player_MissedNote>();
        path = PathManager.GetComponent<Path_Player>().pathLine;
        currentPath = path[currentPathNum];         //Gets current Path
        currentTurn = currentPath.turns[currentTurnNum];
        totalTurns = currentPath.turns.Length - 1;
    }
    void Start()
    {
        //checkNumbers();
    }
    public bool moving, turning;
    [HideInInspector]
    public pathSegment currentPath;
    [HideInInspector]
    public turnArounds currentTurn;
    public float playerDistance;
    float timeTracker;

    bool justPath;

    void Update()
    {
        timeTracker = Time.time;

        //float rotateSpeedTime; rotateSpeedTime = 0;


        //These check if the Lerp is complete for walking and rotation, respectively.

        walkSpeedUpdate();

        if (Input.GetKeyDown(KeyCode.M))
        {
            justPath = true;
            Run();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            justPath = false;
            phaseManager.phase01_moveToShelf();
            phaseManager.gamePhase = 1;

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            checkNumbers();
            Debug.Log("Forcing a number check...");
        }

    }

    void walkSpeedUpdate()      //Uses health of player to modify walk speed
    {
        modifiedWalkSpeed =  missedNote.currentHealth * .1f;
        //missedNote.DebugCanvas.transform.Find("DEBUG(health tracker)").transform.Find("DEBUGTEXT").GetComponent<TMP_Text>().SetText(modifiedWalkSpeed.ToString());

        //Debug.Log(modifiedWalkSpeed);
    }

    private void LateUpdate()
    {
        //if (playerDistance == 1)
        //{
        //    path[currentPathNum].b_reached = true;

        //}

        if (currentPath.b_reached)
        {
            if (moving)
            {
                moving = false;
                Debug.LogWarning("Stopping because currentPath point B has been reached.");
                stop();
            }
            
            nextTurn();

            if (currentPath.allTurnsComplete)
            {
                Debug.Log("ALL TURNS COMPLETE");
                turning = false;
                if (currentPathNum == 0)
                {
                    if (justPath)
                    {
                        Debug.LogWarning("Stopping to go to Path 01 with justPath");
                        stop();
                        nextPath();
                    }
                    else
                    {
                        if (phaseManager.gamePhase == 1)         //If allTurnsComplete and at currentPathNum 0 and justPath is false, stop running FollowPaths and start Phase_02
                        {
                            Debug.LogWarning("Stopping to go to phase 02");
                            stop();
                            phaseManager.P01_check();
                        }
                    }

                }
                else
                {
                    Debug.LogWarning("Stopping to go to next path because allTurnsComplete");
                    stop();
                    nextPath();
                }

            }
        }

        if (currentTurn.turning)
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        

    }

    void checkNumbers()
    {
        currentPath = path[currentPathNum];
        Debug.Log("currentPathNumber is " + currentPathNum + " limit is " + path.Length);
        //Debug.Log("currentTurnNumber is " + currentTurnNum);
        Debug.Log("current difficulty is " + currentDifficulty);
        currentDifficulty = path[currentPathNum].difficulty.level;
        //difficultyStage.updateSectionsInUse();
        spawnerScript.includeSections3();       //This SHOULD change the items in the NotesInUse list.
        if(currentPath.turns.Length > 0)
        {
            currentTurn = currentPath.turns[currentTurnNum];
            totalTurns = currentPath.turns.Length - 1;

        }
        else
        {
            Debug.LogWarning("No turns found on this path- marking as allTurnsComplete");
            currentPath.allTurnsComplete = true;
            totalTurns = 0;
        }

    }

    void nextTurn()
    {
        Quaternion currentRot; currentRot = transform.rotation;

        if (!currentPath.allTurnsComplete)
        {
            if(currentPath.turns.Length == 0)
            {
                currentPath.allTurnsComplete = true;
                playerRotation = 0;
                nextPath();
            }
            else
            {
                if (!currentTurn.turning)
                {
                    Debug.Log("BEGINNING TURN");
                    playerRotation = 0;
                    StartCoroutine(playerFollowTurns(currentPath, currentTurn, currentRot));
                    currentTurn.turning = true;

                    //make a new path between p0turn1 and p0turn2
                }
                else
                {
                    if (currentTurn.turnComplete)
                    {
                        Debug.LogWarning("Stopping because turn is complete");
                        stop();
                        Debug.Log("TURN COMPLETE");

                        
                        if (currentTurnNum != totalTurns)
                        {
                            currentTurnNum++;
                            Debug.Log("currentTurnNum is " + currentTurnNum);
                            checkNumbers();
                            nextTurn();

                        }
                        else
                        {
                            currentPath.allTurnsComplete = true;
                            playerRotation = 0;
                        }

                    }

                }
                
            }
            
        }
    }

    public void nextPath()
    {
        playerDistance = 0;
        currentTurnNum = 0;

        Debug.Log("Next Path");

        
        if (currentPathNum != path.Length - 1)      //If not on the last path in the list...
        {
            currentPathNum++;
            checkNumbers();
            Run();
        }
        
        
        
    }

    public void Run()
    {

        //StartCoroutine(playerFollowPaths(currentPath, timeTracker));
        StartCoroutine(playerFollowPathsMoveTowards(currentPath));
        Debug.Log("BEGINNING TO FOLLOW PATH");
        
    }

    public void stop()
    {
        //Debug.Log("STOPPING");
        StopAllCoroutines();

    }

    IEnumerator playerFollowPaths(pathSegment thisPath, float pathStartTime)
    {
        while (!thisPath.b_reached)
        {
            float totalDist = Vector3.Distance(thisPath.point_A, thisPath.point_B);
            float distTravelled = (Time.time - pathStartTime) * walkSpeed;
            float percentageOfPathTravelled = Mathf.Clamp(distTravelled / totalDist, 0, 1);
            playerDistance = percentageOfPathTravelled;

            transform.position = Vector3.Lerp(thisPath.point_A, thisPath.point_B, percentageOfPathTravelled);
            if (playerDistance == 0)
            {
                currentPath.b_reached = true;

            }
            yield return null;
        }
        //yield return null;
    }

    IEnumerator playerFollowPathsMoveTowards(pathSegment thisPath)
    {
        while (!thisPath.b_reached)
        {
            float step = modifiedWalkSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, thisPath.point_B, step);

            playerDistance = Vector3.Distance(transform.position, thisPath.point_B);
            if (playerDistance == 0)
            {
                currentPath.b_reached = true;
            }
            yield return null;
        }
    }

    float playerRotation; //float rotationTime = 0; float OverShootTime = 0;  //playerRotation is the total, rotationTime is used when there is no overshoot, overshootTime is used when there is an overshoot
    //float playerRotation2 = 0;
    IEnumerator playerFollowTurns(pathSegment thisPath, turnArounds thisTurn, Quaternion startingRot)
    {
        while (!thisTurn.turnComplete)
        {
            Vector3 relativePosi; //currentTarget;
            //float currentSpeed;

            
            relativePosi = thisTurn.rotateTo - transform.position;
            Quaternion targetRot;
            targetRot = Quaternion.LookRotation(relativePosi);
            playerRotation += Time.deltaTime * thisTurn.rotateSpeed;
            transform.rotation = Quaternion.Lerp(startingRot, targetRot, playerRotation);

            if (playerRotation > .8)
            {
                //thisTurn.rotateSpeed = Mathf.
            }
            else
            {

            }
            if (thisTurn.pause)
            {
                if(playerRotation > 1)
                {
                    phaseManager.P01_check();
                    yield return new WaitForSeconds(thisTurn.pauseTime);
                    thisTurn.turnComplete = true;
                }
            }
            else
            {
                if(playerRotation> 1)
                {
                    thisTurn.turnComplete = true;
                }
            }


            Debug.Log("TURNING");
            //float rotSpeedMod;
            //rotSpeedMod = 0f;
            //thisTurn.rotateSpeed = Mathf.Clamp(thisTurn.rotateSpeed, .4f, 1f);
            //thisTurn.rotateSpeed = thisTurn.rotateSpeed - .2f * Time.deltaTime;



            yield return null;
        }
    }

    


}
