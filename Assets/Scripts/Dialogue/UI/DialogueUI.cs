using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public GameObject panel;
    public Text dialogueText1;
    public GameObject dialogueText2;
    public GameObject option;
    public GameObject optionPanel;
    Coroutine fade;
    private string currentDialogue;
    private Text currentText;
    private float currentInterval;
    [SerializeField]private List<OptionsData> currentOptionDatas;
    [SerializeField]private bool ifOption;

    private void ShowDialogue(string dialogue, DialogueData.TextDia text, float interval,List<OptionsData> optionsDatas)
    {
        Debug.Log(optionsDatas.Count);
        currentDialogue = dialogue;
        currentInterval = interval;
        if(optionPanel.transform.childCount!=0)
        {
            foreach (ObjectPool options in optionPanel.transform.GetComponentsInChildren<ObjectPool>(true))
            {
                options.Reset();
            }
        }
        if(optionsDatas.Count!=0)
            ifOption=true;
        optionPanel.SetActive(false);
        if (fade != null)
        {
            StopCoroutine(fade);
            fade = null;
        }
        if (dialogue != string.Empty)
        {
            panel.SetActive(true); DialogueManager.Instance.ifTalking = true;
            currentOptionDatas = optionsDatas;
        }
        else
        {
            panel.SetActive(false); DialogueManager.Instance.ifTalking = false;
            if (interval != 0)
                DialogueManager.Instance.ifTalking = true;
            else
                EventHandler.CallGameStateChangerEvent(GameState.GamePlay);
        }
        switch (text)
        {
            case DialogueData.TextDia.Text1:
                dialogueText1.transform.parent.gameObject.SetActive(true);
                fade = StartCoroutine(FadeText(dialogueText1, dialogue, interval,optionsDatas));
                dialogueText2.gameObject.SetActive(false);
                break;
            case DialogueData.TextDia.Text2:
                dialogueText2.gameObject.SetActive(true);
                fade = StartCoroutine(FadeText(dialogueText2.transform.GetChild(0).gameObject.GetComponent<Text>(), dialogue, interval,optionsDatas));
                dialogueText1.transform.parent.gameObject.SetActive(false);
                break;
        }
    }

    public float ellipsisInterval;
    public float normalInterval;
    public float midInterval;
    private IEnumerator FadeText(Text text, string dialogue, float interval, List<OptionsData> optionsDatas)
    {
        DialogueManager.Instance.ifAllTalked = false;
        DialogueManager.Instance.ifIntervaled = false;
        currentText = text;
        string outdialogue = string.Empty;
        bool ifcolor = false;
        bool ifsize = false;
        while (outdialogue.Length <= dialogue.Length)
        {
            // 已全部取完，退出循环
            if (outdialogue.Length == dialogue.Length)
            {
                text.text = outdialogue;
                break;
            }
            //去除后缀
            try
            {
                if (outdialogue.Substring(outdialogue.Length - 1 - 6, 1 + 6) == "</size>")
                {
                    ifcolor = true;
                    outdialogue = outdialogue.Substring(0, outdialogue.Length - 1 - 6);
                }
                if (outdialogue.Substring(outdialogue.Length - 1 - 7, 1 + 7) == "</color>")
                {
                    ifsize = true;
                    outdialogue = outdialogue.Substring(0, outdialogue.Length - 1 - 7);
                }
            }
            catch
            { }
            //遇到前缀
            if (dialogue.Substring(outdialogue.Length, 1) == "<" && dialogue.Substring(outdialogue.Length + 1, 1) != "/")
            {
                bool ifture;
                try
                {
                    ifture = checkString(dialogue.Substring(outdialogue.Length, 4));//判断是否是意外
                }
                catch
                { ifture = true; }
                if (!ifture)
                {
                    outdialogue += dialogue.Substring(outdialogue.Length, 2);
                    try
                    {
                        while (!checkString(dialogue.Substring(outdialogue.Length + 1, 1)))
                        {
                            outdialogue += dialogue.Substring(outdialogue.Length, 1);
                        }
                    }
                    catch
                    { }
                    outdialogue += dialogue.Substring(outdialogue.Length, 1);
                    if (outdialogue.Contains("<color"))
                        outdialogue += "</color>";
                    if (outdialogue.Contains("<size"))
                        outdialogue += "</size>";
                }
                else
                {
                    outdialogue += dialogue.Substring(outdialogue.Length, 1);
                }
            }
            else
            {
                try
                {
                    outdialogue += dialogue.Substring(outdialogue.Length, 1);
                    if (dialogue.Substring(outdialogue.Length, 1) == "/")
                    {
                        outdialogue = outdialogue.Substring(0, outdialogue.Length - 1);
                    }
                }
                catch { }
            }
            if (ifcolor)
                outdialogue += "</color>"; ifcolor = false;
            if (ifsize)
                outdialogue += "</size>"; ifsize = false;
            text.text = outdialogue;
            bool ifEllipsis;
            bool ifMid;
            try
            {
                ifEllipsis = dialogue.Substring(outdialogue.Length - 1, 1) == "…" && dialogue.Substring(outdialogue.Length, 1) == "…";
                ifMid= (dialogue.Substring(outdialogue.Length-1,1) == "，")|| (dialogue.Substring(outdialogue.Length-1,1) == "。");
            }
            catch
            {
                continue;
            }
            if (ifEllipsis)
                yield return new WaitForSecondsRealtime(ellipsisInterval);
            else  if(ifMid)
                yield return new WaitForSecondsRealtime(midInterval);
            else
                yield return new WaitForSecondsRealtime(normalInterval);
        }

        yield return new WaitForSecondsRealtime(interval);
        DialogueManager.Instance.ifAllTalked = true;
        if(dialogue==string.Empty)
            DialogueManager.Instance.ifIntervaled = true;

        if(currentOptionDatas.Count!=0&&ifOption)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            panel.gameObject.SetActive(false);
            ifOption = false;
            optionPanel.SetActive(true);
            foreach (var data in currentOptionDatas)
            {
                var optionObj = option.gameObject.transform.GetChild(0).gameObject;
                optionObj.SetActive(true);
                var controller = optionObj.GetComponent<DialogueController>();
                controller.dialogueEmpty = data.nextDialogue;
                controller.dialogueFinish = data.nextDialogue;
                controller.FillDialogueStack();
                optionObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = data.option;
            }
        }
    }

    private void OnEnable()
    {
        EventHandler.ShowDialogueEvent += ShowDialogue;
        EventHandler.SkipCurrentDialogueEvent += OnSkipCurrentDialogueEvent;
    }

    private void OnDisable()
    {
        EventHandler.ShowDialogueEvent -= ShowDialogue;
        EventHandler.SkipCurrentDialogueEvent -= OnSkipCurrentDialogueEvent; ;
    }

    private void OnSkipCurrentDialogueEvent()
    {
        if (fade != null)
        {
            StopCoroutine(fade);
            fade = null;
        }
        currentText.text = currentDialogue;

        StartCoroutine(Spik());
    }

    private IEnumerator Spik()
    {
        yield return new WaitForSecondsRealtime(currentInterval);
        DialogueManager.Instance.ifAllTalked = true;
    }

    public bool isChinese(char c)
    {
        return c >= 0x4E00 && c <= 0x9FA5;
    }

    public bool checkString(string str)
    {
        char[] ch = str.ToCharArray();
        if (str != null)
        {
            for (int i = 0; i < ch.Length; i++)
            {
                if (isChinese(ch[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
