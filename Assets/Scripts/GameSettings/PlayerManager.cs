using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>,ISaveable
{
    public bool flashlightActive;
    public float originalspeed;
    public bool ifBrain;
    public bool ifChasing;
    private void OnEnable()
    {
        EventHandler.BeforeSceneChangeEvent += OnBeforeSceneChangeEvent;
        EventHandler.AfterSceneChangeEvent += OnAfterSceneChangeEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneChangeEvent -= OnBeforeSceneChangeEvent;
        EventHandler.AfterSceneChangeEvent -= OnAfterSceneChangeEvent;
    }

    private void OnAfterSceneChangeEvent()
    {
        try
        {
            var player = FindObjectOfType<Player>();
            player.originalspeed = originalspeed;
            player.flashlight.SetActive(flashlightActive);
            player.ifBrain = ifBrain;
            player.ifChasing = ifChasing;
        }
        catch
        { }
    }

    private void OnBeforeSceneChangeEvent()
    {
        try
        {
            var player = FindObjectOfType<Player>();
            originalspeed = player.originalspeed;
            flashlightActive = player.flashlight.activeInHierarchy;
            ifBrain=player.ifBrain;
            ifChasing=player.ifChasing;
        }
        catch
        { }
    }

    private void Start()
    {
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.flashlightActive = flashlightActive;
        saveData.ifBrain = ifBrain;
        saveData.originalspeed = originalspeed;
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        flashlightActive=saveData.flashlightActive;
        ifBrain=saveData.ifBrain;
        originalspeed=saveData.originalspeed;
    }
}
