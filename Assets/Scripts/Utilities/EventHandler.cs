using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    public static event Action<int> StartNewGameEvent;
    public static void CallStartNewGameEvent(int gameWeek)
    {
        StartNewGameEvent?.Invoke(gameWeek);
    }

    public static event Action<string,DialogueData.TextDia,float, List<OptionsData>> ShowDialogueEvent;
    public static void CallShowDialogueEvent(string dialogue,DialogueData.TextDia text,float interval, List<OptionsData> optionsDatas)
    {
        ShowDialogueEvent?.Invoke(dialogue,text,interval,optionsDatas);
    }

    public static event Action SkipCurrentDialogueEvent;
    public static void CallSpikCurrentDialogueEvent()
    {
        SkipCurrentDialogueEvent?.Invoke();
    }

    public static event Action<GameState> GameStateChangedEvent;
    public static void CallGameStateChangerEvent(GameState gameState)
    {
        GameStateChangedEvent?.Invoke(gameState);
    }

    public static event Action BeforeSceneChangeEvent;
    public static void CallBeforeSceneChangeEvent()
    {
        BeforeSceneChangeEvent?.Invoke();
    }

    public static event Action AfterSceneChangeEvent;
    public static void CallAfterSceneChangeEvent()
    {
        AfterSceneChangeEvent?.Invoke();
    }

    public static event Action UnEnterAlienationEvent;
    public static void CallUnEnterAlienationChangeEvent()
    {
        UnEnterAlienationEvent?.Invoke();
    }

    public static event Action AlienationEvent;
    public static void CallAlienationEvent()
    {
        AlienationEvent?.Invoke();
    }

    public static event Action ExitAlienationEvent;
    public static void CallExitAlienationEvent()
    {
        ExitAlienationEvent?.Invoke();
    }

    public static event Action DreamValueChangeEvent;
    public static void CallDreamValueChangeEvent()
    {
        DreamValueChangeEvent?.Invoke();
    }

    public static event Action<float> EnterChasingEvent;
    public static void CallEnterChasingEvent(float amount)
    {
        EnterChasingEvent?.Invoke(amount);
    }

    public static event Action<bool> ExitChasingEvent;
    public static void CallExitChasingEvent(bool ifChaseDown)
    {
        ExitChasingEvent?.Invoke(ifChaseDown);
    }
}


