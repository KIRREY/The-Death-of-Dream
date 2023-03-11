using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Prologue_1_1_1 : DialogueEvent
{
    public override void dialogueEventAction()
    {
        foreach(var obj in Resources.FindObjectsOfTypeAll<RawImage>())
        {
            if (obj.gameObject.name == "RawImageInPrologue") 
                obj.gameObject.SetActive(true);break;
        }
        Destroy(this.gameObject.GetComponent<DialogueEvent>());
    }
}
