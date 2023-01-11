using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : Singleton<Menu>
{
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();                          
        #endif
    }

    public void ContinueGame(int index)
    {
        GameManager.Instance.MenuReset();
        SaveLoadManager.Instance.Load(index);
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }

    public void Save(int index)
    {
        string text = TimeMgr.GetFormatTime((int)GameManager.Instance.gameTime);
        gameObject.transform.GetChild(1).GetChild(index).GetChild(0).gameObject.GetComponent<Text>().text=text;
        gameObject.transform.GetChild(2).GetChild(index).GetChild(0).gameObject.GetComponent<Text>().text = text;
        GameManager.Instance.MenuReset();
        EventHandler.CallBeforeSceneChangeEvent();
        SaveLoadManager.Instance.Save(index);
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }

    public void StartGameWeek()
    {
        EventHandler.CallStartNewGameEvent(0);
    }
}
