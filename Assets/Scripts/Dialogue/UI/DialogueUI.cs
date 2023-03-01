using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public GameObject panel;
    public Text dialogueText;

    private void ShowDialogue( string dialogue)
    {
        if (dialogue != string.Empty)
        {
            panel.SetActive(true); DialogueManager.Instance.ifTalking = true;
        }
        else
        {
            panel.SetActive(false); DialogueManager.Instance.ifTalking = false;
            EventHandler.CallGameStateChangerEvent(GameState.GamePlay);
        }
        dialogueText.text = dialogue;
    }

    private void OnEnable()
    {
        EventHandler.ShowDialogueEvent += ShowDialogue;
    }

    private void OnDisable()
    {
        EventHandler.ShowDialogueEvent -= ShowDialogue;
    }

}
