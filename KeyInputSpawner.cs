using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoteKey;
using TMPro;
using InputColumns;

public class KeyInputSpawner : MonoBehaviour
{
    public GameObject KeySquarePrefab;
    public GameObject Player;
    private changeGameModeSequence phaseManager;
    public bool startSpawn;
    public bool mouse0Pressed;
    public bool mouse1Pressed;
    public bool mouseHeldDown;

    public bool debugMode; 

    public int currentKeyInputsOnScreen = 0;
    public int maxKeyInputsOnScreen = 5;
    public float spawnMoveSpeed;
    public float progNeeded;

    //private float timeBeforeCallsBegin = 1; //This should be the amount of time it takes for player to turn around.
    public List<noteKey> keyInputblocks;

    public InputColumn[] fingerInputColumns;
    public bool noColumnsFree;
    public int columnsFreeCount;

    private DifficultyManger Difficulty;
    public int notesCompletedThisStage;                  //How many notes have been completed during this difficulty level

    private List<noteKey> section0, section1, section2, section3, section4;
    public List<noteKey> NotesInUse;
    public bool[] secInUseBools, addedSec, secInUseBools_Held;
    private List<noteKey>[] selectionCollection;
    public List<noteKey> NotesInUse_Held;
    public bool heldNoteOnScreen;


    public bool finalizeSelection;
    public bool activeInPhase;

    public bool inSpawn;

    int atNum;
    void Start()
    {
        Player = GameObject.Find("Player");
        Difficulty = GetComponent<DifficultyManger>();

        atNum = 0;
        //notesCompletedThisStage = 0;
        sortKeyInputSections();
        setUpBoard();

        //phaseManager = Player.GetComponent<changeGameModeSequence>();
        Difficulty.changeDifficulty();
        includeSections2();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))    //Return all notes in use
        {
            for(int i = 0; i < NotesInUse.Count; i++)
            {
                print(NotesInUse[i].name + NotesInUse[i].placementSection);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))      //Increase difficulty
        {
            if (Difficulty.currentDifficulty < 11)
            {
                Difficulty.currentDifficulty++;
            }
            else
            {
                Difficulty.currentDifficulty = 1;
            }
        }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                mouse0Pressed = true;
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                mouse1Pressed = true;
            }

            if (mouse0Pressed & mouse1Pressed)
            {
                mouseHeldDown = true;
            }
            else
            {
                mouseHeldDown = false;
            }

            if (mouseHeldDown)
            {
                //moved to startSpawnFunction
                    


            }

        //Debug.Log(NotesInUse.Count);
        
        //manualCall();
    }

    public void startSpawnfunction()
    {
        if (!startSpawn)
        {
            //if (Difficulty.difficulty < 1)
            //{
            //    Difficulty.setTo1();
            //}
            //transform.GetChild(0).gameObject.SetActive(true);       //Turns on the Key Input Interface
            //Difficulty.difficulty_Sections();


            StartCoroutine(SpawnKeyCalls());
            startSpawn = true;
            print("STARTING SPAWN");
        }
    }

    void manualCall()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Difficulty.difficultyLevelChange();
            //Difficulty.difficulty_Sections();

            //includeSections2();
            if (currentKeyInputsOnScreen < maxKeyInputsOnScreen)
            {
                

                InstantiateKeyCall();
                InstantiateKeyCall();
                InstantiateKeyCall();
                InstantiateKeyCall();
                InstantiateKeyCall();

                Debug.Log("manual call");
            }
            else
            {
                Debug.LogWarning("Failed to manual call, too many keys on screen already");
            }
        }
    }

    private void LateUpdate()
    {                                       //Mouse button detection
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            mouse0Pressed = false;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            mouse1Pressed = false;
        }
        //if (!mouseHeldDown)
        //{
        //    if (startSpawn)
        //    {
        //        stopSpawning();
        //    }
        //}
    }

    public void stopSpawning()
    {
        startSpawn = false;
        //inSpawn = false;
        StopAllCoroutines();
        Debug.Log("Mouse released. Stopping loop.");
        clearKeys(); //resets the level- removes key inputs on screen and sets currentKeyInputsOnScreen to 0
    }

    public IEnumerator SpawnKeyCalls()
    {
            while (currentKeyInputsOnScreen <= maxKeyInputsOnScreen)
            {
                if (!debugMode)
                {
                    Debug.Log("CALLING NEW KEY");
                    InstantiateKeyCall();
                }
                else
                {
                InstantiateKeyCall();
                    atNum++;
                }
                

                yield return new WaitForSecondsRealtime(Difficulty.refreshRate);
            }
            if (currentKeyInputsOnScreen > maxKeyInputsOnScreen)
            {
                Debug.LogError("current key inputs exeeds max limit");
            }
            yield return null;

    }

    private GameObject spawnThis; private TMP_Text spawnThisText;
   

    void InstantiateKeyCall()
    {
        //Difficulty.changeDifficulty();

        Debug.Log("Called");
        spawnMoveSpeed = Difficulty.difficultyLevels[Difficulty.currentDifficulty].movementSpeed;       

        if (!debugMode)
        {
            spawnKey();
        }
        else
        {
            debugSpawnKey(atNum);
        }
    }

    void spawnKey()
    {
        noteKey chosenNoteKey;
        int chosenNoteKeyID;

        chosenNoteKeyID = setNoteObject();

        //Debug.Log(chosenNoteKeyID);

        chosenNoteKey = NotesInUse[chosenNoteKeyID];


        currentKeyInputsOnScreen++;
        spawnThis = Instantiate(KeySquarePrefab, transform.GetChild(0));            //Instantiates a KeySquarePrefab
        spawnThis.transform.localPosition = spawnLocationRange();                   //Moves the object to the spawnBox
        spawnThisText = spawnThis.GetComponentInChildren<TMP_Text>();               //Sets spawnThisText as the text object parented to the KeySquarePrefab
        spawnThisText.SetText(chosenNoteKey.name);                                  //Sets the text object parented to the KeySquarePrefab to whatever was 
        spawnThis.name = spawnThisText.text;                                        //Changes the KeySquarePrefab object's name to it's required input
        spawnThis.GetComponent<KeyInputBehavior>().keyID = chosenNoteKeyID;
        if (isThisNoteHeldOrTapped(chosenNoteKey))                                               //Determines whether the input requires either a 'hold' or 'tap'
        {
            spawnThis.GetComponent<KeyInputBehavior>().tapKey = true;
            spawnThisText.color = Color.yellow;
        }
        spawnThis.GetComponent<KeyInputBehavior>().progressModifier = Difficulty.globalProgressModifier;

    }

    void debugSpawnKey(int atNum)
    {
        Debug.Log("Called");
        spawnMoveSpeed = Difficulty.difficultyLevels[Difficulty.currentDifficulty].movementSpeed;

        //Difficulty.difficulty_Sections();

        noteKey chosenNoteKey;
        chosenNoteKey = NotesInUse[atNum];

        currentKeyInputsOnScreen++;
        spawnThis = Instantiate(KeySquarePrefab, transform.GetChild(0));            //Instantiates a KeySquarePrefab
        spawnThis.transform.localPosition = spawnLocationRange();                   //Moves the object to the spawnBox
        spawnThisText = spawnThis.GetComponentInChildren<TMP_Text>();               //Sets spawnThisText as the text object parented to the KeySquarePrefab
        spawnThisText.SetText(chosenNoteKey.name);                                  //Sets the text object parented to the KeySquarePrefab to whatever was 
        spawnThis.name = spawnThisText.text;                                        //Changes the KeySquarePrefab object's name to it's required input
        //chosenNoteKey.isOnScreen = true;                                            //Marks this key input as being on screen, so the spawner won't make another key of the same input.
        if (isThisNoteHeldOrTapped(chosenNoteKey))                                               //Determines whether the input requires either a 'hold' or 'tap'
        {
            spawnThis.GetComponent<KeyInputBehavior>().tapKey = true;
            spawnThisText.color = Color.yellow;
        }
        
        spawnThis.GetComponent<KeyInputBehavior>().progressModifier = Difficulty.globalProgressModifier;

        spawnThis.GetComponent<KeyInputBehavior>().keyID = atNum;
    }


    int setNoteObject() //Picks a key input from the list.                          //maxNum is based on current NotesInUse, which is based on difficulty.
    {
        //Difficulty.difficulty_Sections();

        int maxNum; maxNum = NotesInUse.Count - 1;
        bool searching; searching = true;
        int keyObjectID;
        keyObjectID = Random.Range(0, maxNum);
        while (searching)
        {
            if (!keyInputblocks[keyObjectID].isOnScreen)                                 //If a note is already on screen, this code will choose a different one from the list
            {
                searching = false;
                keyInputblocks[keyObjectID].isOnScreen = true;                                            //Marks this key input as being on screen, so the spawner won't make another key of the same input.

            }
            else
            {
                if(keyObjectID < maxNum)
                {
                    keyObjectID++;
                }
                else if(keyObjectID >= maxNum)
                {
                    keyObjectID = 0;
                }
            }
        }
        Debug.Log(keyObjectID);
        return keyObjectID;
    }

    public bool isThisNoteHeldOrTapped(noteKey chosenNoteKey)        //Determines if this Note needs to be held or tapped
    {
        bool tappedNote;
        if (heldNoteOnScreen)
        {
            tappedNote = true;
        }
        else
        {
            if (NotesInUse_Held.Contains(chosenNoteKey))
            {
                tappedNote = false;
                heldNoteOnScreen = true;
            }
            else
            {
                tappedNote = true;
            }
        }
        
        //int num = Random.Range(1, 10);
        //if (num <= Difficulty.tapChance)
        //{
        //    tappedNote = true;
        //}
        //else
        //{
        //    tappedNote = false;
        //}
        return tappedNote;
    }

    public void clearNotesInUse()
    {
        //NotesInUse.Clear();
        int item = -1;
        foreach (bool entry in secInUseBools)
        {
            item++;
            secInUseBools[item] = false;
        }
        for(int i = 0; i < secInUseBools_Held.Length; i++)
        {
            secInUseBools_Held[i] = false;
        }
    }
    int thisone;

    string thisSection;
    public void includeSections2()
    {
        //Difficulty.difficultyLevelChange();
        for(int a = 0; a < 5; a++)
        {
            thisSection = "Section " + a;
            if (secInUseBools[a])
            {
                if (secInUseBools_Held[a])
                {
                    if (!addedSec[a])
                    {
                        Debug.Log("added section " + a + " to NotesInUse");
                        NotesInUse.AddRange(selectionCollection[a]);        //This adds notes to the NotesInUse list.
                        NotesInUse_Held.AddRange(selectionCollection[a]);   //This adds notes to the NotesInUse_Held list.
                        addedSec[a] = true;
                    }
                }
                else
                {
                    if (!addedSec[a])
                    {
                        Debug.Log("added section " + a + " to NotesInUse");
                        NotesInUse.AddRange(selectionCollection[a]);        //This adds notes to the NotesInUse list.
                        addedSec[a] = true;
                    }
                }
                
            }
            else
            {
                if (addedSec[a])
                {
                    addedSec[a] = false;
                    for (int i = 0; i < NotesInUse.Count; i++)
                    {
                        if (NotesInUse[i].placementSection == thisSection)
                        {
                            NotesInUse.Remove(NotesInUse[i]);
                        }
                    }
                    for (int i = 0; i < NotesInUse_Held.Count; i++)
                    {
                        if (NotesInUse_Held[i].placementSection == thisSection)
                        {
                            NotesInUse_Held.Remove(NotesInUse_Held[i]);
                        }
                    }
                }
                
            }
        }
        
    }

   
    public void includeSection(int thisOne)
    {
        secInUseBools[thisOne] = true;

        switch (thisOne)
        {
            case 0:
                NotesInUse.AddRange(section0);
                break;
            case 1:
                NotesInUse.AddRange(section1);
                break;
            case 2:
                NotesInUse.AddRange(section2);
                break;
            case 3:
                NotesInUse.AddRange(section3);
                break;
            case 4:
                NotesInUse.AddRange(section4);
                break;
        }

    }

    private int atListItem;
    public void sortKeyInputSections() //This runs at start and sorts all KeyNote Objects into their sections, that are set via string, based on keyboard placement.
    {
        atListItem = 0;
        //Debug.Log("atListItem is at " + atListItem);

        section0 = new List<noteKey>();
        section1 = new List<noteKey>();
        section2 = new List<noteKey>();
        section3 = new List<noteKey>();
        section4 = new List<noteKey>();

        selectionCollection = new List<noteKey>[5];
        selectionCollection[0] = section0;
        selectionCollection[1] = section1;
        selectionCollection[2] = section2;
        selectionCollection[3] = section3;
        selectionCollection[4] = section4;

        secInUseBools = new bool[5];
        secInUseBools[0] = false; secInUseBools[1] = false; secInUseBools[2] = false; secInUseBools[3] = false; secInUseBools[4] = false;

        addedSec = new bool[5];
        addedSec[0] = false; addedSec[1] = false; addedSec[2] = false; addedSec[3] = false; addedSec[4] = false;

        secInUseBools_Held = new bool[5];
        secInUseBools_Held[0] = false; secInUseBools_Held[1] = false; secInUseBools_Held[2] = false; secInUseBools_Held[3] = false; secInUseBools_Held[4] = false;
        NotesInUse_Held = new List<noteKey>();     //heldSelection is another list that will hold a section whose keys will ALWAYS spawn in as held. During spawn, the tapChance just compares the chosen key input to the heldSelection list. If it's not on there, then it's a tap.


        foreach (noteKey item in keyInputblocks)
        {

            switch (keyInputblocks[atListItem].placementSection)
            {
                case "Section 0":
                    section0.Add(keyInputblocks[atListItem]);
                    atListItem++;
                    break;
                case "Section 1":
                    //Debug.Log(item.requiredInput + " has been placed");
                    section1.Add(keyInputblocks[atListItem]);
                    atListItem++;

                    break;
                case "Section 2":
                    //Debug.Log(keyInputblocks[atListItem].requiredInput + " has been placed");

                    section2.Add(keyInputblocks[atListItem]);
                    atListItem++;

                    break;
                case "Section 3":
                    //Debug.Log(keyInputblocks[atListItem].requiredInput + " has been placed");

                    section3.Add(keyInputblocks[atListItem]);
                    atListItem++;

                    break;
                case "Section 4":
                    //Debug.Log(keyInputblocks[atListItem].requiredInput + " has been placed");

                    section4.Add(keyInputblocks[atListItem]);
                    atListItem++;

                    break;
            }
            if(atListItem >= keyInputblocks.Count)
            {
                Debug.Log("Notes have been sorted");
                //print("section1 contains ");

                //foreach (noteKey item2 in section1)
                //{
                //    print(item2.requiredInput);
                //}
            }

        }
        //Difficulty.changeDifficulty();

    }

    void setUpBoard()
    {
        columnsFreeCount = 0;

        for (int i = 0;i < fingerInputColumns.Length;i++)
        {
            columnsFreeCount++;

            fingerInputColumns[i].isFree = true;
            //Debug.Log("Column " + currentItem + " is free");
        }

    }

    Vector3 spawnLocationRange() //Spawn Box location
    {
        Vector3 spawnLoc;
        int spawnX, spawnY, spawnZ;

        spawnX = 0;
        spawnY = -580;
        spawnZ = 0;

        spawnLoc.x = spawnX; spawnLoc.y = spawnY; spawnLoc.z = spawnZ;
        return spawnLoc;
    }

    void clearKeys()
    {
        print("CLEARING ALL KEYS");
        currentKeyInputsOnScreen = 0;
        foreach(InputColumn item in fingerInputColumns)
        {
            item.isFree = true;
        }

        Transform canvas = transform.GetChild(0);
        foreach (Transform child in canvas)
        {
            if (child.gameObject.CompareTag("keyInputObject") & child.gameObject.activeInHierarchy)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void startEndPhase()
    {
        phaseManager.gamePhase = 7;
    }
}
