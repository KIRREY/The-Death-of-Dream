using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>, ISaveable
{
    public float gameTime;
    public GameObject timer;
    public GameObject menu;
    public Light2D globalLight;
    public RectTransform nightwareimage;
    public GameState gameState;

    private void Start()
    {
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    private void LateUpdate()
    {
        TimeMgr.SetCurTime();
    }

    private void OnEnable()
    {
        EventHandler.GameStateChangedEvent += OnGameStateChangedEvent;
    }

    private void OnDisable()
    {
        EventHandler.GameStateChangedEvent -= OnGameStateChangedEvent;
    }

    private void OnGameStateChangedEvent(GameState obj)
    {
        gameState = obj;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !DialogueManager.Instance.ifTalking && !DialogueUI.Instance.ifOption && AlienationManager.Instance.moveCoroutine == null)
        {
            if (menu.activeInHierarchy)
            {
                EventHandler.CallGameStateChangerEvent(GameState.GamePlay);
                MenuReset();
            }
            else
                EventHandler.CallGameStateChangerEvent(GameState.Pause);
            menu.SetActive(!menu.activeInHierarchy);
        }
    }

    public void MenuReset()
    {
        menu.transform.GetChild(0).gameObject.SetActive(true);
        menu.transform.GetChild(1).gameObject.SetActive(false);
        menu.transform.GetChild(2).gameObject.SetActive(false);
    }

    Coroutine timeReset;
    public void Timer(float time)
    {
        timer.SetActive(true);
        if (timeReset != null)
        {
            StopCoroutine(timeReset);
            timeReset = null;
        }
        timeReset = StartCoroutine(TimerReset(time));
    }

    private IEnumerator TimerReset(float time)
    {
        Text text = timer.GetComponent<Text>();
        while (time > float.Epsilon)
        {
            if (gameState == GameState.GamePlay)
            {
                text.text = ((int)time).ToString();
                time--;
            }
            yield return new WaitForSecondsRealtime(1);
        }
        timer.SetActive(false);
        AlienationManager.Instance.ifAlienation = false;
        EventHandler.CallExitAlienationEvent();
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData savedata = new GameSaveData();
        savedata.globalLightColorA = globalLight.color.a;
        savedata.globalLightColorR = globalLight.color.r;
        savedata.globalLightColorG = globalLight.color.g;
        savedata.globalLightColorB = globalLight.color.b;
        savedata.gameTime = gameTime;
        return savedata;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        globalLight.color = new Color(saveData.globalLightColorR, saveData.globalLightColorG, saveData.globalLightColorB, saveData.globalLightColorA);
        gameTime = saveData.gameTime;
    }
}
