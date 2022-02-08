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
    [SerializeField] private GameObject moneyCollectAnim;
    [SerializeField] private GameObject moneyCollectParticle;



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

    public void DoCollectCoinAnimation(Vector3 currentPos)
    {
        /*
        Vector3 worldPosStartingPoint = Camera.main.ScreenToWorldPoint(currentPos);
        GameObject g =Instantiate(moneyPrefab, currentPos, Quaternion.identity);
        RectTransform rec = MoneyImagePos.GetComponent<RectTransform>();
        //Vector3 worldPosTarget = Camera.main.ScreenToWorldPoint(MoneyImagePos.transform.position);
        Vector3 worldPosTarget = Camera.main.ScreenToWorldPoint(rec.position);

        //g.transform.DOMove(worldPosTarget, 3f);
        g.transform.DOMove(rec.position, 3f);
        */
        StartCoroutine(MoneyCollectRoutine());

    }

    private IEnumerator MoneyCollectRoutine()
    {
        moneyCollectAnim.SetActive(true);
        yield return new WaitForSeconds(0.9f);
        moneyCollectAnim.SetActive(false);
        //RectTransform rec = MoneyImagePos.GetComponent<RectTransform>();
        //Instantiate(moneyCollectParticle,rec.position+new Vector3(0,-2,0),Quaternion.identity);


    }



    private void OnEnable()
    {
        ShuffleManager.suitCasesAdded += DoCollectCoinAnimation;
    }

    private void OnDisable()
    {
        ShuffleManager.suitCasesAdded -= DoCollectCoinAnimation;
    }
}
