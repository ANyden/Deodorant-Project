using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DifficultyManger : MonoBehaviour
{
    [Range(0, 10)]
    public int difficulty;                  //Difficulty stage
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

    private KeyInputSpawner SpawnerScript;



    void Start()
    {
        SpawnerScript = GetComponent<KeyInputSpawner>();
        
    }

    void Update()
    {
        holdChance = 10 - tapChance;
    }

    private void LateUpdate()
    {
        difficultyLevelChange();
        difficulty_Switch_tapChance();
        difficulty_Switch_speedModifier();
        difficulty_Switch_refreshRate();
        difficulty_Switch_notesToNextDiff();
    }

    private void difficultyLevelChange()
    {
        if (SpawnerScript.notesCompletedThisStage >= notesToNextStage)
        {
            if(difficulty < 10)
            {
                difficulty++;
                SpawnerScript.notesCompletedThisStage = 0;
            }
            else
            {
                print("WIN");
                SpawnerScript.StopAllCoroutines();
            }
            
        }
    }
    public int[] tChance;
    private void difficulty_Switch_tapChance()
    {
        tapChance = tChance[difficulty];
    }
    public int[] mSpeed;
    private void difficulty_Switch_speedModifier()
    {
        moveSpeed = mSpeed[difficulty];
    }
    public float[] rRate;
    private void difficulty_Switch_refreshRate()
    {
        refreshRate = rRate[difficulty];
    }
    public int[] nToNext;
    private void difficulty_Switch_notesToNextDiff()
    {
        notesToNextStage = nToNext[difficulty];
    }

    public void difficulty_Sections()
    {
        
        switch (difficulty)
        {
            case 0:                     //tutorial
                SpawnerScript.clearNotesInUse();
                //sec1
                SpawnerScript.includeSection(0);
                break;
            case 1:
                SpawnerScript.clearNotesInUse();

                //sec 1 and 2

                SpawnerScript.includeSection(0);
                SpawnerScript.includeSection(1);
                break;
            case 2:
                SpawnerScript.clearNotesInUse();

                //sec 2 and 3
                SpawnerScript.includeSection(1);
                SpawnerScript.includeSection(2);

                break;
            case 3:
                SpawnerScript.clearNotesInUse();

                //sec 3 and 4
                SpawnerScript.includeSection(2);
                SpawnerScript.includeSection(3);

                break;
            case 4:
                SpawnerScript.clearNotesInUse();

                //sec 4 and 3 and 2
                SpawnerScript.includeSection(3);
                SpawnerScript.includeSection(4);

                break;
            case 5:
                SpawnerScript.clearNotesInUse();

                //all sections
                SpawnerScript.includeSection(4);


                break;
            case 6:
                SpawnerScript.clearNotesInUse();

                //sec 3 and 2 and 1
                SpawnerScript.includeSection(4);
                SpawnerScript.includeSection(3);

                break;
            case 7:
                SpawnerScript.clearNotesInUse();

                //2 and 1
                SpawnerScript.includeSection(3);
                SpawnerScript.includeSection(2);

                break;
            case 8:
                SpawnerScript.clearNotesInUse();

                //all sections
                SpawnerScript.includeSection(2);
                SpawnerScript.includeSection(1);
                SpawnerScript.includeSection(0);

                break;
            case 9:
                SpawnerScript.clearNotesInUse();

                //all sections
                SpawnerScript.includeSection(0);
                SpawnerScript.includeSection(1);
                SpawnerScript.includeSection(2);
                SpawnerScript.includeSection(3);
                break;
            case 10:
                SpawnerScript.clearNotesInUse();

                //all sections
                for (int i = 0; i < 5; i++)             //for (initialization; condition; incrementation)
                {
                    SpawnerScript.includeSection(i);
                }
                break;


        }
    }

    //sec0 is tutorial





}
