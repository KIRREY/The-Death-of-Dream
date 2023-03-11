using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadMenu : MonoBehaviour
{
    Coroutine select;
    public GameObject buttonsPanel;
    public GameObject currentButton;
    public Button button;
    public Image Image;
    public int index;
    public int max;
    private void OnEnable()
    {
        index = 0;

        max = buttonsPanel.transform.childCount;
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
        Fade(index);
        while (true)
        {
            max = buttonsPanel.transform.childCount;
            if (Input.GetKeyDown(KeyCode.W))
            {
                index = Mathf.Clamp(index - 1, 0, max - 1);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                index = Mathf.Clamp(index + 1, 0, max - 1);
            }
            Fade(index);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                button.SendMessageUpwards("Press");
            }
            yield return null;
        }
    }

    void Fade(int indexF)
    {
        try
        {
            if (Image != null)
                Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, 1);
            currentButton = buttonsPanel.transform.GetChild(indexF).gameObject;
            button = currentButton.GetComponent<Button>();
            Image = currentButton.GetComponent<Image>();
            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, Mathf.PingPong(Time.time, 1));
        }
        catch { }
    }
}
