using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Planor : Singleton<Planor>
{
    private bool timeToMove = false;
    private int currentMultiplier;
    
    void Update()
    {
        //if (timeToMove)
        //{
        //transform.Translate(Vector3.forward * 5 * Time.deltaTime);
        //transform.DOMoveZ(transform.position.z+1,1f);
        //transform.DOMoveZ(transform.position.z + 1, 1f);
        if (timeToMove)
        {
            //transform.DOBlendableMoveBy(transform.position + transform.forward, 1f);

        }


        //}
    }

    public void StartMovement()
    {
        timeToMove = true;
    }

    public void StopMovement()
    {
        timeToMove = false;
        MoneyManager.Instance.SaveMoney(currentMultiplier);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Multiplier"))
        {
            currentMultiplier = int.Parse(other.name);
        }
    }
}
