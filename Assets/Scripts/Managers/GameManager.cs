using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager :MonoBehaviourSingleton<GameManager>
{
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
        Avatar.OnDeathAnimationFinished += PlayerDeath;
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
        UIManager.Get().GameOver();
    }
    public void Restart()
    {        
        UpdateScore(-score);
    }
    public void UpdateLives(int v)
    {
        lives = v;
        UIManager.Get().UpdateLives(lives);
    }

    public void UpdateScore(int scoreGain)
    {
        score += scoreGain;
        UIManager.Get().UpdateScore(score);
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
    public void PlayerDeath(PacMan p)
    {
        UpdateLives(lives - 1);
        if(lives > 0)
        {            
            Avatar.Respawn(MapManager.Get().playerStartPos);
            Avatar.Reset();
            EnemyManager.Get().ResetGhosts();
        }
        else
        {
            GameOver();
            return;
        }
    }
}