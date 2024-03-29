using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DialogueData_SO",menuName ="Dialogue/DialogueData_SO")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialogueData> dialogueList;
}
[System.Serializable]
public class DialogueData
{
    [TextArea]
    public string dialogue;
    public string who;
    public Sprite tachie;
    public enum TextDia { Text1, Text2 };
    public TextDia text;
    public float interval;
    public List<OptionsData> optionsDatas;
    [Tooltip("输入添加的脚本的名字")]public string eventName;
}
[System.Serializable]
public class OptionsData
{
    [TextArea]
    public string option;
    public DialogueData_SO nextDialogue;
}
