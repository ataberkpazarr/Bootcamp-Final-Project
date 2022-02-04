using UnityEngine;
using TMPro;

public class MoneyManager : Singleton<MoneyManager>
{
    [SerializeField] private int earnPerSuitcase = 1000;
    [SerializeField] private TextMeshProUGUI MoneyText;

    private int _currentEarning;
    public int CurrentEarning => _currentEarning;

    private void Update()
    {
        MoneyText.text = _currentEarning.ToString();

    }
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
