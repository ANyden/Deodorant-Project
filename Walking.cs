using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
    public GameObject Player;
    private HeadTurning playerLook;
    private Hands hands;
    private bool playerWalking;


    // Start is called before the first frame update
    void Start()
    {
        playerLook = Player.GetComponent<HeadTurning>();
        hands = Player.GetComponent<Hands>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!playerLook.playerIsLooking)
        //{
        //    if (!playerWalking)
        //    {
        //        StartCoroutine(startWalking());
        //        playerWalking = true;
        //        playerTurn180Degrees();
        //    }
        //}
    }

    IEnumerator startWalking()
    {
        playerLook.unlockCursor();
        //lock player out of controls
        
        //turn player around

        //move player away from the shelf

        yield return null;
    }

    //turns player around
    void playerTurn180Degrees()
    {

    }
}
