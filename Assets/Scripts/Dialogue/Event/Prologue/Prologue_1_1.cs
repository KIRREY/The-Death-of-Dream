using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Prologue_1_1 : DialogueEvent
{
    public override void dialogueEventAction()
    {
        GameObject.Find("ProloguePanel").GetComponent<Image>().color = Color.red;
        StartCoroutine(yield());
    }

    IEnumerator yield()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject.GetComponent<DialogueEvent>());
    }
}
