using System;
using UnityEngine;

public class PacMan : MobileEntity
{
    public Transform spawnPoint;
    public float speed = 50.0f;
    public Vector2 direction = new Vector2(1, 0);

    void Start()
    {
        transform.position = new Vector2(Map.Get().playerStartPos.posX, Map.Get().playerStartPos.posY) * Map.Get().tileSize;
        currentTileX = Map.Get().playerStartPos.posX;
        currentTileY = Map.Get().playerStartPos.posY;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();        
        Vector2 destination = new Vector2(nextTileX * Map.Get().tileSize, nextTileY * Map.Get().tileSize);
        if(destination == Vector2.zero)
            return;

        Vector2 destinationDirection = new Vector2(destination.x - transform.position.x, destination.y - transform.position.y);

        float distanceToMove = Time.deltaTime * speed;

        if(distanceToMove > destinationDirection.magnitude)
        {
            transform.position = destination;
            currentTileX = nextTileX;
            currentTileY = nextTileY;
        }
        else
        {
            destinationDirection.Normalize();
            SetPosition((Vector2)transform.position + destinationDirection * distanceToMove);
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
        if(Map.Get().TileIsValid(nextTileX, nextTileY))
        {
            SetNextTile(new Vector2Int(nextTileX, nextTileY));
            direction = newDirection;
        }
        else
        {
            nextTileX = GetCurrentTileX() + (int)direction.x;
            nextTileY = GetCurrentTileY() + (int)direction.y;

            if(Map.Get().TileIsValid(nextTileX, nextTileY))
            {
                SetNextTile(new Vector2Int(nextTileX, nextTileY));                
            }
        }
    }
}