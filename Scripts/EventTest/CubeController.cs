using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public GameEvent gameEvent;
    void Start()
    {
        GameEvent.current.onMakeCubeTrigger += OnSpawnCube;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameEvent.MakeCubeTrigger();
        }
    }

    private void OnSpawnCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 0, 0);
    }
}
