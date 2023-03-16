using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIOptions : Singleton<DialogueUIOptions>
{
    Coroutine select;
    public GameObject OptionsPanel;
    public GameObject optionNull;
    public GameObject currentOption;
    public Button button;
    public Image Image;
    public int index;
    public int max;
    private void OnEnable()
    {
        index = 0;
        max = OptionsPanel.transform.childCount;
        if (select != null)
        {
            select = null;
        }
        select = StartCoroutine(SelectOptions());
    }

    private void OnDisable()
    {
        if (select != null)
        {
            select = null;
        }
    }


    IEnumerator SelectOptions()
    {
        Fade(index);
        EventHandler.CallGameStateChangerEvent(GameState.Pause);
        while (true)
        {
            max = OptionsPanel.transform.childCount;
            if (Input.GetKeyDown(KeyCode.W))
            {
                index = Mathf.Clamp(index - 1, 0, max - 1);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                index = Mathf.Clamp(index + 1, 0, max - 1);
            }
            Fade(index);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                DialogueManager.Instance.controller = optionNull;
                button.SendMessage("Press");
            }
            yield return null;
        }
    }

    void TransformDialogue(DialogueController dialogueController)
    {
        DialogueController nullController = optionNull.GetComponent<DialogueController>();
        nullController.dialogueEmpty=dialogueController.dialogueEmpty;
        nullController.ifEmpty = true;
        nullController.FillDialogueStack();
    }

    void Fade(int indexF)
    {
        try
        {
            if(Image!=null)
                Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, 1);
            currentOption = OptionsPanel.transform.GetChild(indexF).gameObject;
            TransformDialogue(currentOption.GetComponent<DialogueController>());
            button = currentOption.GetComponent<Button>();
            Image = currentOption.GetComponent<Image>();
            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, Mathf.PingPong(Time.time, 1));
        }
        catch { }
    }
}
