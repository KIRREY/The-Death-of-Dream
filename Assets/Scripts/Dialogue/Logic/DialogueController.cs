using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO[] dialogueEmptys;
    public DialogueData_SO dialogueEmpty;
    public DialogueData_SO[] dialogueAlienations;
    public DialogueData_SO dialogueAlienation;
    public DialogueData_SO dialogueFinish;
    public int index;
    public bool ifTransform;
    public bool ifEmpty;

    private Stack<string> dialogueEmptyStack;
    private Stack<string> dialogueFinishStack;
    DialogueManager dialogueManager;

    private bool isTalking;

    private void Awake()
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
    }

    private void FillDialogueStack()
    {
        dialogueEmptyStack = new Stack<string>();
        dialogueFinishStack = new Stack<string>();

        for (int i = dialogueEmpty.dialogueList.Count - 1; i > -1; i--)
        {
            dialogueEmptyStack.Push(dialogueEmpty.dialogueList[i]);
        }
        for (int i = dialogueFinish.dialogueList.Count - 1; i > -1; i--)
        {
            dialogueFinishStack.Push(dialogueFinish.dialogueList[i]);
        }
    }

    public void ShowDialogueEmpty()
    {
        if (!isTalking)
        {
            dialogueManager.controller = this.gameObject;
            dialogueManager.dialogueWho.text = dialogueEmpty.who;
            dialogueManager.tachie.color = new Color(0, 0, 0, 1);
            if (dialogueEmpty.Tachie == null)
                dialogueManager.tachie.color = new Color(0, 0, 0, 0);
            else
                dialogueManager.tachie.sprite = dialogueEmpty.Tachie;
            StartCoroutine(DialogueRoutine(dialogueEmptyStack));
        }
    }

    public void ShowDialogueFinish()
    {
        if (!isTalking)
        {
            dialogueManager.controller = this.gameObject;
            dialogueManager.ifEmpty = false;
            dialogueManager.dialogueWho.text = dialogueFinish.who;
            dialogueManager.tachie.color = new Color(0, 0, 0, 1);
            if (dialogueFinish.Tachie == null)
                dialogueManager.tachie.color = new Color(0, 0, 0, 0);
            else
                dialogueManager.tachie.sprite = dialogueFinish.Tachie;
            StartCoroutine(DialogueRoutine(dialogueFinishStack));
        }
    }

    private IEnumerator DialogueRoutine(Stack<string> data)
    {
        isTalking = true;
        if (data.TryPop(out string result))
        {
            EventHandler.CallShowDialogueEvent(result);
            yield return null;
            isTalking = false;
            EventHandler.CallGameStateChangerEvent(GameState.Pause);
        }
        else
        {
            EventHandler.CallShowDialogueEvent(string.Empty);
            if (index < (dialogueEmptys.Length - 1))
            {
                index++;
                dialogueEmpty = dialogueEmptys[index];
            }
            else
                ifEmpty=false;
            FillDialogueStack();
            isTalking = false; 
            EventHandler.CallGameStateChangerEvent(GameState.GamePlay);
        }
    }

    private void OnEnable()
    {
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
        if (dialogueAlienations != null&&ifTransform)
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
        if (dialogueAlienations != null&&AlienationManager.Instance.alienationLevel>=AlienationLevel.Brain&&!ifTransform)
        {
            DialogueData_SO[] transform;
            transform = dialogueAlienations;
            dialogueAlienations = dialogueEmptys;
            dialogueEmptys = transform;
            FillDialogueStack();
            ifTransform=true;
            try
            {
                dialogueEmpty = dialogueEmptys[index];
            }
            catch { }
        }
    }
}
