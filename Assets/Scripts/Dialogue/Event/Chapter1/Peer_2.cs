using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peer_2 : DialogueEvent
{
    public override void dialogueEventAction()
    {
        TransitionManager.Instance.Transition("YuShengRoom", "FirstDreem", false);
    }
}
