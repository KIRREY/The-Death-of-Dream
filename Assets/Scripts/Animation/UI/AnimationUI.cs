using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AnimationUI : MonoBehaviour
{
    public GameObject panel;
    public GameObject animationPlayer;
    RawImage animationRawImage;
    Coroutine animationCoroutine;
    double animationTime, currentTime;

    private void OnEnable()
    {
        EventHandler.AnimationEvent += ShowAnimation;
    }

    private void OnDisable()
    {
        EventHandler.AnimationEvent -= ShowAnimation;
    }

    private void ReSet()
    {
        animationTime = animationPlayer.GetComponent<VideoPlayer>().clip.length;
        animationRawImage=animationPlayer.GetComponent<RawImage>(); 
    }

    private void ShowAnimation(AnimationDatas texture2D)
    {
        try
        {
            ReSet();
        }
        catch 
        {
            return;
        }
        if(texture2D!=null)
        {
            panel.gameObject.SetActive(true);
            animationRawImage.texture = texture2D.animation;
            if(animationCoroutine!=null)
                animationCoroutine=null;
            animationCoroutine = StartCoroutine(Playing());
        }
        else
        {
            panel.gameObject.SetActive(false);
            if (animationCoroutine != null)
                animationCoroutine = null;
            EventHandler.CallGameStateChangerEvent(GameState.GamePlay);
        }
    }

    IEnumerator Playing()
    {
        AnimationManager.Instance.ifIntervaled=true;
        while(animationTime>=currentTime)
        {
            currentTime+=Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        AnimationManager.Instance.ifIntervaled = false;
    }
}
