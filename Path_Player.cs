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

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(pathLine[target].point_A, new Vector3(1, 1, 1));
        Gizmos.DrawCube(pathLine[target].point_B, new Vector3(1, 1, 1));


    }



    public void markA()
    {
        pathLine[target].point_A = transform.position;
    }
    public void markB()
    {
        pathLine[target].point_B = transform.position;

    }
}
