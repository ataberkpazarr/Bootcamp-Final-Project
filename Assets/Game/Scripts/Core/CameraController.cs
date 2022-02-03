using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cmMenu, cmInGame;

    private void OnEnable()
    {
        GameManager.ActionGameStart += SetInGameCamera;
    }

    public void SetInGameCamera()
    {
        cmMenu.enabled = false;
        cmInGame.enabled = true;
    }

    private void OnDisable()
    {
        GameManager.ActionGameStart -= SetInGameCamera;
    }
}
