using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSelector : MonoBehaviour
{
    public RaycastHit hitadeoderant;
    public SelectionManager SelectionManager;
    public Hands player;
    public bool activeInPhase;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (activeInPhase)
        {
            SelectionLaser();
        }
    }

    public void SelectionLaser()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitadeoderant, 5) && hitadeoderant.transform.tag == "Object")
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5, Color.green);
            //Debug.Log("I hit " + hitadeoderant.collider);
            //Debug.Log("I hit a thing");
            //lookingAtObject = true;

        }
        else
        {
            //lookingAtObject = false;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5, Color.red);
        }
    }
}
