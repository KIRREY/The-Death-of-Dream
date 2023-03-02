using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public GameObject panel;
    public Text dialogueText1;
    public GameObject dialogueText2;
    Coroutine fade;

    private void ShowDialogue(string dialogue, dialogueData.TextDia text)
    {
        if(fade != null)
        {
            StopCoroutine(fade);
            fade = null;
        }
        if (dialogue != string.Empty)
        {
            panel.SetActive(true); DialogueManager.Instance.ifTalking = true;
        }
        else
        {
            panel.SetActive(false); DialogueManager.Instance.ifTalking = false;
            EventHandler.CallGameStateChangerEvent(GameState.GamePlay);
        }
        switch (text)
        {
            case dialogueData.TextDia.Text1:
                dialogueText1.transform.parent.gameObject.SetActive(true);
                fade = StartCoroutine(FadeText(dialogueText1, dialogue));
                dialogueText2.gameObject.SetActive(false);
                break;
            case dialogueData.TextDia.Text2:
                dialogueText2.gameObject.SetActive(true);
                fade = StartCoroutine(FadeText(dialogueText2.transform.GetChild(0).gameObject.GetComponent<Text>(), dialogue));
                dialogueText1.transform.parent.gameObject.SetActive(false);
                break;
        }
    }

    private IEnumerator FadeText(Text text,string dialogue)
    {
        text.text = dialogue;
        yield return null;
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
