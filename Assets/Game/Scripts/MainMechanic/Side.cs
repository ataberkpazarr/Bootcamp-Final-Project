using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[SelectionBase]
public class Side : MonoBehaviour
{
    [SerializeField] private GameObject stairPrefabToSpawn;
    private bool TimeToSpawnStairs=false;
    private Vector3 nextTargetPos;
    private int currentChild=0;
    private GameObject currentParentStair;

    private bool leftSide;

    public static Action<Side> TimeForStair;

    private void Start()
    {
        if (transform.position.x <0) //Left
        {
            leftSide = true;
        }

        else if (transform.position.x>0) //right
        {
            leftSide = false;

        }
    }

    private void Update()
    {
        HandleWithBridge();
    }

    private void HandleWithBridge()
    {
        if (TimeToSpawnStairs)
        {
            if (leftSide)
            {
                if (transform.position.z > nextTargetPos.z)
                {
                    if (ShuffleManager.Instance.GetTotalAmountOfLeftCases() > 0)
                    {
                        nextTargetPos = transform.position + new Vector3(0, 0, 1.3f);
                        GameObject g = currentParentStair.transform.GetChild(currentChild).gameObject;
                        g.SetActive(true);
                        g.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f);
                        //ShuffleManager.Instance.RemoveSuitcaseFromBottom(this);
                        ShuffleManager.Instance.RemoveSuitcaseFromBottomVersion2(this);

                        //StartCoroutine(ShuffleManager.Instance.RemoveSuitcaseRoutine(this, 1));

                        currentChild++;
                    }
                    else // no case 
                    {
                        GameManager.ActionGameOver?.Invoke();

                    }
                }
            }
            else //right side
            {
                if (transform.position.z > nextTargetPos.z)
                {
                    if (ShuffleManager.Instance.GetTotalAmountOfRightCases() > 0)
                    {
                        nextTargetPos = transform.position + new Vector3(0, 0, 1.3f);
                        GameObject g = currentParentStair.transform.GetChild(currentChild).gameObject;
                        g.SetActive(true);
                        g.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f);
                        //ShuffleManager.Instance.RemoveSuitcaseFromBottom(this);
                        ShuffleManager.Instance.RemoveSuitcaseFromBottomVersion2(this);

                        //StartCoroutine(ShuffleManager.Instance.RemoveSuitcaseRoutine(this,1));

                        currentChild++;
                    }
                    else // no case remained
                    {
                        GameManager.ActionGameOver?.Invoke();
                    }
                }
            }



            if (currentChild == currentParentStair.transform.childCount - 1)
            {
                TimeToSpawnStairs = false;
                ShuffleManager.Instance.FixPossiblePositionErrorsAfterBridge(this, 0);

            }

            //StartCoroutine(InstantiateSteps());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            if (other.name == "BombGate")
            {
                ShuffleManager.Instance.AddBomb(this, 1);
                return;
            }

            int amount = int.Parse(other.name);
            if (amount > 0)
                ShuffleManager.Instance.AddSuitcase(this, amount);
            else
                ShuffleManager.Instance.RemoveSuitcase(this, -amount);
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
            TimeForStair.Invoke(this);

        }
    }

    private IEnumerator InstantiateSteps()
    {

        yield return new WaitForSeconds(0.5f);
        Instantiate(stairPrefabToSpawn, new Vector3(transform.position.x, -0.2f, transform.position.z), Quaternion.identity);

    }
}
