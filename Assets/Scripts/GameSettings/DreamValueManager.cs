using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DreamValueManager : Singleton<DreamValueManager>
{
    public Image mask;
    float originalSize;
    public  float timer;
    public  float time;
    [Range(0, 100)]
    public float current;
    [Range(0, 100)]
    public float max;
    private void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }
    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime * 5;
            current = Mathf.Clamp(current + Time.deltaTime * 5*timer, 0, max);
            Instance.SetValue(current / max);
            if(!ifChasing)
                EventHandler.CallDreamValueChangeEvent();
        }
    }
    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize * value);
    }
    public void ChangeHP(float amount)
    {
        time = Mathf.Abs(amount);
        timer = 1;
        if (amount.ToString().Contains("-"))
            timer = -1;
    }

    private void OnEnable()
    {
        EventHandler.DreamValueChangeEvent += OnDreamValueChangeEvent;
        EventHandler.EnterChasingEvent += OnEnterChasingEvent;
        EventHandler.ExitChasingEvent += OnExitChasingEvent;
    }

    private void OnDisable()
    {
        EventHandler.DreamValueChangeEvent -= OnDreamValueChangeEvent;
        EventHandler.EnterChasingEvent -= OnEnterChasingEvent;
        EventHandler.ExitChasingEvent -= OnExitChasingEvent;
    }

    private void OnExitChasingEvent(bool ifChaseDown)
    {
        ifChasing = false;
        GameManager.Instance.globalLight.color = color;
        if (ChasingCoroutine != null)
        {
            StopCoroutine(ChasingCoroutine);
            ChasingCoroutine = null;
        }
    }

    Coroutine ChasingCoroutine;
    Color color;
    public bool ifChasing;
    private void OnEnterChasingEvent(float amount)
    {
        ifChasing = true;

        if (!AlienationManager.Instance.ifAlienation)
        {
            color = GameManager.Instance.globalLight.color;
            GameManager.Instance.globalLight.color = Color.black;
        }

        if (ChasingCoroutine != null)
        {
            StopCoroutine(ChasingCoroutine);
            ChasingCoroutine = null;
        }
        ChasingCoroutine=StartCoroutine(Chasing(amount));
    }

    public float ChasingChangeTime;
    IEnumerator Chasing(float amount)
    {
        float time=ChasingChangeTime;
        ChangeHP(amount);
        while(time>float.Epsilon)
        {
            time-= time -= Time.deltaTime * 5;
            if (time <= float.Epsilon)
            {
                time = ChasingChangeTime;
                ChangeHP(amount);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDreamValueChangeEvent()
    {
        if (AlienationManager.Instance.ifAlienation)
            return;
        float color =current /max;
        GameManager.Instance.globalLight.color = new Color(color, color, color);
    }


}
