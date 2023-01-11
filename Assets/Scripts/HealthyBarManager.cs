using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
//血条管理器
public class HealthyBarManager : MonoBehaviour
{
    //单例化血条,获取当前血条本身
    public static HealthyBarManager Instance { get; private set; }
    public Image mask;
    float originalSize;
    float timer;
    float time;
    [Range(0,100)]
    public float current;
    [Range(0,100)]
    public float max;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }
    private void Update()
    {
        if(time>0)
        {
            time-=Time.deltaTime*5;
            current = Mathf.Clamp(current-Time.deltaTime*5, 0, max);
            Instance.SetValue(current / max);
        }
    }
    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize * value);
    }
    public void ChangeHP(float amount)
    {
        time = Mathf.Abs( amount);
        timer = 1;
        if(amount.ToString().Contains("-"))
            timer = -1;
    }
}
