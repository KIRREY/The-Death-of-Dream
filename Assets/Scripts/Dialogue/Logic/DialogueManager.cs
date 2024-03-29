using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>,ISaveable
{
    public GameObject controller;
    public Text dialogueWho;
    public Image tachie;
    public bool ifEmpty;
    public bool ifTalking;
    public bool ifTransform;
    public bool ifAllTalked;
    public bool ifIntervaled;
    public List<OptionsData> optionsDatas;
    public string eventName;
    public Dictionary<string,int> dialogueIndex=new Dictionary<string,int>();

    private void Update()
    {
        if (ifTalking)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)||ifIntervaled)
            {
                if (!controller.activeInHierarchy)
                    EventHandler.CallShowDialogueEvent(string.Empty, DialogueData.TextDia.Text1, 0f, optionsDatas);
                DialogueController dialogueController=controller.GetComponent<DialogueController>();
                if(!ifAllTalked)
                {
                    EventHandler.CallSpikCurrentDialogueEvent();
                    return;
                }
                if(ifTransform)
                {
                    ifTransform=false;
                    EventHandler.CallShowDialogueEvent(string.Empty,DialogueData.TextDia.Text1,0f,optionsDatas);
                }
                ifEmpty=dialogueController.ifEmpty;
                if (ifEmpty)
                {
                    dialogueController.ShowDialogueEmpty();
                    Debug.Log("empty");
                }
                else
                {
                    dialogueController.ShowDialogueFinish();
                    Debug.Log("finish");
                }
            }
        }
    }

    private void OnEnable()
    {
        EventHandler.BeforeSceneChangeEvent += OnBeforeSceneChangeEvent;
        EventHandler.AfterSceneChangeEvent += OnAfterSceneChangeEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneChangeEvent -= OnBeforeSceneChangeEvent;
        EventHandler.AfterSceneChangeEvent -= OnAfterSceneChangeEvent;
    }

    private void OnAfterSceneChangeEvent()
    {
        foreach(var dialogue in FindObjectsOfType<DialogueController>())
        {
            if (dialogueIndex.ContainsKey(dialogue.name))
                dialogue.index=dialogueIndex[dialogue.name];
        }
    }

    private void OnBeforeSceneChangeEvent()
    {
        foreach(var dialogue in FindObjectsOfType<DialogueController>())
        {
            if (dialogueIndex.ContainsKey(dialogue.name))
            {
                dialogueIndex[dialogue.name]=dialogue.index;
            }
            else
            {
                dialogueIndex.Add(dialogue.name, dialogue.index);
            }
        }
    }

    private void Start()
    {
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.dialogueIndex = this.dialogueIndex;
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        this.dialogueIndex = saveData.dialogueIndex;
    }
}
