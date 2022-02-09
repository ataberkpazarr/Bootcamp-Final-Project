using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private CinemachineVirtualCamera cmMenu, cmInGame,cmMiniGame;

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
