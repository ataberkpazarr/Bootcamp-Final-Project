using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public static UnityAction ActionGameStart, ActionGameOver, ActionLevelPassed;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void GameOver()
    {

        //do game over
    }

    public void LoadNextLevel()
    {

    }

    public void RestartLevel()
    {

    }
}
