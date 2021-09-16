using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public static GameEvent current;


    //So, the trick is to have it loaded beforehand, and then have something else trigger it. It feels like a lot of extra steps. Could this really make it more organized?

    private void Awake()
    {
        current = this;
    }
    private void Update()
    {

    }


public event Action onMakeCubeTrigger;
    public void MakeCubeTrigger()
    {
        if (onMakeCubeTrigger != null)
        {
            onMakeCubeTrigger();
        }
    }
}
