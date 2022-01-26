using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DifficultyLevel;
using PathClass;
//DEPRECIATED
public class DifficultyManger : MonoBehaviour
{
    [Range(0, 10)]
    public int currentDifficulty;                  //Difficulty stage
    public dLev[] difficultyLevels;
    public float globalProgressModifier;    //How fast the keyNote disappears when held
    public float refreshRate; //How fast new button prompts appear
    public float moveSpeed;
    //public float moveSpeedModifier;         //Added onto global speed
    [Range(0,10)]
    public int holdChance;
    [Range(0, 10)]
    [Tooltip("Ratio is Hold:Tap, the lower Tap is the lower the chance of appearing")]
    public int tapChance;                   //number that determines how likely a note is to spawn 'tapped'
    public int notesToNextStage;                 //How many notes need to be completed to get to the next difficulty level
    public bool readyForNextStage;
    private KeyInputSpawner SpawnerScript;


    void Start()
    {
        SpawnerScript = GetComponent<KeyInputSpawner>();
        for (int i = 0; i < difficultyLevels.Length; i++)
        {
            difficultyLevels[i].holdChance = 10 - difficultyLevels[i].tapChance;
        }

    }

    void FixedUpdate()
    {
        if(SpawnerScript.notesCompletedThisStage >= notesToNextStage)
        {
            readyForNextStage = true;

        }

    }

    private void LateUpdate()
    {
        //if (readyForNextStage)
        //{
        //    readyForNextStage = false;
        //    currentDifficulty++;
        //    changeDifficulty();
        //}
    }
    

    //public void changeDifficulty()
    //{
    //    if(currentDifficulty < 6)       //Locks difficulty max at 5 for now
    //    {
    //        //SpawnerScript.clearNotesInUse();

    //        difficulty_Switch_notesToNextDiff();
    //        //difficultyLevelChange();
    //        difficulty_Switch_tapChance();
    //        difficulty_Switch_speedModifier();
    //        difficulty_Switch_refreshRate();
    //        //difficulty_Sections();
    //        SpawnerScript.includeSections2();
    //        Debug.Log("Difficulty Level parameters have been changed");
    //    }
    //    else
    //    {
    //        SpawnerScript.startEndPhase();
    //        SpawnerScript.stopSpawning();
    //    }
    //}

    public int[] tChance;
    private void difficulty_Switch_tapChance()
    {
        tapChance = difficultyLevels[currentDifficulty].tapChance;
    }
    public int[] mSpeed;
    private void difficulty_Switch_speedModifier()
    {
        moveSpeed = difficultyLevels[currentDifficulty].movementSpeed;
    }
    public float[] rRate;
    private void difficulty_Switch_refreshRate()
    {
        refreshRate = difficultyLevels[currentDifficulty].refreshRate;
    }
    public int[] nToNext;
    private void difficulty_Switch_notesToNextDiff()        //Change this- difficulty needs to be determined by Current Path
    {
        notesToNextStage = difficultyLevels[currentDifficulty].notesToNextStage;
        SpawnerScript.notesCompletedThisStage = 0;

    }

    //public void difficulty_Sections()
    //{
    //    Debug.Log("currentDifficulty is set to " + currentDifficulty);

    //    switch (currentDifficulty)
    //    {
    //        case 0:                     //test difficulty
    //            //sec1
    //            SpawnerScript.secInUseBools[0] = true;
    //            SpawnerScript.secInUseBools_Held[0] = true;
    //            //SpawnerScript.secInUseBools[3] = true;

    //            //changeDifficulty();
    //            //SpawnerScript.includeSections2();

    //            break;
    //        case 1:
    //            SpawnerScript.secInUseBools_Held[0] = false;
    //            SpawnerScript.secInUseBools[0] = true;
    //            SpawnerScript.secInUseBools[1] = true;
    //            SpawnerScript.secInUseBools[3] = false;
    //            SpawnerScript.secInUseBools_Held[3] = true;

    //            //SpawnerScript.clearNotesInUse();

    //            //sec 1 and 2

    //            //SpawnerScript.includeSection(0);
    //            //SpawnerScript.includeSection(1);
    //            break;
    //        case 2:
    //            SpawnerScript.secInUseBools[0] = true;
    //            SpawnerScript.secInUseBools[1] = true;
    //            SpawnerScript.secInUseBools[2] = true;
    //            SpawnerScript.secInUseBools[3] = true;
    //            SpawnerScript.secInUseBools[4] = true;
    //            SpawnerScript.secInUseBools_Held[0] = true;
    //            SpawnerScript.secInUseBools_Held[1] = true;
    //            SpawnerScript.secInUseBools_Held[2] = true;
    //            SpawnerScript.secInUseBools_Held[3] = true;
    //            SpawnerScript.secInUseBools_Held[4] = true;



    //            //SpawnerScript.clearNotesInUse();

    //            //sec 2 and 3
    //            //SpawnerScript.includeSection(1);
    //            //SpawnerScript.includeSection(2);

    //            break;
    //        case 3:
    //            SpawnerScript.secInUseBools[0] = true;
    //            SpawnerScript.secInUseBools[1] = true;
    //            SpawnerScript.secInUseBools[2] = true;
    //            SpawnerScript.secInUseBools[3] = true;
    //            SpawnerScript.secInUseBools[4] = true;
    //            SpawnerScript.secInUseBools_Held[0] = true;
    //            SpawnerScript.secInUseBools_Held[1] = true;
    //            SpawnerScript.secInUseBools_Held[2] = true;
    //            SpawnerScript.secInUseBools_Held[3] = true;
    //            SpawnerScript.secInUseBools_Held[4] = true;
    //            //SpawnerScript.clearNotesInUse();

    //            //sec 3 and 4
    //            //SpawnerScript.includeSection(2);
    //            //SpawnerScript.includeSection(3);

    //            break;
    //        case 4:

    //            SpawnerScript.secInUseBools[2] = true;
    //            SpawnerScript.secInUseBools_Held[1] = true;
    //            SpawnerScript.secInUseBools[1] = true;
    //            //sec 4 and 3 and 2
    //            //SpawnerScript.includeSection(3);
    //            //SpawnerScript.includeSection(4);

    //            break;
    //        case 5:
    //            SpawnerScript.secInUseBools[0] = true;
    //            SpawnerScript.secInUseBools[1] = true;
    //            SpawnerScript.secInUseBools[2] = true;
    //            SpawnerScript.secInUseBools[3] = true;
    //            SpawnerScript.secInUseBools[4] = true;
    //            SpawnerScript.secInUseBools_Held[0] = true;
    //            SpawnerScript.secInUseBools_Held[1] = true;
    //            SpawnerScript.secInUseBools_Held[2] = true;
    //            SpawnerScript.secInUseBools_Held[3] = true;
    //            SpawnerScript.secInUseBools_Held[4] = true;
    //            //SpawnerScript.clearNotesInUse();

    //            //all sections
    //            //SpawnerScript.includeSection(4);


    //            break;
    //        case 6:
    //            //SpawnerScript.clearNotesInUse();

    //            //sec 3 and 2 and 1
    //            //SpawnerScript.includeSection(4);
    //            //SpawnerScript.includeSection(3);

    //            break;
    //        case 7:
    //            //SpawnerScript.clearNotesInUse();

    //            //2 and 1
    //            //SpawnerScript.includeSection(3);
    //            //SpawnerScript.includeSection(2);

    //            break;
    //        case 8:
    //            //SpawnerScript.clearNotesInUse();

    //            //all sections
    //            //SpawnerScript.includeSection(2);
    //            //SpawnerScript.includeSection(1);
    //           // SpawnerScript.includeSection(0);

    //            break;
    //        case 9:
    //            //SpawnerScript.clearNotesInUse();

    //            //all sections
    //            //SpawnerScript.includeSection(0);
    //            //SpawnerScript.includeSection(1);
    //            //SpawnerScript.includeSection(2);
    //            //SpawnerScript.includeSection(3);
    //            //SpawnerScript.includeSection(4);
    //            break;
    //        case 10:
    //            //SpawnerScript.clearNotesInUse();

    //            //all sections
    //            //for (int i = 0; i < 5; i++)             //for (initialization; condition; incrementation)
    //            //{
    //            //    SpawnerScript.includeSection(i);
    //            //}
    //            break;


    //    }
    //}

    public void setTo1()
    {
        currentDifficulty = 1;
    }

    //sec0 is tutorial





}
