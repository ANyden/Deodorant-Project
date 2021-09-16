using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class MenuEvents : MonoBehaviour
{
    public static MenuEvents current;
    public event Action menuOpen;
    public event Action menuClose;

    //public int[] anxietyNum;

    public GameObject container;
    public HeadTurning player;

    //private GameObject Held;
    public void Awake()
    {
        current = this;
    }

    private void Start()
    {
        container = transform.GetChild(1).gameObject;
        //Held = player.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(menuOpen);
        }
    }

    public void menuActivation()
    {
        if (menuOpen != null)
        {
            menuOpen(); //This function activates the menu
            
           
        }
    }

    public void MenuDeactivation()
    {
        if (menuClose != null)
        {
            menuClose();
        }
        
    }

}
