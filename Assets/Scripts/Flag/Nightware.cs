using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nightware : Flag
{
    public NightwareLevel level;
    public override void FlagEvent()
    {
       switch(level)
        {
            case NightwareLevel.Lv1:StartCoroutine("Lv1");break;
            case NightwareLevel.Lv2:StartCoroutine("Lv2");break;
            case NightwareLevel.Lv3:Lv3();break;
        }
    }

    public float Lv1Up;
    public float Lv1Time;
    public float Lv2Up;
    public float Lv2Time;
    public float Lv3Up;
    public RectTransform image;
    private IEnumerator Lv1()
    {
        float speed= CommunalStart();
        float time=Lv1Time;
        float y = 2160;
        float x = 3840;
        image.anchoredPosition = new Vector2(x, y);
        while (time>float.Epsilon)
        {
            time-=Time.fixedDeltaTime; 
            x=3840*(time/Lv1Time);
            y = 2160*(time/Lv1Time);
            image.anchoredPosition=new Vector2(x, y);
            yield return new WaitForFixedUpdate();
        }
        CommunalEnd(Lv1Up, speed);
    }

    private IEnumerator Lv2()
    {
        float speed=CommunalStart();
        float time=Lv2Time;
        image.anchoredPosition = new Vector2(1920, 1080);
        while (time > float.Epsilon)
        {
            time -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        CommunalEnd(Lv2Up, speed);
    }

    public GameObject NightwarePrefabs;
    private void Lv3()
    {
        NightwarePrefabs.SetActive(true);
        EventHandler.CallEnterChasingEvent(-Lv3Up);
    }

    private float CommunalStart()
    {
        EventHandler.CallGameStateChangerEvent(GameState.Pause);
        var player = FindObjectOfType<Player>();
        float speed = player.originalspeed;
        player.originalspeed = 0;
        GameManager.Instance.nightwareimage.gameObject.SetActive(true);
        image = GameManager.Instance.nightwareimage;
        return speed;
    }

    private void CommunalEnd(float amount,float speed)
    {
        image.gameObject.SetActive(false);
        DreamValueManager.Instance.ChangeHP(-amount);
        var player = FindObjectOfType<Player>();
        player.originalspeed = speed;
        EventHandler.CallGameStateChangerEvent(GameState.GamePlay);
    }
}
