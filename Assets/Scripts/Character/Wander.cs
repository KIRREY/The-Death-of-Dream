using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : Character
{
    public Transform targetTransform;
    public Vector3 endPosition;
    Coroutine move;
    public float istrack;
    public float speed;
    Rigidbody2D rb;
    public float distance;
    public float timer;
    public float trackSpeed;
    public Path path;
    Seeker seeker;
    int currentWayPoint = 0;
    public float nextWaypointDistance;
    bool reachedEndOfPath = false;
    public CharacterType characterType;
    Animator animator;

    public float directionChangeInterval;
    float currentAngle = 0;

    private void Start()
    {
        targetTransform = characterType switch
        {
            CharacterType.Player => FindObjectOfType<Player>().transform,
            _ => null
        };
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        animator = GetComponent<Animator>();

        if (move != null)
            StopCoroutine(move);
        move = StartCoroutine(RandomRun());
        InvokeRepeating("UpdatePath", 0, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, targetTransform.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void Update()
    {
        distance = (targetTransform.position - this.gameObject.transform.position).sqrMagnitude;
    }

    private void FixedUpdate()
    {
        if (distance < istrack)
        {
            if (move != null)
                StopCoroutine(move);
            OnTrack();
        }
        else
        {
            if (move != null)
                StopCoroutine(move);
            move = StartCoroutine(RandomRun());
        }
    }

    public void OnTrack()
    {
        if (path == null)
            return;
        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        rb.velocity = direction * trackSpeed;
        AnimatorUpdate(direction); 
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWaypointDistance)
            currentWayPoint++;
    }

    public IEnumerator RandomRun()
    {
        ChooseNewEndPoint();

        float remainingDistance = (transform.position - endPosition).sqrMagnitude;
        while (remainingDistance > float.Epsilon)
        {
            direction = (endPosition - transform.position).normalized;
            rb.velocity = direction * speed;
            AnimatorUpdate(direction);
            remainingDistance = (transform.position - endPosition).sqrMagnitude;
            yield return new WaitForFixedUpdate();
        }

        endPosition = transform.position;
        yield return new WaitForSeconds(directionChangeInterval);
    }

    void ChooseNewEndPoint()
    {
        currentAngle += Random.Range(0, 360);

        currentAngle = Mathf.Repeat(currentAngle, 360);
        endPosition += Vector3FromAngle(currentAngle);
    }

    Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);
    }

    void AnimatorUpdate(Vector2 direction)
    {
        bool isRight = direction.x > 0;
        float Atan = Mathf.Atan(direction.y / direction.x);
        if (isRight&&Atan!=float.NaN)
        {
            if (Atan > Mathf.PI / 4)
                animator.SetInteger("Direction", 1);
            else if (Atan < -Mathf.PI / 4)
                animator.SetInteger("Direction", 0);
            else
                animator.SetInteger("Direction", 2);
        }
        else
        {
            if (Atan > Mathf.PI / 4)
                animator.SetInteger("Direction", 0);
            else if (Atan < -Mathf.PI / 4)
                animator.SetInteger("Direction", 1);
            else
                animator.SetInteger("Direction", 3);
        }
    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        yield return null;  
    }

    public override void ResetCharacter(Direction direction)
    {

    }
}
