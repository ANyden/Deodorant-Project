using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathClass;

public class Path_MovePlayer : MonoBehaviour
{
    public GameObject PathManager;
    public float walkSpeed;
    private int currentPathNum = 0;
    private int currentTurnNum = 0;
    private pathSegment[] path;
    private int totalTurns;
    void Start()
    {
        path = PathManager.GetComponent<Path_Player>().pathLine;
        currentPath = path[currentPathNum];         //Gets current Path
        currentTurn = currentPath.turns[currentTurnNum];
        totalTurns = currentPath.turns.Length - 1;
    }
    public bool moving;
    pathSegment currentPath;
    turnArounds currentTurn;
    public float playerDistance;
    float timeTracker;
    void Update()
    {
        timeTracker = Time.time;
        

        //These check if the Lerp is complete for walking and rotation, respectively.
        if (playerDistance == 0)
        {
            currentPath.a_reached = true;
            
        }
        if(rotationTime > 1)
        {
            currentTurn.turnComplete = true;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            start2();
        }
    }

    private void LateUpdate()
    {
        if (playerDistance == 1)
        {
            path[currentPathNum].b_reached = true;

        }

        if (currentPath.b_reached)
        {
            if (moving)
            {
                moving = false;
                stop();
            }
            
            nextTurn();


        }

        if (currentPath.allTurnsComplete)
        {
            stop();
            nextPath();

        }


    }

    void checkNumbers()
    {
        currentPath = path[currentPathNum];
        currentTurn = currentPath.turns[currentTurnNum];
        totalTurns = currentPath.turns.Length - 1;

    }

    void nextTurn()
    {
        Quaternion currentRot; currentRot = transform.rotation;

        if (!currentPath.allTurnsComplete)
        {
            if (!currentTurn.turning)
            {
                Debug.Log("BEGINNING TURN");
                rotationTime = 0;
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
                        rotationTime = 0;
                    }

                }

            }
        }
    }

    void nextPath()
    {
        playerDistance = 0;
        currentPathNum++;
        currentTurnNum = 0;
        checkNumbers();
        start2();
    }

    void start2()
    {
        
        StartCoroutine(playerFollowPaths(currentPath, timeTracker));
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
            yield return null;
        }
        //yield return null;
    }
    float rotationTime = 0;

    IEnumerator playerFollowTurns(pathSegment thisPath, turnArounds thisTurn, Quaternion startingRot)
    {
        while (!thisTurn.turnComplete)
        {
            Vector3 relativePosi;
            relativePosi = thisTurn.rotateTo - transform.position;
            Quaternion targetRot;
            targetRot = Quaternion.LookRotation(relativePosi);
            rotationTime += Time.deltaTime * thisTurn.rotateSpeed;
            transform.rotation = Quaternion.Lerp(startingRot, targetRot, rotationTime);
            Debug.Log("TURNING");
            

            yield return null;
        }
    }

    Quaternion targetRot(turnArounds thisTurn, pathSegment thisPath)
    {
        float xTarget, yTarget, zTarget; xTarget = thisPath.point_B.x; yTarget = thisPath.point_B.y; zTarget = thisPath.point_B.z;
        switch (thisTurn.turnAxis)
        {
            case turnArounds.axis.xTurn:
                zTarget = thisPath.point_B.x + 1;
                break;
            case turnArounds.axis.yTurn:
                xTarget = thisPath.point_B.y + 1;
                break;
            case turnArounds.axis.zTurn:
                yTarget = thisPath.point_B.z + 1;
                break;
        }
        Quaternion targetToRotateTo;
        Vector3 relativePosi, target;
        target = new Vector3(xTarget, yTarget, zTarget);
        relativePosi = target - transform.position;
        targetToRotateTo = Quaternion.LookRotation(relativePosi);
        Debug.DrawLine(thisPath.point_B, relativePosi, Color.red);
        return targetToRotateTo;
    }
   
}
