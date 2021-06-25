using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoteKey;

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
    public float timeBeforeCallsBegin = 1; //This should be the amount of time it takes for player to turn around.
    public float timeInbetweenNewCalls = 5; //How fast new button prompts appear
    public List<noteKey> keyInputblocks;

    [Range(0, 10)]
    public int difficulty;

    //private void OnEnable()
    //{
    //    foreach (noteKey item in keyInputblocks)
    //    {
    //        item.backgroundSprite = badKeySquareSymbol;
    //    }
    //}
    void Start()
    {
        sortKeyInputSections();
        Player = GameObject.Find("Player");
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

        if (mouse0Pressed)
        {
            //Debug.Log("mouse0Pressed is " + mouse0Pressed);
        }
        if (mouse1Pressed)
        {
            //Debug.Log("mouse1Pressed is " + mouse1Pressed);

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
    }

    private void LateUpdate()
    {
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

    IEnumerator SpawnKeyCalls()
    {
        while (currentKeyInputsOnScreen < maxKeyInputsOnScreen)
        {
            InstantiateKeyCall();
            yield return new WaitForSecondsRealtime(timeInbetweenNewCalls);
        }
        yield return null;
    }

    private GameObject keyObjectToBeSpawned;

    void InstantiateKeyCall()
    {
        currentKeyInputsOnScreen++;
        Instantiate(KeySquarePrefab, transform.GetChild(0));
        keyObjectToBeSpawned.transform.localPosition = spawnLocationRange();
        print(currentKeyInputsOnScreen);
        keyObjectToBeSpawned = setRequiredKeyInput(keyObjectToBeSpawned);
    }

    public void setRequiredKeyInput(GameObject keyObjectToBeSpawned)
    {
        keyInputblocks[]
    }
    public void sortKeyInputSections()
    {
        int atListItem = -1;
        foreach(noteKey item in keyInputblocks)
        {
            atListItem++;
            switch (keyInputblocks[atListItem].placementSection)
            {
                case "section 1":
                    break;
                case "section 2":
                    break;
                case "section 3":
                    break;
                case "section 4":
                    break;
            }
        }
    }

    Vector3 spawnLocationRange()
    {
        Vector3 spawnLoc;
        int spawnX, spawnY, spawnZ;

        spawnX = Random.Range(-240, 240);
        spawnY = 0;
        spawnZ = 0;

        spawnLoc.x = spawnX; spawnLoc.y = spawnY; spawnLoc.z = spawnZ;
        return spawnLoc;
    }


    void clearKeys()
    {
        print("CLEARING ALL KEYS");
        currentKeyInputsOnScreen = 0;

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
