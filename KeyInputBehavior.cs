using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NoteKey;
using UnityEngine.UI;
using InputColumns;

public class KeyInputBehavior : MonoBehaviour
{
    private GameObject SpawnerObject;
    private KeyInputSpawner SpawnerScript;

    public int keyID;


    public int desiredColumn;
    public int finalColumn;

    public bool moving;
    public bool keySquare_Ready;
    public bool tapKey;
    [Range (1,0)]
    public float progress;
    public float progressModifier;
    void Start()
    {
        
    }

    void Awake()
    {
        moving = true;
        SpawnerObject = GameObject.Find("KeyPromptManager");
        SpawnerScript = SpawnerObject.GetComponent<KeyInputSpawner>();
        desiredColumn = Random.Range(0, 5);                     //rolls for the column the input wants to go to
        progress = 1f;

        if (tapKey)
        {
            transform.GetChild(0).GetComponent<TMP_Text>().color = Color.yellow;
        }
        //progressModifier = -.5f;
    }


    void Update()
    {
        

        Vector4 imgColor; Color col;
        col = GetComponent<Image>().color;
        //gunna do a lerp.
        
        imgColor = new Vector4(col.r, col.g, col.b, Mathf.Lerp(0f, 1f, progress));
        GetComponent<Image>().color = imgColor;

        Vector4 textColor; Color tCol;
        tCol = transform.GetComponentInChildren<TMP_Text>().color;
        textColor = new Vector4(tCol.r, tCol.g, tCol.b, Mathf.Lerp(0f, 1f, progress));
        transform.GetComponentInChildren<TMP_Text>().color = textColor;

        if(progress <= 0)       //This SHOULD force the input key to trigger the collision exit stuff, and get despawned.
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 250, transform.localPosition.z);
        }

        if (keySquare_Ready)
        {
            listeningForInput();
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        

        if (transform.localPosition.y > 270)
        {
            despawnNote();
        }

        if (moving)
        {
            transform.Translate(Vector3.up * Time.deltaTime * SpawnerScript.spawnMoveSpeed);

        }

        //if(progress <= 0)
        //{
        //    keySquare_Ready = false;
        //}

        //if(!moving & !keySquare_Ready)
        //{
        //    despawnNote();
        //}


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ColumnSlot")
        {
            keySquare_Ready = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "SpawnBox")
        {
            //Debug.Log(gameObject.name + " desiredColumn is " + desiredColumn);

            //Debug.Log(gameObject.name + " has left the spawnBox");
            //move to one of the 5 columns
            if(SpawnerScript.columnsFreeCount == 0)
            {
                moving = false;
                GetComponentInChildren<TMP_Text>().color = Color.red;
            }
            else
            {
                setColumn();
                transform.localPosition = SpawnerScript.fingerInputColumns[finalColumn].location;
            }
            Debug.Log(gameObject.name + " keyID is " + keyID + " and my required input is " + SpawnerScript.NotesInUse[keyID].requiredInput);
        }
        if(other.gameObject.tag == "ColumnSlot")
        {
            keySquare_Ready = false;
            SpawnerScript.fingerInputColumns[finalColumn].isFree = true;

        }
    }
    
    void setColumn()
    {
        bool searching;
        searching = true;

        while (searching)
        {
            if (SpawnerScript.fingerInputColumns[desiredColumn].isFree)
            {
                searching = false;
                columnValueConversion();
            }
            else
            {
                if(desiredColumn < 4)
                {
                    desiredColumn++;
                }
                else if(desiredColumn == 4)
                {
                    desiredColumn = 0;
                }
            }
        }
    }

    void columnValueConversion()
    {
        finalColumn = desiredColumn;
        if (SpawnerScript.fingerInputColumns[finalColumn].isFree)
        {
            SpawnerScript.fingerInputColumns[finalColumn].isFree = false;
        }
        else
        {
            Debug.LogWarning("FAILED TO PICK FREE COLUMN");
            SpawnerScript.StopAllCoroutines(); Debug.LogWarning("Stopping spawning calls");
            despawnNote();
        }
    }

    void listeningForInput()        //This makes the note key react when the proper input is pressed.
    {
        if (Input.GetKey(SpawnerScript.NotesInUse[keyID].requiredInput))
        {
            //print("CORRECT KEY" + SpawnerScript.NotesInUse[keyID].requiredInput);
            progress = progress + progressModifier * Time.deltaTime;
            //moving = false;
            if(progress > SpawnerScript.progNeeded)
            {
                transform.localPosition = SpawnerObject.transform.GetChild(0).Find("Column_" + finalColumn).localPosition;
                moving = false;
            }
            else
            {
                moving = true;
            }
            if (tapKey)
            {
                progress = 0;
            }

            //if(progress <= SpawnerScript.progNeeded)
            //{
            //    moving = true;
            //}
        }
        else
        {
            moving = true;
        }
    }

    public void despawnNote()
    {
        Destroy(gameObject);
        SpawnerScript.currentKeyInputsOnScreen--;
        SpawnerScript.keyInputblocks[keyID].isOnScreen = false;
        SpawnerScript.fingerInputColumns[finalColumn].isFree = true;
        if (progress <= SpawnerScript.progNeeded)
        {
            SpawnerScript.notesCompletedThisStage++;
        }
        if (!tapKey)
        {
            SpawnerScript.heldNoteOnScreen = false;
        }

        //transform.parent.Find("Column_" + finalColumn.ToString()).GetComponent<BoxCollider>().

    }
}
