using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Timer : Singleton<Timer>
{
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private GameObject player;



    public int duration;

    private int remainingDuration;
    

    // Start is called before the first frame update
    void Start()
    {
        
        //Begin(duration);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void Begin()
    {
        
        remainingDuration = duration;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration>=0)
        {
            uiText.gameObject.SetActive(true);

            uiText.text =remainingDuration.ToString();

            uiFill.fillAmount = Mathf.InverseLerp(0,duration,remainingDuration);
            remainingDuration--;
            yield return new WaitForSeconds(0.8f);
            uiText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.15f);


        }
        OnEnd();
    }

    private void OnEnd()
    {
        //GameObject g = transform.GetChild(2).gameObject;
        //g.SetActive(true);
        //this.gameObject.SetActive(false);
        //InGamePanel.Instance.DeactivateTimer();
        //Instantiate(explosionParticle,player.transform.position + new Vector3(0,3,0),Quaternion.identity);
    }

 
}
