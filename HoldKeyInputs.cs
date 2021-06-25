using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldKeyInputs : MonoBehaviour
{
    public bool keyHeld;
    
    void Start()
    {
        
    }

    void Update()
    {



    }

    private void OnEnable()
    {
        print(gameObject + " has spawned");
    }

    private void LateUpdate()
    {
        if (!keyHeld)
        {


        }
    }

}
