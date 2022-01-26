using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Gate")
        {
            ShuffleManager.Instance.AddSuitcase(this, 5);
        }
    }
}
