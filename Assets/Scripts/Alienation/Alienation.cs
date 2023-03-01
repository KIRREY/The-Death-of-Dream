using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class Alienation : MonoBehaviour
{
    public AlienationLevel alienationLevel;
    private DialogueController dialogueController;

    private void Awake()
    {
        dialogueController = GetComponent<DialogueController>();
    }

    public void OnAlienationChangeEvent()
    {
        dialogueController.ShowDialogueEmpty();
        Debug.Log("empty");
    }
}
