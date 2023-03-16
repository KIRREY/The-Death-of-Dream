using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatherRoom_None : DialogueEvent
{
    public AnRingTrigger ring;
    public override void dialogueEventAction()
    {
        ring = FindObjectOfType<AnRingTrigger>();
        ring.amount++;
        StartCoroutine(yield());
    }

    IEnumerator yield()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject.GetComponent<DialogueEvent>());
    }
}
