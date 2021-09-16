using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameButtonSwitch : MonoBehaviour
{
    private GameObject ButtonA;
    private GameObject ButtonB;
    private Vector3 ButtonAOrigin;
    private Vector3 ButtonBOrigin;
    public bool Switched;
    private void Awake()
    {
        ButtonA = GameObject.Find("L1Yes");
        ButtonB = GameObject.Find("L1No");

        ButtonAOrigin = ButtonA.transform.position;
        ButtonBOrigin = ButtonB.transform.position;

        
        Switched = false;

        //ButtonA.AddComponent(SwitchA)()
    }
    void Start()
    {
        
    }

    void Update()
    {
        //This minigame will switch the positions of the buttons once on mouse over.
        //On Awake, mark transform positions of the buttons.
        //On MouseOver, transform.position = the other button's
        //Mark a bool as true.

    }

    private void OnMouseOver()
    {
        if (!Switched)
        {
            ButtonA.transform.position = ButtonBOrigin;
            ButtonB.transform.position = ButtonAOrigin;
            Switched = true;
        }
    }
}
