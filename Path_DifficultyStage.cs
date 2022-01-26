using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathClass;
using DifficultyLevel;

public class Path_DifficultyStage : MonoBehaviour
{
    //public GameObject player;
    private Path_MovePlayer playerPaths;
    public GameObject spawner;
    private KeyInputSpawner noteSpawner;
    private pathSegment currentPath;
    private dLev diffLevel;
    void Start()
    {
        playerPaths = transform.GetComponent<Path_MovePlayer>();
        noteSpawner = spawner.GetComponent<KeyInputSpawner>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //updateSectionsInUse();
        }
    }

    //public void updateSectionsInUse()    //This removes the need for that long switch case in the
    //Manager. It DOES NOT add the sections of notes to the "Notes in Use" list. That is handled by the KeyInputSpawner.
    //{
    //    currentPath = playerPaths.currentPath;
    //    diffLevel = currentPath.difficulty;
    //    Debug.Log("DIFFICULTY UPDATED");

    //    for(int i = 0; i < diffLevel.sectionsInUse.Length; i++)
    //    {
    //        if (diffLevel.sectionsInUse[i])     //If sectionInUse is True...
    //        {
    //            noteSpawner.secInUseBools[i] = true;        //...Add that section to the "In Use" list.
    //            noteSpawner.secInUseBools_Held[i] = true;   //And it's "held" note counterparts
    //            //Debug.Log("section " + i + " is in use");
    //        }
    //        else
    //        {                                   //If sectionInUse is False...
    //            noteSpawner.secInUseBools[i] = false;       //It is marked as such
    //            noteSpawner.secInUseBools_Held[i] = false;  //As are it's "held" note counterparts.
    //            //Debug.Log("section " + i + "is not in use");
    //        }
            
    //    }
    //}
}
