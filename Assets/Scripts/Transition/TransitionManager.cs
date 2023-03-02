using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>, ISaveable
{
    [SceneName] public string startScene;
    public string lastScene;

    public CanvasGroup fadeCanvasGroup;
    public CinemachineVirtualCamera virtualCamera;
    public float fadeDuration;
    public bool teleport;
    private bool isFade;
    private bool canTransition = true;
    private bool isStart;

    private void Start()
    {
        isStart = true;
        teleport=false;
        StartCoroutine(TransitionToScene("Persistent", startScene, false));
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    public void Transition(string from, string to, bool ifNow)
    {
        if (!isFade && canTransition)
            StartCoroutine(TransitionToScene(from, to, ifNow));
    }

    private IEnumerator TransitionToScene(string from, string to, bool ifNow)
    {
        lastScene = SceneManager.GetActiveScene().name;
        if (!isStart)
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (!ifNow)
            EventHandler.CallBeforeSceneChangeEvent();

        yield return Fade(1f, 3.5f);
        isStart = false;
        yield return new WaitForSeconds(0.1f);

        if (from != string.Empty || from == "Persistent")
        {
            yield return SceneManager.UnloadSceneAsync(from);
        }
        yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);

        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
        if(teleport)
        {
            var player = FindObjectOfType<Player>();
            string target="Teleport to "+lastScene;
            player.transform.position = GameObject.Find(target).GetComponent<Teleport>().playerPos.position;
        }
        teleport = false;

        EventHandler.CallAfterSceneChangeEvent();

        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCamera.m_Lens.OrthographicSize = 3;

        yield return Fade(0, 5.4f);
    }

    /// <summary>
    /// ÇÐ»»³¡¾°
    /// </summary>
    /// <param name="targetAlpha"></param>
    /// <returns></returns>
    private IEnumerator Fade(float targetAlpha, float targetScale)
    {
        EventHandler.CallGameStateChangerEvent(GameState.Pause);
        isFade = true;

        fadeCanvasGroup.blocksRaycasts = true;
        float speedFade = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;
        float speedScale = 0;
        if (!isStart)
        {
            speedScale = Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - targetScale) / fadeDuration;
        }

        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speedFade * Time.deltaTime);
            if (!isStart)
                virtualCamera.m_Lens.OrthographicSize = Mathf.MoveTowards(virtualCamera.m_Lens.OrthographicSize, targetScale, speedScale * Time.deltaTime);
            yield return null;
        }

        if (!isStart)
        {
            while (!Mathf.Approximately(virtualCamera.m_Lens.OrthographicSize, targetScale))
            {
                virtualCamera.m_Lens.OrthographicSize = Mathf.MoveTowards(virtualCamera.m_Lens.OrthographicSize, targetScale, speedScale * Time.deltaTime);
                yield return null;
            }
            virtualCamera.m_Lens.OrthographicSize = targetScale;
        }

        fadeCanvasGroup.blocksRaycasts = false;

        isFade = false;
        EventHandler.CallGameStateChangerEvent(GameState.GamePlay);
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.currentScene = SceneManager.GetActiveScene().name;
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        bool ifNow = SceneManager.GetActiveScene().name == saveData.currentScene;
        Transition(SceneManager.GetActiveScene().name, saveData.currentScene, ifNow);
    }
}
