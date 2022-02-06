using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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

    public void LoadNextLevel()
    {

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
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
