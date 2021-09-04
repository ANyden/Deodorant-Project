using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NoteKey;

public class aDEBUGTOOL : MonoBehaviour
{
    public GameObject spawner;
    private KeyInputSpawner noteSpawner;
    private DifficultyManger difficultyManager;
    public GameObject notesInUseInfo;
    public GameObject difficultyInfo;
    public GameObject keyboardInfo;
    private TMP_Text nIUItext, dItext;

    void Start()
    {
        notesInUseInfo.SetActive(true);
        difficultyInfo.SetActive(true);
        keyboardInfo.SetActive(true);

        noteSpawner = spawner.GetComponent<KeyInputSpawner>();
        difficultyManager = spawner.GetComponent<DifficultyManger>();
        nIUItext = notesInUseInfo.transform.GetChild(0).GetComponent<TMP_Text>();
        dItext = difficultyInfo.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    void Update()
    {
        nIUItext.SetText(inList());
        dItext.SetText(difficultyInfoText());
        keyGraphic();
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

    string difficultyInfoText()
    {
        string diffText;
        int someMath;


        someMath = difficultyManager.notesToNextStage - noteSpawner.notesCompletedThisStage;
        diffText =  "Difficulty Level: " + difficultyManager.difficulty.ToString() + "\n" + "Notes needed to advance: " + someMath;
     

        return diffText;
    }

    public void keyGraphic()
    {
        int atSec; atSec = -1;
        //for(int i = 0; i < 5)
        foreach (bool item in noteSpawner.secInUseBools)
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
}
