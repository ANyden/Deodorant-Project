using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movetowardsmepls : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject Home;
    private Vector3 HomeZone;

    public enum State { comingHome, fleeingHome }
    public State movementStatus;
    public bool atHome;

    private CursorTracker CursorInfo;

    public float xRange = 18f;
    public float yRange = 13f;

    public float FleeRange = 5f;
    private Vector3 FleeForce;
    private float moveSpeedNow;
    private Collider BoxCollider;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        HomeZone = Home.transform.position;

        CursorInfo = GameObject.FindGameObjectWithTag("Cursor").GetComponent<CursorTracker>();

        BoxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        Boundries();

        if (CursorInfo.still)
        {
            movementStatus = State.comingHome;
        }

        if (!CursorInfo.still)
        {
            movementStatus = State.fleeingHome;
        }

        if (transform.position == HomeZone)
        {
            atHome = true;
        }
        else
        {
            atHome = false;
        }

    }

    private void FixedUpdate()
    {
        switch (movementStatus)
        {
            case State.fleeingHome:
                Vector3 direction = (CursorInfo.CursorLoc - transform.position).normalized;
                rb.velocity = new Vector3(-direction.x * 20f, -direction.y * 20f, direction.z * 1);
                break;
            case State.comingHome:
                rb.velocity = rb.velocity * .5f;
                if (rb.velocity == Vector3.zero)
                {
                    transform.Translate(comingHome() * Time.deltaTime);
                }
                break;

        }
    }

    Vector3 comingHome()
    {
        return HomeZone-transform.position;  
    }

    public void Boundries()
    {
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
        if (transform.position.y < -yRange)
        {
            transform.position = new Vector3(transform.position.x, -yRange, transform.position.z);
        }
        else if (transform.position.y > yRange)
        {
            transform.position = new Vector3(transform.position.x, yRange, transform.position.z);
        }
    }
}
