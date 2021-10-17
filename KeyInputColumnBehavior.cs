using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NoteKey;
using InputColumns;

public class KeyInputColumnBehavior : MonoBehaviour
{
    public bool readyForInput;
    public GameObject spawnManager;
    private KeyInputSpawner spawnerScript;

    public GameObject pingGraphic;

    void Awake()
    {
        spawnerScript = spawnManager.GetComponent<KeyInputSpawner>();
    }

    void Update()
    {
        
    }

    public bool testPing;

    private void LateUpdate()
    {
        
        if (testPing)
        {
            noteKeyPingEffect();
            testPing = false;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "keyInputObject")
        {
            //readyForInput = true;
            GetComponent<Image>().color = Color.green;
            spawnerScript.columnsFreeCount--;
        }
    }

    private void OnTriggerExit(Collider other)          //When the note leaves, if it's progress is 0 it is complete.
    {
        if (other.tag == "keyInputObject")
        {
            spawnerScript.columnsFreeCount++;
            //readyForInput = false;
            GetComponent<Image>().color = Color.black;
            if(other.GetComponent<KeyInputBehavior>().progress <= .2f)
            {
                other.GetComponent<KeyInputBehavior>().despawnNote();
                noteKeyPingEffect();
            }
        }
    }

    Vector3 innerWavePos;
    Vector3 outerWavePos;
    void noteKeyPingEffect()    //Progress complete effect
    {
        innerWavePos.y = 50; outerWavePos.y = 64;
        GameObject effectWave_0, effectWave_1, effectWave_2, effectWave_3;
        effectWave_0 = Instantiate(pingGraphic, gameObject.transform);
        effectWave_1 = Instantiate(pingGraphic, gameObject.transform);
        effectWave_2 = Instantiate(pingGraphic, gameObject.transform);
        effectWave_3 = Instantiate(pingGraphic, gameObject.transform);
        //Positions
        effectWave_0.transform.localPosition = innerWavePos;
        effectWave_1.transform.localPosition = outerWavePos;
        effectWave_2.transform.localPosition = -innerWavePos;
        effectWave_3.transform.localPosition = -outerWavePos;
        //Rotations
        effectWave_2.transform.Rotate(Vector3.forward * 180);
        effectWave_3.transform.Rotate(Vector3.forward * 180);

        effectWave_0.GetComponent<Rigidbody>().AddForce(transform.up * 30, ForceMode.Impulse);
        effectWave_1.GetComponent<Rigidbody>().AddForce(transform.up * 50, ForceMode.Impulse);
        effectWave_2.GetComponent<Rigidbody>().AddForce(transform.up * -30, ForceMode.Impulse);
        effectWave_3.GetComponent<Rigidbody>().AddForce(transform.up * -50, ForceMode.Impulse);


    }

}
