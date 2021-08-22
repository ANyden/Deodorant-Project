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
    public float Speed;


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
        Speed = SpawnerScript.spawnMoveSpeed;
        if (tapKey)
        {
            GetComponent<Image>().color = Color.blue;
        }
        //progressModifier = -.5f;
    }


    void Update()
    {
        if (keySquare_Ready)
        {
            listeningForInput();
        }

        Vector4 imgColor; Color col;
        col = GetComponent<Image>().color;
        //gunna do a lerp.
        
        imgColor = new Vector4(col.r, col.g, col.b, Mathf.Lerp(0f, 1f, progress));
        GetComponent<Image>().color = imgColor;

        Vector4 textColor; Color tCol;
        tCol = transform.GetComponentInChildren<TMP_Text>().color;
        textColor = new Vector4(tCol.r, tCol.g, tCol.b, Mathf.Lerp(0f, 1f, progress));
        transform.GetComponentInChildren<TMP_Text>().color = textColor;
    }

    private void LateUpdate()
    {
        if (transform.localPosition.y > 270)
        {
            despawnNote();
        }

        if (moving)
        {
            transform.Translate(Vector3.up * Time.deltaTime * Speed);

        }

        if(progress <= 0)
        {
            keySquare_Ready = false;
        }

        if(!moving & !keySquare_Ready)
        {
            despawnNote();
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
            }
            else
            {
                setColumn();
                transform.localPosition = SpawnerScript.fingerInputColumns[finalColumn].location;

            }
            //Debug.Log(gameObject.name + " 's column would be " + desiredColumn);



        }
        if(other.gameObject.tag == "keyInputObject")
        {
            keySquare_Ready = false;

        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ColumnSlot")
        {
            keySquare_Ready = true;
        }
        else
        {

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
        SpawnerScript.fingerInputColumns[finalColumn].isFree = false;
    }

    void listeningForInput()        //This makes the note key react when the proper input is pressed.
    {
        if (Input.GetKey(SpawnerScript.keyInputblocks[keyID].requiredInput))
        {
            transform.localPosition = SpawnerObject.transform.GetChild(0).Find("Column_" + finalColumn).localPosition;
            progress = progress + progressModifier * Time.deltaTime;
            moving = false;
            if (tapKey)
            {
                progress = 0;
            }

            if(progress <= 0)
            {
                moving = true;
            }
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
    }
}
