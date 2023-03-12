using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlienationManager : Singleton<AlienationManager>,ISaveable
{
    public Image mask;
    float originalSize;
    public float timer;
    public float time;
    [Range(0, 100)]
    public float current;
    [Range(0, 100)]
    public float max;

    private void OnEnable()
    {
        EventHandler.AlienationEvent += AlienationAction;
        EventHandler.ExitAlienationEvent += ExitAlienationAction;
        EventHandler.AfterSceneChangeEvent += OnAfterSceneChangeEvent;
    }

    private void OnDisable()
    {
        EventHandler.AlienationEvent -= AlienationAction;
        EventHandler.ExitAlienationEvent -= ExitAlienationAction;
        EventHandler.AfterSceneChangeEvent -= OnAfterSceneChangeEvent;
    }

    private bool isDisplay=false;
    private void OnAfterSceneChangeEvent()
    {
        foreach (var alienation in FindObjectsOfType<Alienation>())
        {
            if (!ifAlienation || alienationLevel == 0)
            {
                if (alienation.alienationLevel == AlienationLevel.Eye)
                    alienation.gameObject.SetActive(isDisplay);
            }
        }
    }

    private void Start()
    {
        originalSize = mask.rectTransform.rect.width;
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }
    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime * 5;
            current = Mathf.Clamp(current + Time.deltaTime * 5 * timer, 0, max);
            Instance.SetValue(current / max);
        }
    }
    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
    public void ChangeHP(float amount)
    {
        time = Mathf.Abs(amount);
        timer = 1;
        if (amount.ToString().Contains("-"))
            timer = -1;
    }

    public AlienationLevel alienationLevel;
    public bool ifAlienation;
    public void CheckAction(Alienation AlienationLevel)
    {
        if (!ifAlienation||(int)alienationLevel<(int)AlienationLevel.alienationLevel)
        {
            EventHandler.CallGameStateChangerEvent(GameState.Pause);
            moveCoroutine= StartCoroutine(PlayerMove(AlienationLevel));
        }
    }

    public Coroutine moveCoroutine;
    public float distanceF;
    public float speed;
    private IEnumerator PlayerMove(Alienation AlienationLevel)
    {
        Vector3 playerPost = FindObjectOfType<Player>().transform.position;
        Vector3 direction = TransformManager.Instance.playerDirection switch
        {
            Direction.Left => new Vector3(1, 0, 0),
            Direction.Right => new Vector3(-1, 0, 0),
            Direction.Up => new Vector3(0, -1, 0),
            Direction.Down => new Vector3(0, 1, 0),
            _ => Vector3.zero
        };
        Player player = FindObjectOfType<Player>();
        float distance2 = float.Epsilon;
        float time = 0;
        while (true)
        {
            float distance = (playerPost - player.transform.position).magnitude;
            time += Time.deltaTime;
            if (time > 0.5f)
            {
                if (distance == distance2)
                {
                    player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    StopCoroutine(PlayerMove(AlienationLevel));
                    break;
                }
                time = 0f;
            }
            if (distance > distanceF)
            {
                player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                StopCoroutine(PlayerMove(AlienationLevel));
                break;
            }
            distance2 = distance;
            player.GetComponent<Rigidbody2D>().velocity = direction * Time.deltaTime * speed;
            yield return new WaitForFixedUpdate();
        }
        EventHandler.CallGameStateChangerEvent(GameState.GamePlay);
        AlienationLevel.OnAlienationChangeEvent();
        moveCoroutine = null;
    }

    public void AlienationAction()
    {
        if (ifAlienation)
            return;
        ifAlienation = true;
        switch (alienationLevel)
        {
            case AlienationLevel.None: ifAlienation = false; break;
            case AlienationLevel.Eye: GameManager.Instance.Timer(60); EyeAction(); break;
            case AlienationLevel.Leg: GameManager.Instance.Timer(50); LegAction(); break;
            case AlienationLevel.Hand:GameManager.Instance.Timer(40); HandAction(); break;
            case AlienationLevel.Brain:GameManager.Instance.Timer(30); BrainAction(); break;
            case AlienationLevel.All:GameManager.Instance.Timer(20); AllAction(); break;
        }
    }

    public void EyeAction()
    {
        GameManager.Instance.globalLight.color = Color.red;
        foreach (var alienation in FindObjectsOfType<Alienation>())
        {
            if (alienation.alienationLevel == AlienationLevel.Eye)
                alienation.gameObject.SetActive(true);
        }
        isDisplay=true;
    }

    public void LegAction()
    {
        EyeAction();
        var player = FindObjectOfType<Player>();
        player.originalspeed = 1.5f * player.originalspeed;
    }

    public void HandAction()
    {
        LegAction();
    }

    public void BrainAction()
    {
        HandAction();
        var player=FindObjectOfType<Player>();
        player.ifBrain = true;
    }

    public void AllAction()
    {
        BrainAction();
        //无敌
    }

    public void ExitAlienationAction()
    {
        switch (alienationLevel)
        {
            case AlienationLevel.None: break;
            case AlienationLevel.Eye: ExitEyeAction(); break;
            case AlienationLevel.Leg: ExitLegAction(); break;
            case AlienationLevel.Hand: ExitHandAction(); break;
            case AlienationLevel.Brain: ExitBrainAction(); break;
            case AlienationLevel.All: ExitAllAction(); break;
        }
    }

    private void ExitEyeAction()
    {
        GameManager.Instance.globalLight.color = Color.black; 
        foreach (var alienation in FindObjectsOfType<Alienation>())
        {
            if (alienation.alienationLevel == AlienationLevel.Eye)
                alienation.gameObject.SetActive(false);
        }
        isDisplay = false;
    }

    private void ExitLegAction()
    {
        ExitEyeAction();
        var player = FindObjectOfType<Player>();
        player.originalspeed = player.originalspeed/1.5f/1.5f;
    }

    private void ExitHandAction()
    {
        ExitLegAction();
        //失手
    }

    private void ExitBrainAction()
    {
        ExitHandAction();
        var player = FindObjectOfType<Player>();
        player.ifBrain = false;
        //进入混乱结局
    }

    private void ExitAllAction()
    {
        //进入同化结局
    }

    public void AlienationReset()
    {
        GameManager.Instance.Timer(0f);
        StartCoroutine(enumerator());
    }

    private IEnumerator enumerator()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.globalLight.color = Color.white;
        var player = FindObjectOfType<Player>();
        player.originalspeed = 3;
    }

    public void AlienationEnd()
    {
        GameManager.Instance.Timer(0f);
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.AlienationLevel = alienationLevel;
        saveData.currentAlienation = current;
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        alienationLevel=saveData.AlienationLevel;
        current=saveData.currentAlienation;
    }
}
