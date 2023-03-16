using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class Teleport : Interactive
{
    [SceneName] public string sceneFrom;
    [SceneName] public string sceneToGO;
    public Direction direction;
    public Transform playerPos;

    private void Awake()
    {
        try
        {
            playerPos = transform.GetChild(0);
        }
        catch { }
    }

    protected override void OnAction()
    {
        TeleportToScene();
    }

    public override void EmptyAction()
    {
        if (requireItem == ItemName.None)
        {
            TeleportToScene(); return;
        }
        Debug.Log("showempty");
        GetComponent<DialogueController>().ShowDialogueEmpty();
    }

    public void TeleportToScene()
    {
        TransitionManager.Instance.teleport = true;
        TransformManager.Instance.playerDirection = direction;
        TransitionManager.Instance.Transition(sceneFrom, sceneToGO, false);
    }
}
