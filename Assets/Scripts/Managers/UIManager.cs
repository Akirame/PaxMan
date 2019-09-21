using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    public GameObject UIGameOver;
    public GameObject UIGameplay;
    public Text PointsText;
    public Image[] livesImages;

    public void UpdateScore(int points)
    {
        PointsText.text = "Points: " + points;
    }
    public void GameOver()
    {
        UIGameOver.SetActive(true);        
    }
    public void ResetUI()
    {
        UIGameOver.SetActive(false);
    }
    public void UpdateLives(int lives)
    {
        foreach(Image liveUI in livesImages)
            liveUI.color = new Color(0, 0, 0, 0);
        for(int i = 0; i < lives; i++)
            livesImages[i].color = Color.white;
    }
}
