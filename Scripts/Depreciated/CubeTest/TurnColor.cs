using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnColor : MonoBehaviour
{
    public bool onOrOff;
    private Renderer MyRenderer;
    public GameObject eventmanager;

    private void Start()
    {
        MyRenderer = GetComponent<Renderer>();
        onOrOff = false;
    }
    public void OnOff()
    {
        switch (onOrOff)
        {
            case false:
                onOrOff = true;
                EventManager.OnClicked += ChangeColor;
                eventmanager.SendMessage("aaa");
                break;
            case true:
                onOrOff = false;
                EventManager.OnClicked -= ChangeColor;
                eventmanager.SendMessage("aaa");
                break;


        }
    }

    void ChangeColor()
    {
        Color col = new Color(Random.value, Random.value, Random.value);
        MyRenderer.material.color = col;
    }
}
