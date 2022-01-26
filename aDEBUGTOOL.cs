using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NoteKey;
using UnityEngine.UI;

public class aDEBUGTOOL : MonoBehaviour
{
    public GameObject spawner;
    public GameObject player;
    private KeyInputSpawner noteSpawner;
    private DifficultyManger difficultyManager;
    public GameObject notesInUseInfo;
    public GameObject difficultyInfo;
    public GameObject keyboardInfo;
    private TMP_Text nIUItext;
    private TMP_Text[] dItext;
    private string[] diffText;
    public GameObject phaseInfoList;
    private changeGameModeSequence phaseManager;
    private TMP_Text[] pTextObjects;
    public GameObject mouseInfo;
    public GameObject healthInfo;
    void Start()
    {

        notesInUseInfo.SetActive(true);
        difficultyInfo.SetActive(true);
        keyboardInfo.SetActive(true);
        phaseInfoList.SetActive(true);

        noteSpawner = spawner.GetComponent<KeyInputSpawner>();
        nIUItext = notesInUseInfo.transform.GetChild(0).GetComponent<TMP_Text>();

        difficultyManager = spawner.GetComponent<DifficultyManger>();
        dItext = new TMP_Text[difficultyInfo.transform.childCount];
        diffText = new string[difficultyInfo.transform.childCount];
        for (int i = 0; i < 6; i++)
        {
            dItext[i] = difficultyInfo.transform.GetChild(i).GetComponent<TMP_Text>();
        }
        phaseManager = player.GetComponent<changeGameModeSequence>();
        pTextObjects = new TMP_Text[phaseInfoList.transform.childCount];
        for(int i = 0; i < phaseInfoList.transform.childCount; i++)
        {
            pTextObjects[i] = phaseInfoList.transform.GetChild(i).GetComponent<TMP_Text>();
        }


    }

    void Update()
    {
        nIUItext.SetText(inList());
        difficultyInfoSetStrings();
        keyGraphic();
        phaseTrackerHighlight();
        mouseGraphic();
        healthTracker();
    }

    string inList()             //Shows which input letters are in user
    {
        string notesInUseInfoText;
        int notesInUseCount; notesInUseCount = noteSpawner.NotesInUse.Count;
        notesInUseInfoText = notesInUseCount.ToString() + "\n";
        //string plsWork;
        //plsWork = noteSpawner.NotesInUse.Count.ToString() + "\n";
        int atItem; atItem = -1;

        foreach (noteKey item in noteSpawner.NotesInUse)
        {
            atItem++;
            string addThis;
            addThis = item.name.ToString();
            addThis = addThis + "    ";
            notesInUseInfoText = notesInUseInfoText + addThis;
        }


        return notesInUseInfoText;
    }

    void difficultyInfoSetStrings()
    {
        int someMath;


        someMath = difficultyManager.difficultyLevels[difficultyManager.currentDifficulty].notesToNextStage - noteSpawner.notesCompletedThisStage;
        diffText[0] = "Difficulty Level: " + difficultyManager.currentDifficulty.ToString();
        diffText[1] = "Notes needed to advance: " + someMath;
        diffText[2] = "Global Progress Modifier:" + difficultyManager.globalProgressModifier.ToString();
        diffText[3] = "Refresh Rate:" + difficultyManager.difficultyLevels[difficultyManager.currentDifficulty].refreshRate.ToString();
        diffText[4] = "Move Speed:" + difficultyManager.difficultyLevels[difficultyManager.currentDifficulty].movementSpeed.ToString();
        diffText[5] = "Hold::Tap-" + difficultyManager.difficultyLevels[difficultyManager.currentDifficulty].holdChance.ToString() + ":" + difficultyManager.difficultyLevels[difficultyManager.currentDifficulty].tapChance.ToString();

        for (int i = 0; i < dItext.Length; i++)
        {
            dItext[i].SetText(diffText[i]);
        }

    }

    void phaseTrackerHighlight()
    {
        for(int i = 0; i < pTextObjects.Length; i++)
        {
            if(i == phaseManager.gamePhase)
            {
                pTextObjects[i].color = Color.red;
            }
            else
            {
                pTextObjects[i].color = Color.black;
            }
        }
    }

    public void keyGraphic()
    {
        int atSec; atSec = -1;
        //for(int i = 0; i < 5)
        foreach (bool item in noteSpawner.addedSec)
        {
            atSec++;
            if (item)
            {
                keyboardInfo.transform.GetChild(atSec).gameObject.SetActive(true);
            }
            else
            {
                keyboardInfo.transform.GetChild(atSec).gameObject.SetActive(false);
            }
        }
    }



    void mouseGraphic()
    {
        Image mouse0, mouse1; GameObject mouseBoth;
        mouse0 = mouseInfo.transform.GetChild(1).GetComponent<Image>();
        mouse1 = mouseInfo.transform.GetChild(2).GetComponent<Image>();
        mouseBoth = mouseInfo.transform.GetChild(0).gameObject;

        if (noteSpawner.mouse0Pressed)
        {
            mouse0.color = Color.red;
        }
        else
        {
            mouse0.color = Color.white;
        }

        if (noteSpawner.mouse1Pressed)
        {
            mouse1.color = Color.red;
        }
        else
        {
            mouse1.color = Color.white;
        }

        if (noteSpawner.startSpawn)
        {
            mouseBoth.SetActive(true);
        }
        else
        {
            mouseBoth.SetActive(false);
        }
    }

    void healthTracker()
    {
        healthInfo.transform.GetChild(0).GetComponent<TMP_Text>().SetText(player.GetComponent<Path_MovePlayer>().modifiedWalkSpeed.ToString());
    }

}
