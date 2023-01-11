using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player : Character
{
    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        yield return null;
    }

    public override void ResetCharacter(Direction direction)
    {
        animator.SetInteger("Direction", (int)direction);
        int angle = direction switch
        {
            Direction.Down => 180,
            Direction.Up => 0,
            Direction.Right => 270,
            Direction.Left => 90,
            _ => int.MaxValue,
        };
        flashlight.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public float speed;
    public float originalspeed;
    public GameObject flashlight;
    private Animator animator;
    new public Rigidbody2D rigidbody2D;

    private void Awake()
    {
        originalspeed = speed;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            direction.Set(move.x, move.y);
            direction.Normalize();
        }
        if (!ifPause&&!ifChasing)
        {
            HitCheck();
            FlashControll();
        }
    }

    void FlashControll()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            flashlight.SetActive(!flashlight.activeInHierarchy);
    }

    private bool ifPause;
    private void FixedUpdate()
    {
        if(!ifPause)
            Move();
    }

    public bool ifBrain;
    private float normalize;
    private int animatorDirection;
    public void Move()
    {
        speed = originalspeed;
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 1.5f * speed;
        }

        normalize = 1;
        if (ifBrain)
            normalize = -1;

        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -normalize;
            TransformManager.Instance.playerDirection = Direction.Left;
            flashlight.transform.rotation = Quaternion.Euler(0, 0, 90);
            animatorDirection = normalize switch
            {
                1 => 3,
                -1 => 2,
                _=>3
            };
            animator.SetInteger("Direction", animatorDirection);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = normalize;
            TransformManager.Instance.playerDirection = Direction.Right;
            flashlight.transform.rotation = Quaternion.Euler(0, 0, 270);
            animatorDirection = normalize switch
            {
                1 => 2,
                -1 => 3,
                _ => 2
            };
            animator.SetInteger("Direction", animatorDirection);
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = normalize;
            TransformManager.Instance.playerDirection = Direction.Up;
            flashlight.transform.rotation = Quaternion.Euler(0, 0, 0);
            animatorDirection = normalize switch
            {
                1 => 1,
                -1 => 0,
                _ => 1
            };
            animator.SetInteger("Direction", animatorDirection);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -normalize;
            TransformManager.Instance.playerDirection = Direction.Down;
            flashlight.transform.rotation = Quaternion.Euler(0, 0, 180);
            animatorDirection = normalize switch
            {
                1 => 0,
                -1 => 1,
                _ => 0
            };
            animator.SetInteger("Direction", animatorDirection);
        }

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        rigidbody2D.velocity = speed * dir;
    }

    public override void TriggerEvent(Collider2D collsion)
    {
        switch (collsion.gameObject.tag)
        {
            case "Teleport":
                var teleport = collsion.gameObject.GetComponent<Teleport>();
                teleport?.TeleportToScene();
                break;
            case "Alienation":
                var alienation = collsion.gameObject.GetComponent<Alienation>();
                AlienationManager.Instance.CheckAction(alienation);
                break;
            case "Flag":
                var flag = collsion.gameObject.GetComponent<Flag>();
                flag?.FlagEvent();
                break;
        }
    }

    public float distance;
    public void HitCheck()
    {
        //int layerMask = 7;
        //layerMask = ~layerMask;

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position, direction, distance,LayerMask.GetMask("Default"));
            if (hit.collider != null && hit.collider.gameObject.tag == "Interactive")
            {
                var interactive = hit.collider.gameObject.GetComponent<Interactive>();
                if (holdItem)
                    interactive?.CheckItem(currentItem);
                else
                    interactive?.EmptyAction();
            }
        }
    }

    private void OnEnable()
    {
        EventHandler.GameStateChangedEvent += OnGameStateChangedEvent;
        EventHandler.EnterChasingEvent += OnEnterChasingEvent;
        EventHandler.ExitChasingEvent += OnExitChasingEvent;
    }

    private void OnDisable()
    {
        EventHandler.GameStateChangedEvent -= OnGameStateChangedEvent;
        EventHandler.EnterChasingEvent -= OnEnterChasingEvent;
        EventHandler.ExitChasingEvent -= OnExitChasingEvent;
    }

    public bool ifChasing;
    private void OnExitChasingEvent(bool obj)
    {
        StartCoroutine(ChasingEvent(GameState.GamePlay));
    }

    private void OnEnterChasingEvent(float obj)
    {
        StartCoroutine(ChasingEvent(GameState.Pause));
    }

    IEnumerator ChasingEvent(GameState gameState)
    {
        yield return new WaitForSeconds(0.1f);

        ifChasing = gameState switch
        {
            GameState.GamePlay => false,
            GameState.Pause => true,
            _ => true,
        };
    }

    private void OnGameStateChangedEvent(GameState gameState)
    {
        StartCoroutine(GameStateChanged(gameState));
    }

    IEnumerator GameStateChanged(GameState gameState)
    {
        yield return new WaitForSeconds(0.1f);

        ifPause = gameState switch
        {
            GameState.GamePlay => false,
            GameState.Pause => true,
            _ => true,
        };
    }
}
