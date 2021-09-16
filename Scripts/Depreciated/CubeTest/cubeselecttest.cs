using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeselecttest : MonoBehaviour
{
    private cubes cube;
    void Start()
    {
        
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),out hit, 100) && hit.collider.tag =="cube")
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.green);

            cube = hit.collider.gameObject.GetComponent<cubes>();
            cube._selected = true;
        }
        else if(cube != null)
        {
            cube._selected = false;
            cube = null;
        }
    }
}
