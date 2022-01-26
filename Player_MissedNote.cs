using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_MissedNote : MonoBehaviour
{
    public float maxHealth;

    public float currentHealth;
    public float regenRate;
    public float missedNoteDamage;

    public Canvas DebugCanvas;
    public bool start;
    public bool failState;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        //missedNoteTakeDamage();
        

        currentHealth = Mathf.Clamp(currentHealth, -10, maxHealth);

        if (currentHealth < maxHealth & currentHealth > 0)
        {
            StartCoroutine(healthRegen());

        }
        else
        {
            StopCoroutine(healthRegen());
        }

        if (failState)
        {
            gameOver();
        }


        DebugCanvas.transform.Find("DEBUG(health tracker)").transform.GetChild(0).GetComponent<TMP_Text>().SetText(currentHealth.ToString());
    }

    public void missedNoteTakeDamage()
    {
        
        currentHealth = currentHealth - missedNoteDamage;

    }

    IEnumerator healthRegen()
    {
        
        currentHealth = currentHealth + regenRate * Time.deltaTime;

        yield return null;
    }

    public void gameOver()
    {
        switch (GetComponent<changeGameModeSequence>().gamePhase)
        {
            case 3:
                regenRate = 0;
                currentHealth = 0;
                break;
            case 4:
                regenRate = 0;
                currentHealth = 0;
                GetComponent<changeGameModeSequence>().gamePhase++;     //Move into Phase 5
                break;


        }
    }


}
