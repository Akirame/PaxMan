using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameManager Get()
    {
        return instance;
    }
    private void Awake()
    {
        if(!instance)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    public GameObject PacManPrefab;
    public Transform playerSpawnPoint;
    public PacMan Avatar;
    public int lives;
    public int score;

    // Start is called before the first frame update
    void Start()
    {        
        lives = 3;
        Avatar = GameObject.Instantiate(PacManPrefab).GetComponent<PacMan>();
        Avatar.Respawn(MapManager.Get().playerStartPos);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        //if (MapManager.DotCount == 0)
        //{
        //    return;
        //}
        //else if (lives <= 0)
        //{
        //    return;
        //}

        
    }

    private void GameOver()
    {        
    }

    private void UpdateLives(int v)
    {
        lives += v;
    }

    private void UpdateScore(int scoreGain)
    {
        score += scoreGain;
    }

    public void HandleInput()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public PacMan GetPlayer()
    {
        return Avatar;
    }
    public void GhostDestroyed()
    {
        UpdateScore(50);
    }
    public void PlayerDestroyed()
    {
        UpdateLives(lives - 1);
        if(lives > 0)
        {
            Avatar.Respawn(MapManager.Get().playerStartPos);                        
        }
        else
        {
            GameOver();
            return;
        }
    }
}