using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float verticalInput;
    public float horizontalInput;
    public float speed = 10.0f;
    private Vector3 playerMove;
    private Vector3 horMove;
    private Vector3 forwMove;
    

    void Start()
    {
        
    }

    void Update()
    {
        MovePlayer();

       

    }

    void MovePlayer()
    {
        transform.Translate(playerMove);

        playerMove = (horMove + forwMove).normalized * Time.deltaTime * speed;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        horMove = Vector3.right * horizontalInput;
        forwMove = Vector3.forward * verticalInput;
    }
}
