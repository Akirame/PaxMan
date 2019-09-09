using System;
using UnityEngine;

public class PacMan : MobileEntity
{        
    public Vector2 direction = new Vector2(1, 0);

    void Start()
    {
        speed = 50.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(alive)
        {
            HandleInput();
            MoveToDestination();
        }
    }
    public void HandleInput()
    {
        Vector2 newDirection = direction;
        if(Input.GetKey(KeyCode.UpArrow))
        {
            newDirection = new Vector2(0, 1);
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            newDirection = new Vector2(0, -1);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            newDirection = new Vector2(1, 0);
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            newDirection = new Vector2(-1, 0);
        }
        Move(newDirection);
    }
    public void Move(Vector2 newDirection)
    {
        int nextTileX = GetCurrentTileX() + (int)newDirection.x;
        int nextTileY = GetCurrentTileY() + (int)newDirection.y;
        if(MapManager.Get().TileIsValid(nextTileX, nextTileY))
        {
            SetNextTile(new Vector2Int(nextTileX, nextTileY));
            direction = newDirection;
        }
        else
        {
            nextTileX = GetCurrentTileX() + (int)direction.x;
            nextTileY = GetCurrentTileY() + (int)direction.y;

            if(MapManager.Get().TileIsValid(nextTileX, nextTileY))
            {
                SetNextTile(new Vector2Int(nextTileX, nextTileY));                
            }
        }
    }
}