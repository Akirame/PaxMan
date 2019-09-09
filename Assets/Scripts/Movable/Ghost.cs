using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MobileEntity
{
    public delegate void GhostActions(Ghost g);
    public GhostActions onCollisionWithPlayer;    
    private FSM fsm;
    public enum States
    {
        Idle = 0,
        ExitSpawn,
        Patrol,
        Chase,        
        Vulnerable,
        Dead,
        Count
    }
    public enum Events
    {
        OnActivation,
        OnExited,
        OnSight,
        OffSight,
        OnBigDot,
        OffBigDot,
        OnDeath,
        OnRespawn,
        Count = 0
    }

    public bool isVulnerable;    

    public int desiredMovementX;
    public int desiredMovementY;

    public Behaviour myBehaviour;
    public List<PathmapTile> path = new List<PathmapTile>();

    // Start is called before the first frame update
    void Start()
    {
        fsm = new FSM((int)States.Count, (int)Events.Count, (int)States.Idle);

        fsm.SetRelation((int)States.Idle, (int)Events.OnActivation, (int)States.ExitSpawn);
        fsm.SetRelation((int)States.ExitSpawn, (int)Events.OnExited, (int)States.Patrol);

        fsm.SetRelation((int)States.Patrol, (int)Events.OnBigDot, (int)States.Vulnerable);
        fsm.SetRelation((int)States.Patrol, (int)Events.OnSight, (int)States.Chase);

        fsm.SetRelation((int)States.Chase, (int)Events.OffSight, (int)States.Patrol);
        fsm.SetRelation((int)States.Chase, (int)Events.OnBigDot, (int)States.Vulnerable);

        fsm.SetRelation((int)States.Vulnerable, (int)Events.OffBigDot, (int)States.Patrol);
        fsm.SetRelation((int)States.Vulnerable, (int)Events.OnDeath, (int)States.Dead);

        fsm.SetRelation((int)States.Dead, (int)Events.OnRespawn, (int)States.Idle);


        isVulnerable = false;
        alive = false;
        speed = 30.0f;

        desiredMovementX = 1;
        desiredMovementY = 0;
    }

    // Update is called once per frame
    public void OnUpdate(MapManager MapManager, PacMan avatar)
    {

        switch((States)fsm.GetState())
        {
            case States.Idle:
                Idle();
                break;
            case States.ExitSpawn:
                ExitSpawn();
                break;
            case States.Patrol:
                Patrol();
                break;
            case States.Chase:
                Chase();
                break;
            case States.Vulnerable:
                Vulnerable();
                break;
            case States.Dead:
                Dead();
                break;
        }
        int nextTileX = currentTileX + desiredMovementX;
        int nextTileY = currentTileY + desiredMovementY;

        if(IsAtDestination())
        {
            if(path.Count > 0)
            {
                PathmapTile nextTile = path[0];
                path.RemoveAt(0);
                SetNextTile(new Vector2Int(nextTile.posX, nextTile.posY));
            }
            else if(MapManager.TileIsValid(nextTileX, nextTileY))
            {
                SetNextTile(new Vector2Int(nextTileX, nextTileY));
            }
        }
        MoveToDestination();
    }

    private void Idle()
    {
    }
    private void ExitSpawn()
    {
    }

    private void Patrol()
    {
        System.Random rng = new System.Random();
        MovementDirection nextDirection = (MovementDirection)(rng.Next((int)MovementDirection.DirectionCount));
        switch (nextDirection)
        {
            case MovementDirection.Up:
                desiredMovementX = 0;
                desiredMovementY = 1;
                break;
            case MovementDirection.Down:
                desiredMovementX = 0;
                desiredMovementY = -1;
                break;
            case MovementDirection.Left:
                desiredMovementX = -1;
                desiredMovementY = 0;
                break;
            case MovementDirection.Right:
                desiredMovementX = 1;
                desiredMovementY = 0;
                break;
            default:
                break;
        }
    }

    private void Chase()
    {
        path.Clear();
        path = MapManager.Get().GetPath(currentTileX, currentTileY, GameManager.Get().GetPlayer().currentTileX, GameManager.Get().GetPlayer().currentTileY);
    }

    private void Vulnerable()
    {
        throw new NotImplementedException();
    }

    public void Dead()
    {
        path.Clear();
        path = MapManager.Get().GetPath(currentTileX, currentTileY, 13, 13);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            onCollisionWithPlayer(this);
    }
}
