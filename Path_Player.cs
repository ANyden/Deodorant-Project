using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathClass;

[ExecuteInEditMode]
public class Path_Player : MonoBehaviour
{
    public GameObject player;
    public pathSegment[] pathLine;
    public int target;
    private Gizmos[] gizmoCubes;
    [HideInInspector]
    public int turnTarget;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnDrawGizmosSelected()
    {
        drawMarkerCube();

    }

    public void drawMarkerCube()
    {
        for (int i = 0; i < pathLine.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(pathLine[i].point_A, new Vector3(1, 1, 1));
            Gizmos.DrawCube(pathLine[i].point_B, new Vector3(1, 1, 1));
        }

        for (int i = 0; i < pathLine[target].turns.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(pathLine[target].turns[i].rotateTo, new Vector3(1, 1, 1));
        }
    }


    public void markA()
    {
        pathLine[target].point_A = transform.position;
    }
    public void markB()
    {
        pathLine[target].point_B = transform.position;
    }

    public void nextTarget()
    {
        target++;
    }

    public void prevTarget()
    {
        target--;
    }

    public void moveMarkerA()
    {
        transform.position = new Vector3(pathLine[target].point_A.x, pathLine[target].point_A.y, pathLine[target].point_A.z);
    }

    public void markTurn()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.white, 5f);
        Vector3 endOfRay;
        endOfRay = transform.position + transform.forward * 5f;

        pathLine[target].turns[turnTarget].rotateTo = endOfRay;
    }

    public void nextTurnTarget()
    {
        turnTarget++;
    }

    public void prevTurnTarget()
    {
        turnTarget--;
    }
}
