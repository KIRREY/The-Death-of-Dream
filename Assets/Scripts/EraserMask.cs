using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EraserMask : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    //�Ƿ������
    public bool isStartEraser;

    //�Ƿ����������
    public bool isEndEraser;

    //��ʼ�¼�
    public Action eraserStartEvent;

    //�����¼�
    public Action eraserEndEvent;

    public RawImage uiTex;
    Texture2D tex;
    Texture2D MyTex;
    int mWidth;
    int mHeight;

    [Header("Brush Size")]
    public int brushSize = 50;

    [Header("Rate")]
    public int rate = 90;

    float maxColorA;
    float colorA;

    void Awake()
    {
        tex = (Texture2D)uiTex.mainTexture;
        MyTex = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        mWidth = MyTex.width;
        mHeight = MyTex.height;

        MyTex.SetPixels(tex.GetPixels());
        MyTex.Apply();
        uiTex.texture = MyTex;
        maxColorA = MyTex.GetPixels().Length;
        colorA = 0;
        isEndEraser = false;
        isStartEraser = false;

    }


    /// <summary>
    /// ������ƽ��
    /// </summary>
    /// <param name="start">���</param>
    /// <param name="mid">�е�</param>
    /// <param name="end">�յ�</param>
    /// <param name="segments">����</param>
    /// <returns></returns>
    public Vector2[] Beizier(Vector2 start, Vector2 mid, Vector2 end, int segments)
    {
        float d = 1f / segments;
        Vector2[] points = new Vector2[segments - 1];
        for (int i = 0; i < points.Length; i++)
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



    bool startDraw = false;
    bool twoPoints = false;
    Vector2 lastPos;//���һ����
    Vector2 penultPos;//�����ڶ�����
    float radius = 12f;
    float distance = 1f;



    #region �¼�
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isEndEraser) { return; }
        startDraw = true;
        penultPos = eventData.position;
        CheckPoint(penultPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isEndEraser) { return; }
        if (twoPoints && Vector2.Distance(eventData.position, lastPos) > distance)//������μ�¼���������������һ���ľ��룬��ʼ��¼���ĵ�
        {
            Vector2 pos = eventData.position;
            float dis = Vector2.Distance(lastPos, pos);

            CheckPoint(eventData.position);
            int segments = (int)(dis / radius);//�����ƽ���Ķ���
            segments = segments < 1 ? 1 : segments;
            if (segments >= 10) { segments = 10; }
            Vector2[] points = Beizier(penultPos, lastPos, pos, segments);//���б�����ƽ��
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

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isEndEraser) { return; }
        //CheckPoint(eventData.position);
        startDraw = false;
        twoPoints = false;
    }


    #endregion



    void CheckPoint(Vector3 pScreenPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pScreenPos);
        Vector3 localPos = uiTex.gameObject.transform.InverseTransformPoint(worldPos);

        if (localPos.x > -mWidth / 2 && localPos.x < mWidth / 2 && localPos.y > -mHeight / 2 && localPos.y < mHeight / 2)
        {
            for (int i = (int)localPos.x - brushSize; i < (int)localPos.x + brushSize; i++)
            {
                for (int j = (int)localPos.y - brushSize; j < (int)localPos.y + brushSize; j++)
                {
                    if (Mathf.Pow(i - localPos.x, 2) + Mathf.Pow(j - localPos.y, 2) > Mathf.Pow(brushSize, 2))
                        continue;
                    if (i < 0) { if (i < -mWidth / 2) { continue; } }
                    if (i > 0) { if (i > mWidth / 2) { continue; } }
                    if (j < 0) { if (j < -mHeight / 2) { continue; } }
                    if (j > 0) { if (j > mHeight / 2) { continue; } }

                    Color col = MyTex.GetPixel(i + (int)mWidth / 2, j + (int)mHeight / 2);
                    if (col.a != 0f)
                    {
                        col.a = 0.0f;
                        colorA++;
                        MyTex.SetPixel(i + (int)mWidth / 2, j + (int)mHeight / 2, col);
                    }
                }
            }


            //��ʼ�ε�ʱ�� ȥ�жϽ���
            if (!isStartEraser)
            {
                isStartEraser = true;
                InvokeRepeating("getTransparentPercent", 0f, 0.2f);
                if (eraserStartEvent != null)
                    eraserStartEvent.Invoke();
            }

            MyTex.Apply();
        }
    }



    double fate;


    /// <summary>
    /// ��⵱ǰ�ιο� ����
    /// </summary>
    /// <returns></returns>
    public void getTransparentPercent()
    {
        if (isEndEraser) { return; }


        fate = colorA / maxColorA * 100;

        fate = (float)Math.Round(fate, 2);

        // Debug.LogError("��ǰ�ٷֱ�: " + fate);

        if (fate >= rate)
        {
            isEndEraser = true;
            CancelInvoke("getTransparentPercent");
            gameObject.SetActive(false);

            //���������¼�
            if (eraserEndEvent != null)
                eraserEndEvent.Invoke();

        }
    }

}