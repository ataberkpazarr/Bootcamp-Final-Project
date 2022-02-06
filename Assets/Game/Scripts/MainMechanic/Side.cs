using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[SelectionBase]
public class Side : MonoBehaviour
{
    [SerializeField] private GameObject stairPrefabToSpawn;
    [Header("Animations & Effects")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject cryParticle;
    [SerializeField] private GameObject happyParticle;

    private bool TimeToSpawnStairs=false;
    private Vector3 nextTargetPos;
    private int currentChild=0;
    private GameObject currentParentStair;

    private bool leftSide;
    public static Action<Side> TimeForStair;

    private void OnEnable()
    {
        GameManager.ActionGameStart += ActivateRunAnim;
        GameManager.ActionGameOver += ActivateGameOverEffects;
    }

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
                        ShuffleManager.Instance.RemoveSuitcaseFromBottomVersion2(this);

                        currentChild++;
                    }
                    else // no case 
                    {                        
                        GameManager.ActionGameOver?.Invoke();
                        TimeToSpawnStairs = false;
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
                        ShuffleManager.Instance.RemoveSuitcaseFromBottomVersion2(this);

                        currentChild++;
                    }
                    else // no case remained
                    {                       
                        GameManager.ActionGameOver?.Invoke();
                        TimeToSpawnStairs = false;
                    }
                }
            }

            if (currentChild == currentParentStair.transform.childCount - 1)
            {
                TimeToSpawnStairs = false;
                StartCoroutine(FixPositionErrorRoutine());
            }   
        }
    }

    private void ActivateRunAnim()
    {
        animator.SetTrigger("Run");
    }

    private void ActivateGameOverEffects()
    {
        if(TimeToSpawnStairs)// falling side
        {
            animator.SetTrigger("TimeToDie");

            Vector3 particlePos = this.transform.position;
            if (particlePos.x > 0)
            {
                particlePos.y += 2;
            }
            else
            {
                particlePos.y += 2;
            }
            Instantiate(cryParticle, particlePos, Quaternion.identity);
        }
        else// standing side
        {
            animator.SetTrigger("OthersFall");
        }
    }

    private IEnumerator FixPositionErrorRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        ShuffleManager.Instance.FixPossiblePositionErrorsAfterBridge(this);


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

    private void OnDisable()
    {
        GameManager.ActionGameStart -= ActivateRunAnim;
        GameManager.ActionGameOver -= ActivateGameOverEffects;
    }
}
