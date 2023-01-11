using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SceneName] public string sceneFrom;
    [SceneName] public string sceneToGO;
    public Direction direction;

    public void TeleportToScene()
    {
        TransformManager.Instance.playerDirection = direction;
        TransitionManager.Instance.Transition(sceneFrom, sceneToGO, false);
    }
}
