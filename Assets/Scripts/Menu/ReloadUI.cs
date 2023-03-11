using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadUI : MonoBehaviour
{
    Coroutine select;
    public GameObject buttonsPanel;
    public GameObject currentButton;
    public Image showShot;
    public Sprite spriteNull;
    public Button button;
    public Image Image;
    public int index;
    public int max;
    private void OnEnable()
    {
        index = 0;
        ReSetShowShot(index);

        max = buttonsPanel.transform.childCount;
        if (select != null)
        {
            select = null;
        }
        select = StartCoroutine(SelectOptions());
    }

    private void OnDisable()
    {
        ReSetShowShot(0);
        showShot.gameObject.SetActive(false);
        if (select != null)
        {
            select = null;
        }
    }

    void ReSetShowShot(int indexR)
    {
        try
        {
            showShot.sprite = SaveLoadManager.Instance.LoadShot(indexR);
        }
        catch
        {
            showShot.sprite = spriteNull;
        }
    }

    IEnumerator SelectOptions()
    {
        yield return new WaitForSeconds(0.1f);
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
                button.SendMessage("Press");
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
            ReSetShowShot(indexF);
        }
        catch { }
    }
}
