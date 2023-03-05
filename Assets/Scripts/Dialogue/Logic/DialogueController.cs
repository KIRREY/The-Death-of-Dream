using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO[] dialogueEmptys;
    public DialogueData_SO dialogueEmpty;
    public DialogueData_SO[] dialogueAlienations;
    public DialogueData_SO dialogueFinish;
    public DialogueData_SO dialogueFinish_A;
    public int index;
    public bool ifTransform;
    public bool ifEmpty;

    private Stack<DialogueData> dialogueEmptyStack;
    private Stack<DialogueData> dialogueFinishStack;
    DialogueManager dialogueManager;

    private bool isTalking;

    public void FillDialogueStack()
    {
        dialogueEmptyStack = new Stack<DialogueData>();
        dialogueFinishStack = new Stack<DialogueData>();
        try
        {
            Debug.Log(dialogueEmpty.dialogueList.Count);
        }
        catch
        { Debug.Log("None");
            try
            {
                Debug.Log(dialogueEmpty);
            }
            catch { Debug.Log("NO!"); }
        }

        try
        {
            for (int i = dialogueEmpty.dialogueList.Count - 1; i > -1; i--)
            {
                dialogueEmptyStack.Push(dialogueEmpty.dialogueList[i]);
            }

            for (int i = dialogueFinish.dialogueList.Count - 1; i > -1; i--)
            {
                dialogueFinishStack.Push(dialogueFinish.dialogueList[i]);
            }
        }
        catch { }
    }

    public void ShowDialogueEmpty()
    {
        if (!isTalking)
            StartCoroutine(DialogueRoutine(dialogueEmptyStack));
    }

    public void ShowDialogueFinish()
    {
        if (!isTalking)
        {
            dialogueManager.ifEmpty = false;
            StartCoroutine(DialogueRoutine(dialogueFinishStack));
        }
    }

    private IEnumerator DialogueRoutine(Stack<DialogueData> data)
    {
        isTalking = true;
        if (data.TryPop(out var result))
        {
            EventHandler.CallShowDialogueEvent(result.dialogue, result.text, result.interval,result.optionsDatas);
            dialogueManager.dialogueEvent = result.dialogueEvent;
            dialogueManager.optionsDatas = result.optionsDatas;
            dialogueManager.controller = this.gameObject;
            dialogueManager.dialogueWho.text = result.who;
            dialogueManager.tachie.color = new Color(0, 0, 0, 1);
            if (result.tachie == null)
                dialogueManager.tachie.color = new Color(0, 0, 0, 0);
            else
                dialogueManager.tachie.sprite = result.tachie;
            yield return null;
            isTalking = false;
            EventHandler.CallGameStateChangerEvent(GameState.Pause);
        }
        else
        {
            EventHandler.CallShowDialogueEvent(string.Empty, DialogueData.TextDia.Text1, 0f,new List<OptionsData>());
            switch (dialogueManager.dialogueEvent)
            {
                case DialogueEvent.None: break;
                case DialogueEvent.Teleport:
                    gameObject.GetComponent<Teleport>().TeleportToScene();
                    break;
            }
            dialogueManager.dialogueEvent = DialogueEvent.None;
            if (index < (dialogueEmptys.Length - 1))
            {
                index++;
                dialogueEmpty = dialogueEmptys[index];
            }
            else
                ifEmpty = false;
            FillDialogueStack();
            isTalking = false;
        }
    }

    private void OnEnable()
    {
        try
        {
            index = 0;
            dialogueEmpty = dialogueEmptys[index];
        }
        catch { }
        ifEmpty = true;
        FillDialogueStack();
        dialogueManager = DialogueManager.Instance;
        isTalking = false;
        if (gameObject.tag == "StoryDialogue")
        {
            ShowDialogueEmpty();
        }
        EventHandler.AlienationEvent += OnAlienationEvent;
        EventHandler.ExitAlienationEvent += ExitAlienationAction;
        EventHandler.AfterSceneChangeEvent += OnAlienationEvent;
    }

    private void OnDisable()
    {
        EventHandler.AlienationEvent -= OnAlienationEvent;
        EventHandler.ExitAlienationEvent -= ExitAlienationAction;
        EventHandler.AfterSceneChangeEvent -= OnAlienationEvent;
    }

    private void ExitAlienationAction()
    {
        if (dialogueAlienations != null && ifTransform)
        {
            DialogueData_SO[] transform;
            transform = dialogueEmptys;
            dialogueEmptys = dialogueAlienations;
            dialogueAlienations = transform;
            FillDialogueStack();
            ifTransform = false;
            try
            {
                dialogueEmpty = dialogueEmptys[index];
            }
            catch { }
        }
    }

    public void OnAlienationEvent()
    {
        if (AlienationManager.Instance.alienationLevel >= AlienationLevel.Brain && !ifTransform && AlienationManager.Instance.ifAlienation)
        {
            if (dialogueAlienations == null && dialogueFinish_A == null)
                return;
            ifEmpty = true;
            index = 0;
            ifTransform = true;
            dialogueManager.ifTransform = true;
            if (dialogueAlienations != null)
            {
                DialogueData_SO[] transform;
                transform = dialogueAlienations;
                dialogueAlienations = dialogueEmptys;
                dialogueEmptys = transform;
                try
                {
                    dialogueEmpty = dialogueEmptys[index];
                }
                catch { }
            }

            if (dialogueFinish_A != null)
            {
                DialogueData_SO dialogue;
                dialogue = dialogueFinish_A;
                dialogueFinish_A = dialogueFinish;
                dialogueFinish = dialogue;
            }
            FillDialogueStack();
        }
    }
}
