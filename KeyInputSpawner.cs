using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoteKey;
using TMPro;
using InputColumns;

public class KeyInputSpawner : MonoBehaviour
{
    public GameObject KeySquarePrefab;
    //public Sprite badKeySquareSymbol;
    private GameObject Player;
    public bool startSpawn;
    public bool mouse0Pressed;
    public bool mouse1Pressed;
    public bool mouseHeldDown;

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
    public bool[] secInUseBools;



    void Start()
    {
        notesCompletedThisStage = 0;
        sortKeyInputSections();
        setUpBoard();
        Player = GameObject.Find("Player");
        Difficulty = GetComponent<DifficultyManger>();
        spawnMoveSpeed = Difficulty.moveSpeed;       //KeyInputBehavior calls on moveSpeed

    }

    void Update()
    {
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
            if (!startSpawn)
            {
                StartCoroutine(SpawnKeyCalls());
                startSpawn = true;
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (currentKeyInputsOnScreen <= maxKeyInputsOnScreen)
            {
                InstantiateKeyCall();
            }
            else
            {
                Debug.LogWarning("Too many keys on screen");
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
        if (!mouseHeldDown)
        {
            if (startSpawn)
            {
                startSpawn = false;
                StopAllCoroutines();
                Debug.Log("Mouse released. Stopping loop.");
                clearKeys(); //resets the level- removes key inputs on screen and sets currentKeyInputsOnScreen to 0
            }
        }
    }

    public IEnumerator SpawnKeyCalls()
    {
        while (currentKeyInputsOnScreen <= maxKeyInputsOnScreen)
        {
            Debug.Log("CALLING NEW KEY");
            InstantiateKeyCall();
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
        Difficulty.difficulty_Sections();
        noteKey chosenNoteKey;
        int chosenNoteKeyID;
        chosenNoteKeyID = setNoteObject();
        //Debug.Log(chosenNoteKeyID);
        chosenNoteKey = keyInputblocks[chosenNoteKeyID];

        currentKeyInputsOnScreen++;
        spawnThis = Instantiate(KeySquarePrefab, transform.GetChild(0));            //Instantiates a KeySquarePrefab
        spawnThis.transform.localPosition = spawnLocationRange();                   //Moves the object to the spawnBox
        spawnThisText = spawnThis.GetComponentInChildren<TMP_Text>();               //Sets spawnThisText as the text object parented to the KeySquarePrefab
        spawnThisText.SetText(chosenNoteKey.name);                                  //Sets the text object parented to the KeySquarePrefab to whatever was 
        spawnThis.name = spawnThisText.text;                                        //Changes the KeySquarePrefab object's name to it's required input
        chosenNoteKey.isOnScreen = true;                                            //Marks this key input as being on screen, so the spawner won't make another key of the same input.
        if (isThisNoteHeldOrTapped())
        {
            spawnThis.GetComponent<KeyInputBehavior>().tapKey = true;
            spawnThisText.color = Color.yellow;
        }
        spawnThis.GetComponent<KeyInputBehavior>().progressModifier = Difficulty.globalProgressModifier;

        spawnThis.GetComponent<KeyInputBehavior>().keyID = chosenNoteKeyID;
    }

    int setNoteObject() //Picks a key input from the list.                          //maxNum is based on current NotesInUse, which is based on difficulty.
    {
        int maxNum; maxNum = NotesInUse.Count;
        bool searching; searching = true;
        int keyObjectID;
        keyObjectID = Random.Range(0, maxNum);
        while (searching)
        {
            if (!keyInputblocks[keyObjectID].isOnScreen)                                 //If a note is already on screen, this code will choose a different one from the list
            {
                searching = false;
            }
            else
            {
                if(keyObjectID < maxNum)
                {
                    keyObjectID++;
                }
                else if(keyObjectID == maxNum)
                {
                    keyObjectID = 0;
                }
            }
        }
        
        return keyObjectID;
    }

    public bool isThisNoteHeldOrTapped()        //Determines if this Note needs to be held or tapped
    {
        bool tappedNote;
        int num = Random.Range(1, 10);
        if (num <= Difficulty.tapChance)
        {
            tappedNote = true;
        }
        else
        {
            tappedNote = false;
        }
        return tappedNote;
    }

    public void clearNotesInUse()
    {
        NotesInUse.Clear();
        int item = -1;
        foreach (bool entry in secInUseBools)
        {
            item++;
            secInUseBools[item] = false;
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
        Debug.Log("atListItem is at " + atListItem);

        section0 = new List<noteKey>();
        section1 = new List<noteKey>();
        section2 = new List<noteKey>();
        section3 = new List<noteKey>();
        section4 = new List<noteKey>();

        foreach(noteKey item in keyInputblocks)
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
        secInUseBools = new bool[5];
        secInUseBools[0] = false; secInUseBools[1] = false; secInUseBools[2] = false; secInUseBools[3] = false; secInUseBools[4] = false;

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
}
