using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : Singleton<CanvasController>
{
    [SerializeField] private GameObject panelMenu, panelInGame, panelEndGame, panelGameOver;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void OnEnable()
    {
        GameManager.ActionGameStart += SetInGameUI;
        GameManager.ActionGameOver += SetGameOverUI;
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

    private void OnDisable()
    {
        GameManager.ActionGameStart -= SetInGameUI;
        GameManager.ActionGameOver -= SetGameOverUI;
    }
}
