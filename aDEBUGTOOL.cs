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
    private TMP_Text nIUItext, dItext;

    void Start()
    {
        notesInUseInfo.SetActive(true);
        difficultyInfo.SetActive(true);

        noteSpawner = spawner.GetComponent<KeyInputSpawner>();
        difficultyManager = spawner.GetComponent<DifficultyManger>();
        nIUItext = notesInUseInfo.transform.GetChild(0).GetComponent<TMP_Text>();
        dItext = difficultyInfo.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    void Update()
    {
        nIUItext.SetText(inList());
        dItext.SetText(difficultyInfoText());
    }

    string inList()
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
        diffText = "Difficulty Level: " + difficultyManager.difficulty.ToString() + "\n" + "Notes needed to advance: " + someMath;
     

        return diffText;
    }
}
