using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathClass;
using TMPro;

public class Path_MovePlayer : MonoBehaviour
{
    public GameObject PathManager;
    public float walkSpeed;
    public float modifiedWalkSpeed;
    private Player_MissedNote missedNote;
    public int currentPathNum = 0;
    public int currentTurnNum = 0;
    public pathSegment[] path;
    private int totalTurns;

    void Start()
    {
        missedNote = transform.GetComponent<Player_MissedNote>();
        path = PathManager.GetComponent<Path_Player>().pathLine;
        currentPath = path[currentPathNum];         //Gets current Path
        currentTurn = currentPath.turns[currentTurnNum];
        totalTurns = currentPath.turns.Length - 1;
    }
    public bool moving, turning;
    [HideInInspector]
    public pathSegment currentPath;
    [HideInInspector]
    public turnArounds currentTurn;
    public float playerDistance;
    float timeTracker;
    void Update()
    {
        timeTracker = Time.time;

        float rotateSpeedTime; rotateSpeedTime = 0;


        //These check if the Lerp is complete for walking and rotation, respectively.

        walkSpeedUpdate();

        if (Input.GetKeyDown(KeyCode.M))
        {
            Run();
        }
    }

    void walkSpeedUpdate()      //Uses health of player to modify walk speed
    {
        modifiedWalkSpeed =  missedNote.currentHealth * .1f;
        missedNote.DebugCanvas.transform.Find("DEBUG(health tracker)").transform.Find("DEBUGTEXT").GetComponent<TMP_Text>().SetText(modifiedWalkSpeed.ToString());

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
                stop();
            }
            
            nextTurn();
        }

        if (currentTurn.turning)
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (currentPath.allTurnsComplete)
        {
            stop();
            nextPath();

        }


    }

    void checkNumbers()
    {
        Debug.Log("currentPathNumber is " + currentPathNum + " limit is " + path.Length);
        Debug.Log("currentTurnNumber is " + currentTurnNum);
        currentPath = path[currentPathNum];
        if(currentPath.turns.Length > 0)
        {
            currentTurn = currentPath.turns[currentTurnNum];
            totalTurns = currentPath.turns.Length - 1;

        }
        else
        {
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
                }
                else
                {
                    if (currentTurn.turnComplete)
                    {
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

    void nextPath()
    {
        playerDistance = 0;
        if(currentPathNum != path.Length - 1)
        {
            currentPathNum++;

        }
        currentTurnNum = 0;
        checkNumbers();
        Run();
    }

    void Run()
    {

        //StartCoroutine(playerFollowPaths(currentPath, timeTracker));
        StartCoroutine(playerFollowPathsMoveTowards(currentPath));
        Debug.Log("BEGINNING TO FOLLOW PATH");

        
    }

    void stop()
    {
        Debug.Log("STOPPING");
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
            Vector3 relativePosi, currentTarget;
            float currentSpeed;

            
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
            float rotSpeedMod;
            //rotSpeedMod = 0f;
            //thisTurn.rotateSpeed = Mathf.Clamp(thisTurn.rotateSpeed, .4f, 1f);
            //thisTurn.rotateSpeed = thisTurn.rotateSpeed - .2f * Time.deltaTime;



            yield return null;
        }
    }

   
}
