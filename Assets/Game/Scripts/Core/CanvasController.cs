using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : Singleton<CanvasController>
{
    [SerializeField] private GameObject panelMenu, panelInGame, panelEndGame, panelGameOver;

    private void OnEnable()
    {
        GameManager.ActionGameStart += SetInGameUI;
    }

    private void SetInGameUI()
    {
        panelMenu.SetActive(false);
        panelInGame.SetActive(true);
    }

    #region UI Buttons' Methods

    public void ButtonStartPressed()
    {
        GameManager.ActionGameStart?.Invoke();
    }


    #endregion

    private void OnDisable()
    {
        GameManager.ActionGameStart -= SetInGameUI;
    }
}
