using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DialogueManager : Singleton<DialogueManager>,ISaveable
{
    public GameObject controller;
    public bool ifEmpty;
    public bool ifTalking;
    public Dictionary<string,int> dialogueIndex=new Dictionary<string,int>();

    private void Update()
    {
        if (ifTalking)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            {
                DialogueController dialogueController=controller.GetComponent<DialogueController>();
                if (ifEmpty)
                    dialogueController.ShowDialogueEmpty();
                else
                    dialogueController.ShowDialogueFinish();
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
