using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side : MonoBehaviour
{

    [SerializeField] private GameObject stairPrefabToSpawn;
    private bool TimeToSpawnStairs=false;
    private Vector3 nextTargetPos;
    private int currentChild=0;
    private GameObject currentParentStair;

    private void Update()
    {
        if (TimeToSpawnStairs)
        {
            if (transform.position.z >nextTargetPos.z)
            {
            nextTargetPos = transform.position + new Vector3(0,0,1.3f);
                currentParentStair.transform.GetChild(currentChild).gameObject.SetActive(true) ;
                currentChild++;

            }
            if (currentChild== currentParentStair.transform.childCount-1)
            {
                TimeToSpawnStairs = false;

            }
            
            //StartCoroutine(InstantiateSteps());
        }

        else if (true)
        {

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Gate")
        {
            ShuffleManager.Instance.AddSuitcase(this, 5);
        }

        if(other.name == "NegativeGate")
        {
            ShuffleManager.Instance.RemoveSuitcase(this, 5);
        }

        if (other.name == "BombGate")
        {
            ShuffleManager.Instance.AddBomb(this, 1);
        }
        /*
        else if (other.gameObject.CompareTag("TimeForStairs"))
        {
            TimeToSpawnStairs = true;
        }

        else if (other.gameObject.CompareTag("TimeToEndStairs"))
        {
            TimeToSpawnStairs = false;
        }*/
        else if (other.gameObject.CompareTag("Stair"))
        {
            TimeToSpawnStairs = true;
            nextTargetPos = transform.position + new Vector3(0, 0, 1.5f);
            currentParentStair = other.gameObject;
            currentChild=1;

        }
    }

    private IEnumerator InstantiateSteps()
    {

        yield return new WaitForSeconds(0.5f);
        Instantiate(stairPrefabToSpawn, new Vector3(transform.position.x, -0.2f, transform.position.z), Quaternion.identity);

    }
}
