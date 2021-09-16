using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class AnxietyManager : MonoBehaviour
{
    public static AnxietyManager current;
    public event Action<int> TextAction;

    public int level;
    private int QuestionText;
    private GameObject container;
    private GameObject levelQuestion;

    private TMP_Text levelQuestionText;
    private TMP_Text anxietyPrompt;
    public string[] LevelQuestionPrompts;
    public string[] L1_prompts;

    private Button yes;
    private Button no;

    public bool PlayerHasChosenAStick;

    //The Anxiety Prompts go here. When the menu manager sends

    public void Awake()
    {
        current = this;
    }
    void Start()
    {
        container = transform.GetChild(1).gameObject;
        level = 1;
        anxietyPrompt = container.transform.GetChild(level).GetComponent<TextMeshProUGUI>();
    }
    
    public void AnxietyQuestion(int anxietyID) //Called by Deoderant, part of event system
    {
        if (TextAction != null)
        {
            TextAction(anxietyID);
            FirstQuestion();
            print("anxietyID is " + anxietyID);
            QuestionText = anxietyID;
            //Roll for which minigame based on level
            //Set minigame as variable
        }
    }

    void FirstQuestion()
    {
        yes = container.transform.GetChild(level).GetChild(0).GetComponent<Button>();
        no = container.transform.GetChild(level).GetChild(1).GetComponent<Button>();

        levelQuestion = container.transform.GetChild(level).gameObject; //Gets the Level Prompt based on current level
        levelQuestionText = levelQuestion.GetComponent<TextMeshProUGUI>();

        levelQuestion.SetActive(true);
        levelQuestionText.SetText(LevelQuestionPrompts[level]); //Resets the First Question Text

        FirstQuestionButtons();
    }
    //First Question Answers:
    public void Q1Yes()
    {
        SecondQuestion();
    }

    public void Q1No()
    {
        MenuEvents.current.menuClose += Denied;
        MenuEvents.current.MenuDeactivation();
    }

    void Deny()
    {
        MenuEvents.current.menuClose += Denied;
        MenuEvents.current.MenuDeactivation();
        //raise level
    }

    void Denied()//This is the event that is sent to MenuEvents
    {
        //MenuEvents.current.player.GetComponent<HeadTurning>().lockCursor();
        MenuEvents.current.container.SetActive(false);
        MenuEvents.current.container.transform.GetChild(0).gameObject.SetActive(false);
    }

    void Confirm()
    {
        //next stage of play
    }

    void SecondQuestion()
    {
        switch (level)
        {
            case 1:
                L1QuestionPrompts();
                break;
            case 2:
                L2QuestionPrompts();
                break;
            case 3:
                L3QuestionPrompts();
                break;
            case 4:
                L4QuestionPrompts();
                break;
        }
        
        print("SecondQuestion will go here");
    }

    void L1QuestionPrompts()
    {
        anxietyPrompt.SetText(L1_prompts[QuestionText]);
        //print("QuestionText is " + QuestionText);
        switch (QuestionText)
        {
            case 0:
                //Are you sure?
                ButtonSetupA();
                break;
            case 1:
                //Really? This one?
                ButtonSetupA();
                break;
            case 2:
                //Is it safe?
                ButtonSetupA();
                break;
            case 3:
                //Look at another one?
                ButtonSetupB();
                break;
            case 4:
                //You might like one of the others more...
                ButtonSetupB();
                break;
        }
    }

    void L2QuestionPrompts()
    {

    }

    void L3QuestionPrompts()
    {

    }

    void L4QuestionPrompts()
    {

    }
    //PromptGame<anxietyID>(anxietyID)

    //ButtonBehavior is going to be moved to another script later
    void FirstQuestionButtons()
    {
        ClearButtonListeners();
        yes.onClick.AddListener(Q1Yes);
        no.onClick.AddListener(Q1No);
        //Yes is confirm
        //No is deny
    }

    void ButtonSetupA()
    {
        ClearButtonListeners();
        yes.onClick.AddListener(Confirm);
        no.onClick.AddListener(Deny);
        //Yes is confirm
        //No is deny
    }

    void ButtonSetupB()
    {
        ClearButtonListeners();
        yes.onClick.AddListener(Deny);
        no.onClick.AddListener(Confirm);
        //Yes is deny
        //No is confirm
    }

    void ClearButtonListeners()
    {
        yes.onClick.RemoveAllListeners();
        no.onClick.RemoveAllListeners();
    }
    
}
