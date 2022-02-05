using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject CryParticle;
    [SerializeField] private GameObject HappyParticle;

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

    public void GameOver(Vector3 pos)
    {
        Vector3 particlePos = pos;
        if (pos.x > 0)
        {
            particlePos.x = particlePos.x + 5;
        }
        else
        {
            particlePos.x = particlePos.x - 5;

        }
        Instantiate(CryParticle,particlePos,Quaternion.identity);
        //do game over
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
