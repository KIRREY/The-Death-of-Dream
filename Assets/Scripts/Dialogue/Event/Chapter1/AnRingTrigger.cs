using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DialogueController))]
public class AnRingTrigger : MonoBehaviour
{
    public int amount;
    public int target;
    public DialogueController controller;

    private void Start()
    {
        controller=GetComponent<DialogueController>();
    }

    private void Update()
    {
        if(amount==target&&!DialogueManager.Instance.ifTalking)
        {
            EventHandler.CallGameStateChangerEvent(GameState.Pause);
            controller.ShowDialogueEmpty();
            Destroy(this.gameObject.GetComponent<AnRingTrigger>());
        }
    }
}
