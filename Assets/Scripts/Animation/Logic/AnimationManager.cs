using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    public GameObject controller;
    public bool ifPlaying;
    public bool ifIntervaled;
    public Dictionary<string, int> animationIndex = new Dictionary<string, int>();

    private void Update()
    {
        if (ifPlaying)
        {
            if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) && !ifIntervaled)
            {
                if (!controller.activeInHierarchy)
                    EventHandler.CallAnimationEvent(new AnimationDatas());
                AnimationController animationController = controller.GetComponent<AnimationController>();
                animationController.ShowAnimation();
            }
        }
    }
}
