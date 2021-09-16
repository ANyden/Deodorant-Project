using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public RaycastSelector player;
    public Material highlightMaterial;
    public Material[] defaultMaterial;
    private Deoderant stick;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<RaycastSelector>();
    }
    //Highlight System
    void Update()
    {

        if (player.hitadeoderant.collider && player.hitadeoderant.transform.tag == "Object")
        {
            stick = player.hitadeoderant.collider.gameObject.GetComponent<Deoderant>();
            stick._selected = true;
        }
        else if (stick != null)
        {
            stick._selected = false;
            stick = null;
        }
        else if (stick == null)
        {

        }

    }

}
