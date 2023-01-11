using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Pathfinding;
using System.Security.Cryptography;
using Cainos.PixelArtTopDown_Basic;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float istrack;
    public float speed;
    Rigidbody2D rb;
    public float distance;
    public float timer;
    public float trackSpeed;
    public Path Path;
    Seeker seeker;
    int currentWaypoint=0;
    public float nextWaypointDistance;
    bool reachedEndOfPath=false;    
    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0, 0.5f);
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, player.position, OnPathComplete);
        }
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            Path = p;
            currentWaypoint = 0;
        }
    }
    void Update()
    {
        distance = Mathf.Sqrt(Mathf.Abs(player.transform.position.x - transform.position.x) * Mathf.Abs(player.transform.position.x - transform.position.x) +
        Mathf.Abs(player.transform.position.y - transform.position.y) * Mathf.Abs(player.transform.position.y - transform.position.y));
    }
    private void FixedUpdate()
    {
        timer+=Time.fixedDeltaTime;
        if (distance < istrack)
        {
            OnTrack();
        }
        else
        {
            if (timer > 1f)
            {
                RandomRun();
                timer = 0f;
            }
        }
    }
    void OnTrack()
    {
        if(Path==null)
            return;
        if(currentWaypoint>=Path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)Path.vectorPath[currentWaypoint] - rb.position).normalized;
        rb.velocity = direction * trackSpeed;
        float distance = Vector2.Distance(rb.position, Path.vectorPath[currentWaypoint]);

        if(distance<nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
    void RandomRun()
    {
        Vector2 dir = Vector2.zero;
        if (Random.Range(0,2)==0)
        {
            if(Random.Range(0,2)==0)
            {
                dir.x = 1;
            }
            else
            { 
                dir.x = -1;
            }
            rb.velocity = dir * speed;
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                dir.y = 1;
            }
            else
            {
                dir.y = -1;
            }
            rb.velocity = dir * speed;
        }
    }
}
