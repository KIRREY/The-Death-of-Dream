using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Eraser : MonoBehaviour, IPointerMoveHandler,IPointerEnterHandler
{
    public RawImage rawImage;
    Texture2D texture2D;
    Texture2D myTexture2D;
    int mWidth;
    int mHeight;

    public int burshSize;
    public float rate;

    float maxColorACounts;
    float colorA;

    private void OnEnable()
    {
        InvokeRepeating("getTransparentPercent", 0f, 0.2f);
        rawImage = GetComponent<RawImage>();
        texture2D = (Texture2D)rawImage.mainTexture;
        myTexture2D = new Texture2D(texture2D.width, texture2D.height, TextureFormat.ARGB32, false);
        mWidth = myTexture2D.width;
        mHeight = myTexture2D.height;
        Debug.Log("MWIDTH" + mWidth);
        Debug.Log("MHEIGHT" + mHeight);

        myTexture2D.SetPixels(texture2D.GetPixels());
        myTexture2D.Apply();
        rawImage.texture = myTexture2D;
        maxColorACounts = myTexture2D.GetPixels().Length;
        colorA = 0;
    }

    /// <summary>
    /// 贝塞尔平滑
    /// </summary>
    /// <param name="start">起点</param>
    /// <param name="mid">中点</param>
    /// <param name="end">终点</param>
    /// <param name="segments">段数</param>
    /// <returns></returns>
    public Vector2[] Beizier(Vector2 start, Vector2 mid, Vector2 end, int segments)
    {
        float d = 1f / segments;
        Vector2[] points = new Vector2[segments - 1];

        for (int i = 1; i < points.Length; i++)
        {
            float t = d * (i + 1);
            points[i] = (1 - t) * (1 - t) * mid + 2 * t * (1 - t) * start + t * t * end;
        }
        List<Vector2> rps = new List<Vector2>();
        rps.Add(mid);
        rps.AddRange(points);
        rps.Add(end);
        return rps.ToArray();
    }

    bool twoPoints;
    Vector2 lastPos;
    Vector2 penultPos;
    public float radius;
    public float distance;

    public void OnPointerMove(PointerEventData eventData)
    {
        if (twoPoints && Vector2.Distance(eventData.position, lastPos) > distance)//如果两次记录的鼠标坐标距离大于一定的距离，开始记录鼠标的点
        {
            Vector2 pos = eventData.position;
            float dis = Vector2.Distance(lastPos, pos);

            CheckPoint(eventData.position);
            int segments = (int)(dis / radius);

            segments = segments < 1 ? 1 : segments;
            if (segments >= 10)
                segments = 10;
            if (Vector2.Distance(eventData.position, lastPos) > distance * 5)
            {
                lastPos = eventData.position;   
                return;
            }
            Vector2[] points = Beizier(penultPos, lastPos, pos, segments);
            for (int i = 0; i < points.Length; i++)
            {
                CheckPoint(points[i]);
            }
            lastPos = pos;
            if (points.Length > 2)
                penultPos = points[points.Length - 2];
        }
        else
        {
            twoPoints = true;
            lastPos = eventData.position;
        }
    }

    void CheckPoint(Vector3 pScreenPos)
    {
        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(pScreenPos);
        Vector3 localPos = rawImage.gameObject.transform.InverseTransformPoint(pScreenPos);
        if (localPos.x > -mWidth /2 && localPos.x < mWidth /2 && localPos.y > -mHeight  /2&& localPos.y < mHeight /2)
        {
            for (int i = (int)localPos.x - burshSize; i < (int)localPos.x + burshSize; i++)
            {
                for (int j = (int)localPos.y - burshSize; j < (int)localPos.y + burshSize; j++)
                {
                    if (Mathf.Pow(i - localPos.x, 2) + Mathf.Pow(j-localPos.y, 2) > Mathf.Pow(burshSize, 2))
                        continue;
                    if (i < 0) { if (i < -mWidth/2 ) { continue; } }
                    if (i > 0) { if (i > mWidth /2) { continue; } }
                    if (j < 0) { if (j < -mHeight /2) { continue; } }
                    if (j > 0) { if (j > mHeight /2) { continue; } }
                    Color col = myTexture2D.GetPixel(i +mWidth/2, j +mHeight/2);
                    if (col.a != 0f)
                    {
                        col.a = 0f;
                        colorA++;
                        myTexture2D.SetPixel(i +mWidth/2, j +mHeight/2, col);
                    }
                }
            }
            myTexture2D.Apply();
        }

    }

    float fate;
    /// <summary>
    /// 检测进度
    /// </summary>
    public void getTransparentPercent()
    {
        fate = colorA / maxColorACounts;
        Debug.Log("当前进度："+fate/rate+"%");

        if (fate >= rate)
        {
            CancelInvoke("getTransparentPercent");
            gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        penultPos = eventData.position;
    }
}
