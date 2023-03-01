using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SceneName] public string sceneFrom;
    [SceneName] public string sceneToGO;
    public Direction direction;
    public Transform playerPos;

    private void Awake()
    {
        playerPos = transform.GetChild(0);
    }

    public void TeleportToScene()
    {
        TransitionManager.Instance.teleport = true;
        TransformManager.Instance.playerDirection = direction;
        TransitionManager.Instance.Transition(sceneFrom, sceneToGO, false);
    }
}
