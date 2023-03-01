using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class CommonInteractive : Interactive
{
    private DialogueController dialogueController;

    private void Awake()
    {
        dialogueController = GetComponent<DialogueController>();
    }

    public override void EmptyAction()
    {
        if (isDone)
            dialogueController.ShowDialogueFinish();
        else
        {
            dialogueController.ShowDialogueEmpty();
            isDone=true;
        }
    }

    protected override void OnAction()
    {
        dialogueController.ShowDialogueFinish();
    }
}
