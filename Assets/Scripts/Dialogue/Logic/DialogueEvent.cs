using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueEvent : MonoBehaviour
{
    public GameObject Dialogue;
    public abstract void dialogueEventAction(); 
}
