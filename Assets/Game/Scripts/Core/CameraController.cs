using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private CinemachineVirtualCamera cmMenu, cmInGame,cmMiniGame, cmConfetti;
    [SerializeField] private GameObject confetti1, confetti2;


    private void OnEnable()
    {
        GameManager.ActionGameStart += SetInGameCamera;
        FinishLineTrigger.FinishLineTriggered += SetMiniGameCamera;
    }

    public void SetInGameCamera()
    {
        cmMenu.enabled = false;
        cmInGame.enabled = true;
    }
    public void SetMiniGameCamera()
    {
        /*
        cmInGame.enabled = false;
        cmMiniGame.enabled = true;
        */
    }

    public void SetConfettiCamera()
    {
        cmMiniGame.enabled = false;

        cmConfetti.enabled = true;
        confetti1.SetActive(true);
        confetti2.SetActive(true);
    }

    public void SetMiniGameCam()
    {
        
        cmInGame.enabled = false;
        cmMiniGame.enabled = true;
        
    }

    private void OnDisable()
    {
        GameManager.ActionGameStart -= SetInGameCamera;
        FinishLineTrigger.FinishLineTriggered -= SetMiniGameCamera;

    }
}
