using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FinishLineTrigger : MonoBehaviour
{
    public static Action FinishLineTriggered;
    
    

    private void OnTriggerEnter(Collider other)//iki kere çağrılıyor
    {
        if (other.CompareTag("Player"))
        {
            FinishLineTriggered?.Invoke();
        }
    }
}
