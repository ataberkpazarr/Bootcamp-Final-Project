using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    [SerializeField] private int earnPerSuitcase = 1000;
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
        // update InGame ui
    }

    public void SaveMoney()
    {
        // player prefs
    }

    private void OnDisable()
    {
        GameManager.ActionLevelPassed -= SaveMoney;
    }
}
