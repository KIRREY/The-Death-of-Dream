using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DialogueData_SO",menuName ="Dialogue/DialogueData_SO")]
public class DialogueData_SO : ScriptableObject
{
    public List<dialogueData> dialogueList;
}
[System.Serializable]
public class dialogueData
{
    [TextArea]
    public string dialogue;
    public string who;
    public Sprite tachie;
    public enum TextDia { Text1, Text2 };
    public TextDia text;
}
