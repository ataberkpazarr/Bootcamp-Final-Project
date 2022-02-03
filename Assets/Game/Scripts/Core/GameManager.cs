using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public static UnityAction ActionGameStart, ActionGameOver, ActionLevelPassed;
    private bool _isGameStarted;
    public bool IsGameStarted => _isGameStarted;

    private void OnEnable()
    {
        ActionGameStart += StartTheGame;
    }

    void Start()
    {
        _isGameStarted = false;
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

    private void StartTheGame()
    {
        _isGameStarted = true;
    }

    private void OnDisable()
    {
        ActionGameStart -= StartTheGame;
    }
}
