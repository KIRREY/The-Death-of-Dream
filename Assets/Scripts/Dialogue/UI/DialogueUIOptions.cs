using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIOptions : Singleton<DialogueUIOptions>
{
    Coroutine select;
    public GameObject OptionsPanel;
    public Button button;
    public Image Image;
    public int index;
    public int max;
    private void OnEnable()
    {
        index = 0;
        max = OptionsPanel.transform.childCount;
        if (select != null)
        {
            select = null;
        }
        select = StartCoroutine(SelectOptions());
    }

    private void OnDisable()
    {
        if (select != null)
        {
            select = null;
        }
    }


    IEnumerator SelectOptions()
    {
        Debug.Log("start");
        Fade(index);
        while (true)
        {
            max = OptionsPanel.transform.childCount;
            if (Input.GetKeyDown(KeyCode.W))
            {
                index = Mathf.Clamp(index - 1, 0, max - 1);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                index = Mathf.Clamp(index + 1, 0, max - 1);
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                button.SendMessage("Press");
            }
            Fade(index);
            yield return null;
        }
    }

    void Fade(int indexF)
    {
        try
        {
            button = OptionsPanel.transform.GetChild(indexF).gameObject.GetComponent<Button>();
            Image = OptionsPanel.transform.GetChild(indexF).gameObject.GetComponent<Image>();
            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, Mathf.PingPong(Time.time, 1));
        }
        catch { }
    }
}
