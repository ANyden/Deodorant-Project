using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DifficultyManger : MonoBehaviour
{
    [Range(0, 10)]
    public int difficulty;                  //Difficulty stage
    public float globalProgressModifier;    //How fast the keyNote disappears when held
    public float refreshRate = 3; //How fast new button prompts appear
    public float moveSpeed = 200;
    public float moveSpeedModifier;         //Added onto global speed
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
    }

    private void difficultyLevelChange()
    {
        if (SpawnerScript.notesCompletedThisStage >= notesToNextStage)
        {
            difficulty++;
            SpawnerScript.notesCompletedThisStage = 0;
        }
    }

    private void difficulty_Switch_tapChance()
    {
        switch (difficulty)
        {
            case 0:
                break;
        }
    }

    private void difficulty_Switch_speedModifier()
    {
        switch (difficulty)
        {
            case 0:
                break;
        }
    }

    private void difficulty_Switch_refreshRate()
    {
        switch (difficulty)
        {
            case 0:
                break;
        }
    }







}
