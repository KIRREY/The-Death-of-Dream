using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private string jsonFolder;
    private string jsonPNGFolder;
    private List<ISaveable> saveableList=new List<ISaveable>();
    private Dictionary<string,GameSaveData> saveDataDict=new Dictionary<string,GameSaveData>();
    protected override void Awake()
    {
        base.Awake();
        jsonFolder = Application.persistentDataPath + "/SAVE/";
        jsonPNGFolder = jsonFolder + "/PNG/";
    }

    private void OnEnable()
    {
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    private void OnStartNewGameEvent(int obj)
    {
        var resultPath = jsonFolder + "data.sav";
        if(File.Exists(resultPath))
        {
            File.Delete(resultPath);
        }
        var resultPathPng = jsonPNGFolder + "save.PNG";
        if(File.Exists(resultPathPng))
        {
            File.Delete(resultPathPng);
        }
    }

    public void Register(ISaveable saveable)
    {
        saveableList.Add(saveable);
    }

    public void Save(int index)
    {
        saveDataDict.Clear();

        foreach(var saveable in saveableList)
        {
            saveDataDict.Add(saveable.GetType().Name,saveable.GenerateSaveData());
        }

        var resultPath = jsonFolder + ("data{0}.sav",index);

        var jsonData=JsonConvert.SerializeObject(saveDataDict,Formatting.Indented);

        if(!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        ScreenShot(index,new Rect(0,0,Screen.width,Screen.height));


        File.WriteAllText(resultPath,jsonData);
    }

    public void Load(int index)
    {
        var resultPath = jsonFolder + ("data{0}.sav",index);

        if (!File.Exists(resultPath)) return;

        var stringData=File.ReadAllText(resultPath);

        var jsonData=JsonConvert.DeserializeObject<Dictionary<string,GameSaveData>>(stringData);

        foreach(var saveable in saveableList)
        {
            saveable.RestoreGameData(jsonData[saveable.GetType().Name]);
        }
    }

    private void ScreenShot(int index,Rect rect)
    {
        var resultPath = jsonPNGFolder + ("save{0}.PNG",index);

        int weight = (int)rect.width;
        int height=(int)rect.height;

        RenderTexture rt = new RenderTexture(weight, height, 0);
        Camera.main.targetTexture = rt;
        Camera.main.Render();

        RenderTexture.active = rt;

        Texture2D texture2D = new Texture2D(weight, height, TextureFormat.ARGB32, false);

        texture2D.ReadPixels(rect, 0, 0);
        texture2D.Apply();

        byte[] bytes = texture2D.EncodeToPNG();

        if(!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonPNGFolder);
        }
        File.WriteAllBytes(resultPath, bytes);

        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(rt);
    }

    public Sprite LoadShot(int index)
    {
        var resultPath = jsonPNGFolder + ("save{0}.PNG", index);

        Texture2D t2D = new Texture2D(640, 360);
        t2D.LoadImage(GetImgByte(resultPath));
        return Sprite.Create(t2D, new Rect(0, 0, t2D.width, t2D.height), new Vector2(0.5f, 0.5f));
    }

    static byte[] GetImgByte(string path)
    {
        FileStream s = new FileStream(path, FileMode.Open);
        byte[] imgByte = new byte[s.Length];
        s.Read(imgByte, 0, imgByte.Length);
        s.Close();
        return imgByte;
    }
}

