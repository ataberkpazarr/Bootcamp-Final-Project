using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InGamePanel : Singleton<InGamePanel>
{
    [SerializeField] private GameObject timerPanel;
    [SerializeField] private GameObject MoneyImagePos;
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private GameObject moneyAnimStartingPoint;

    Vector3 moneyAnimStartingPoint_;

    private void Start()
    {
        /*
        RectTransform rec = moneyAnimStartingPoint.GetComponent<RectTransform>();
        moneyAnimStartingPoint_ = rec.position;
        */
        
    }
    public void ActivateTimer()
    {
        timerPanel.SetActive(true);
    }

    public void DeactivateTimer()
    {
        timerPanel.SetActive(false);
    }

    public void CollectCoinAnimation(Vector3 currentPos)
    {
        /*
        GameObject g =Instantiate(moneyPrefab,moneyAnimStartingPoint_,Quaternion.identity);
        RectTransform rec = MoneyImagePos.GetComponent<RectTransform>();
        g.transform.DOMove(rec.position,3f);
        */
    }

    

    private void OnEnable()
    {
        ShuffleManager.suitCasesAdded += CollectCoinAnimation;
    }

    private void OnDisable()
    {
        ShuffleManager.suitCasesAdded -= CollectCoinAnimation;
    }
}
