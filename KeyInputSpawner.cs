using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoteKey;
using TMPro;
using InputColumns;
using DifficultyLevel;

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
    //public List<noteKey> keyInputblocks;
    public NotesCollection NotesList;

    public InputColumn[] fingerInputColumns;
    public bool noColumnsFree;
    public int columnsFreeCount;

    //private DifficultyManger Difficulty;
    private Path_MovePlayer playerPath;
    //private dLev difficulty;
    public int notesCompletedThisStage;                  //How many notes have been completed during this difficulty level

    public List<noteKey> section0, section1, section2, section3, section4;
    public List<noteKey> NotesInUse;
    public bool[] addedSec;
    //private List<noteKey>[] selectionCollection;
    public List<noteKey> NotesInUse_Held;
    public bool heldNoteOnScreen;


    public bool finalizeSelection;
    public bool activeInPhase;

    public bool inSpawn;

    int atNum;

    private void Awake()
    {
        Player = GameObject.Find("Player");
        playerPath = Player.GetComponent<Path_MovePlayer>();
        //difficulty = playerPath.currentPath.difficulty;
        NotesList = transform.GetComponent<NotesCollection>();

        atNum = 0;
        setUpBoard();

        //notesCompletedThisStage = 0;
        sortKeyInputSections();

        //phaseManager = Player.GetComponent<changeGameModeSequence>();
        //Difficulty.changeDifficulty();
        //includeSections2();
        //includeSections3();
    }
    void Start()
    {
        
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
        if (Input.GetKeyDown(KeyCode.LeftControl))      //SHOULD increase current Path Num and move player forward 1 path. NEEDS TESTING.
        {
            if(playerPath.currentPathNum < playerPath.path.Length)
            {
                playerPath.currentPath.b_reached = true;
                playerPath.currentPath.allTurnsComplete = true;

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
                

                yield return new WaitForSecondsRealtime(playerPath.currentPath.difficulty.refreshRate);
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
        spawnMoveSpeed = playerPath.currentPath.difficulty.movementSpeed;     

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
        spawnThis.GetComponent<KeyInputBehavior>().progressModifier = -.2f;            //This is the rate that Held Keys lose progress.
    }

    void debugSpawnKey(int atNum)
    {
        Debug.Log("Called");

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

        spawnThis.GetComponent<KeyInputBehavior>().progressModifier = -2f;

        spawnThis.GetComponent<KeyInputBehavior>().keyID = atNum;
    }


    int setNoteObject() //Picks a key input from the list.                          //maxNum is based on current NotesInUse, which is based on difficulty.
    {
        //Difficulty.difficulty_Sections();

        int maxNum; maxNum = NotesInUse.Count - 1;
        bool searching; searching = true;
        int keyObjectID;
        keyObjectID = Random.Range(0, maxNum);
        string keyObjectName; keyObjectName = NotesInUse[keyObjectID].name;
        while (searching)
        {
            if (!NotesList.keysMasterList[keyObjectID].isOnScreen)                                //If a note is already on screen, this code will choose a different one from the list
            {

                searching = false;
                NotesList.keysMasterList[keyObjectID].isOnScreen = true;                                            //Marks this key input as being on screen, so the spawner won't make another key of the same input.
            }
            else
            {                                                                       //If the key is on screen, choose the next key in the NotesInUse list
                if(keyObjectID < maxNum)
                {
                    keyObjectID++;
                }
                else if(keyObjectID >= maxNum)                                      //If the key is on screen, and the current key in the NotesInUse list is last, then the key chosen becomes the first available in the NotesInUse
                {
                    keyObjectID = 0;
                }
            }
        }
        //Debug.Log(keyObjectID);
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

    //public void clearNotesInUse()
    //{
    //    //NotesInUse.Clear();
    //    int item = -1;
    //    foreach (bool entry in secInUseBools)
    //    {
    //        item++;
    //        secInUseBools[item] = false;
    //    }
    //    for(int i = 0; i < secInUseBools_Held.Length; i++)
    //    {
    //        secInUseBools_Held[i] = false;
    //    }
    //}
    int thisone;

    string thisSection;
    //public void includeSections2()
    //{
    //    //Difficulty.difficultyLevelChange();
    //    for(int a = 0; a < 5; a++)
    //    {
    //        thisSection = "Section " + a;
    //        if (secInUseBools[a])
    //        {
    //            if (secInUseBools_Held[a])
    //            {
    //                if (!addedSec[a])
    //                {
    //                    Debug.Log("added section " + a + " to NotesInUse");
    //                    NotesInUse.AddRange(selectionCollection[a]);        //This adds notes to the NotesInUse list.
    //                    NotesInUse_Held.AddRange(selectionCollection[a]);   //This adds notes to the NotesInUse_Held list.
    //                    addedSec[a] = true;
    //                }
    //            }
    //            else
    //            {
    //                if (!addedSec[a])
    //                {
    //                    Debug.Log("added section " + a + " to NotesInUse");
    //                    NotesInUse.AddRange(selectionCollection[a]);        //This adds notes to the NotesInUse list.
    //                    addedSec[a] = true;
    //                }
    //            }
                
    //        }
    //        else
    //        {
    //            if (addedSec[a])
    //            {
    //                addedSec[a] = false;
    //                for (int i = 0; i < NotesInUse.Count; i++)
    //                {
    //                    if (NotesInUse[i].placementSection == thisSection)
    //                    {
    //                        NotesInUse.Remove(NotesInUse[i]);
    //                    }
    //                }
    //                for (int i = 0; i < NotesInUse_Held.Count; i++)
    //                {
    //                    if (NotesInUse_Held[i].placementSection == thisSection)
    //                    {
    //                        NotesInUse_Held.Remove(NotesInUse_Held[i]);
    //                    }
    //                }
    //            }
                
    //        }
    //    }
        
    //}

    public void includeSections3()
    {

        for(int i = 0; i < 5; i++)
        {
            if (playerPath.path[playerPath.currentPathNum].difficulty.sectionsInUseFlag[i])            //if the flags are true...
            {
                if (!addedSec[i])                           //and the sections have not been added...
                {
                    Debug.Log("adding sectionBlock " + i);
                    NotesInUse.AddRange(sectionBlock[i]);   //...add them to the in use list.
                    NotesInUse_Held.AddRange(sectionBlock[i]);
                    addedSec[i] = true;
                }
            }
            else
            {                                               //if the flags are false...
                if (addedSec[i])
                {
                    foreach (noteKey item in NotesInUse)
                    {
                        if(item.placementSection == i)
                        {
                            NotesInUse.Remove(item);
                            NotesInUse_Held.Remove(item);
                        }
                    }
                    addedSec[i] = false;
                }
            }
        }
    }


    //public void includeSection(int thisOne)
    //{
    //    secInUseBools[thisOne] = true;

    //    switch (thisOne)
    //    {
    //        case 0:
    //            NotesInUse.AddRange(section0);
    //            break;
    //        case 1:
    //            NotesInUse.AddRange(section1);
    //            break;
    //        case 2:
    //            NotesInUse.AddRange(section2);
    //            break;
    //        case 3:
    //            NotesInUse.AddRange(section3);
    //            break;
    //        case 4:
    //            NotesInUse.AddRange(section4);
    //            break;
    //    }

    //}

    //private int atListItem;

    public List<noteKey>[] sectionBlock;
    public void sortKeyInputSections() //This runs at start and sorts all KeyNote Objects into their sections, that are set via string, based on keyboard placement.
    {
        Debug.Log("Sorting Keys...");
        //atListItem = 0;
        //Debug.Log("atListItem is at " + atListItem);

        section0 = new List<noteKey>();
        section1 = new List<noteKey>();
        section2 = new List<noteKey>();
        section3 = new List<noteKey>();
        section4 = new List<noteKey>();

        //selectionCollection = new List<noteKey>[5];
        //selectionCollection[0] = section0;
        //selectionCollection[1] = section1;
        //selectionCollection[2] = section2;
        //selectionCollection[3] = section3;
        //selectionCollection[4] = section4;

        //secInUseBools = new bool[5];
        //secInUseBools[0] = false; secInUseBools[1] = false; secInUseBools[2] = false; secInUseBools[3] = false; secInUseBools[4] = false;

        addedSec = new bool[5];
        addedSec[0] = false; addedSec[1] = false; addedSec[2] = false; addedSec[3] = false; addedSec[4] = false;

        //secInUseBools_Held = new bool[5];
        //secInUseBools_Held[0] = false; secInUseBools_Held[1] = false; secInUseBools_Held[2] = false; secInUseBools_Held[3] = false; secInUseBools_Held[4] = false;
        NotesInUse_Held = new List<noteKey>();     //heldSelection is another list that will hold a section whose keys will ALWAYS spawn in as held. During spawn, the tapChance just compares the chosen key input to the heldSelection list. If it's not on there, then it's a tap.

        sectionBlock = new List<noteKey>[5];
        for (int i = 0; i < NotesList.keysMasterList.Count; i++)
        {
            switch (NotesList.keysMasterList[i].placementSection)
            {
                case 0:
                    section0.Add(NotesList.keysMasterList[i]);
                    //Debug.Log(NotesList.keysMasterList[i].name + " Has been added to section0");
                    break;
                case 1:
                    section1.Add(NotesList.keysMasterList[i]);
                    //Debug.Log(NotesList.keysMasterList[i].name + " Has been added to section0");

                    break;
                case 2:
                    section2.Add(NotesList.keysMasterList[i]);
                    //Debug.Log(NotesList.keysMasterList[i].name + " Has been added to section0");

                    break;
                case 3:
                    section3.Add(NotesList.keysMasterList[i]);
                    //Debug.Log(NotesList.keysMasterList[i].name + " Has been added to section0");

                    break;
                case 4:
                    section4.Add(NotesList.keysMasterList[i]);
                    //Debug.Log(NotesList.keysMasterList[i].name + " Has been added to section0");

                    break;
            }
        }
        //switch (NotesList.gameModeSetTo)
        //{
        //    case NotesCollection.gameMode.alpha:                    
        //        for (int i = 0; i < NotesList.keysMasterList.Count; i++)
        //        {
        //            switch (NotesList.keysMasterList[i].placementSection)
        //            {
        //                case 0:
        //                    section0.Add(NotesList.keysMasterList[i]);
        //                    break;
        //                case 1:
        //                    section1.Add(NotesList.keysMasterList[i]);
        //                    break;
        //                case 2:
        //                    section2.Add(NotesList.keysMasterList[i]);
        //                    break;
        //                case 3:
        //                    section3.Add(NotesList.keysMasterList[i]);
        //                    break;
        //                case 4:
        //                    section4.Add(NotesList.keysMasterList[i]);
        //                    break;
        //            }
        //        }
        //        break;
        //    case NotesCollection.gameMode.numeric:                  
        //        for (int i = 0; i < NotesList.keysMasterList.Count + 1; i++)
        //        {
        //            switch (NotesList.keysMasterList[i].placementSection)
        //            {
        //                case 0:
        //                    section0.Add(NotesList.keysMasterList[i]);
        //                    break;
        //                case 1:
        //                    section1.Add(NotesList.keysMasterList[i]);
        //                    break;
        //                case 2:
        //                    section2.Add(NotesList.keysMasterList[i]);
        //                    break;
        //                case 3:
        //                    section3.Add(NotesList.keysMasterList[i]);
        //                    break;
        //                case 4:
        //                    section4.Add(NotesList.keysMasterList[i]);
        //                    break;
        //            }
        //        }
        //        break;
        //}
        sectionBlock[0] = section0;
        sectionBlock[1] = section1;
        sectionBlock[2] = section2;
        sectionBlock[3] = section3;
        sectionBlock[4] = section4;
    }

    void setUpBoard()
    {
        Debug.Log("Setting up the input board...");
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

    int searchMasterList(string keyObjectName, int keyObjectID)
    {
        int masterListIDMatch = keyObjectID;
        bool searching; searching = true;
        while (searching)
        {
            for (int i = 0; i < NotesList.keysMasterList.Count; i++)
            {
                if (NotesList.keysMasterList[masterListIDMatch].name == keyObjectName)
                {
                    searching = false;

                }
                else
                {
                    if(masterListIDMatch < NotesList.keysMasterList.Count)
                    {
                        masterListIDMatch++;
                    }
                    else
                    {
                        masterListIDMatch = 0;
                    }
                }

            }
        }
        return masterListIDMatch;
    }
}
