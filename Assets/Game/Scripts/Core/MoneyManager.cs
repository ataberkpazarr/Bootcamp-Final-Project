using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    [SerializeField] private int earnPerSuitcase = 1000;
    [SerializeField] private GameObject moneyPrefab;

    private int _currentEarning;
    public int CurrentEarning => _currentEarning;

    private void OnEnable()
    {
        GameManager.ActionLevelPassed += SaveMoney;
    }

    private void Start()
    {
        _currentEarning = 0;
    }

    public void UpdateMoney(int amount)
    {
        _currentEarning = amount * earnPerSuitcase;
        //CanvasController.Instance.UpdateMoneyText(CurrentEarning);
    }

    public void SaveMoney()
    {
        // player prefs
        PlayerPrefs.SetInt("MONEY", CurrentEarning);
    }

    private void OnDisable()
    {
        GameManager.ActionLevelPassed -= SaveMoney;
    }
}
