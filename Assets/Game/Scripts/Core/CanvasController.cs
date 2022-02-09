using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
public class CanvasController : Singleton<CanvasController>
{
    [SerializeField] private GameObject panelMenu, panelInGame, panelEndGame, panelGameOver;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private Image imageProgressBar;
    [SerializeField] private GameObject progressBar;


    private void OnEnable()
    {
        GameManager.ActionGameStart += SetInGameUI;
        GameManager.ActionGameOver += SetGameOverUI;
    }

    private void Start()
    {
        int currentTotalMoney = PlayerPrefs.GetInt("MONEY", 0);
        UpdateMoneyText(currentTotalMoney);
    }

    private void SetInGameUI()
    {
        panelMenu.SetActive(false);
        panelInGame.SetActive(true);
    }

    private void SetGameOverUI()
    {
        panelInGame.SetActive(false);
        panelGameOver.SetActive(true);

    }

    #region UI Buttons' Methods
    public void ButtonStartPressed()
    {
        GameManager.ActionGameStart?.Invoke();
    }

    public void ButtonNextLevelPressed()
    {
        GameManager.Instance.LoadNextLevel();
    }

    public void ButtonTryAgainPressed()
    {
        GameManager.Instance.RestartLevel();
    }

    public void ButtonSettingsPressed()
    {

    }
    #endregion

    public void UpdateMoneyText(int amount)
    {
        moneyText.text = amount.ToString();
    }

    public void UpdateProgressBar(float value)
    {
        imageProgressBar.fillAmount = value;
    }

    public void DisableProgressBar()
    {

        StartCoroutine(DisableProgressBarRoutine());
    }

    private IEnumerator DisableProgressBarRoutine()
    {
        yield return new WaitForSeconds(0.4f);
        progressBar.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        levelText.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        GameManager.ActionGameStart -= SetInGameUI;
        GameManager.ActionGameOver -= SetGameOverUI;
    }
}
