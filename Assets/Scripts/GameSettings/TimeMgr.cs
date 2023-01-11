using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;               //TimeSpan


public static class TimeMgr
{
    public static float oriT;          //��׼ʱ��
    public static float curT;          //��ǰʱ��    

    public static void SetOriTime()
    {
        float tempT = Time.realtimeSinceStartup;
        oriT = GameManager.Instance.gameTime - tempT;
        SetCurTime();
    }

    public static void SetCurTime()
    {
        //��ǰʱ�䲻Ӧ��С��0
        curT = Mathf.Max(TimeMgr.oriT + Time.realtimeSinceStartup, 0);
        GameManager.Instance.gameTime = curT;
    }


    //������ת��Ϊ00:00:00
    public static string GetFormatTime(int seconds)
    {
        //������תΪʱ����
        TimeSpan ts = new TimeSpan(0, 0, seconds);
        return $"{ts.Hours.ToString("00")}:{ts.Minutes.ToString("00")}:{ts.Seconds.ToString("00")}";
    }

    //��8λ����תΪYYYY/MM/DD
    public static void SetDate(ref string date)
    {
        date = date.Insert(4, "/");
        date = date.Insert(7, "/");
    }

    //��8λʱ��תΪHH:MM:SS
    public static void SetTime(ref string time)
    {
        time = time.Insert(2, ":");
        time = time.Insert(5, ":");
    }


}
