using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerController : MonoBehaviour
{
    [Header("Specs")]
    [SerializeField] private float speed = 5f;
    [Header("Components")]
    [SerializeField] private Animator leftSideAnim;
    [SerializeField] private Animator rightSideAnim;
    // inputs
    private Vector3 mouseRootPos;
    private float input;
    private bool isLevelDone = false;

    private void OnEnable()
    {
        //SwipeManager.rightSwiped += HandleRightSlide;
        //SwipeManager.leftSwiped += HandleLeftSlide;
        GameManager.ActionGameStart += ActivateRunAnim;
        GameManager.ActionGameOver += DeactivateThis;
        FinishLineTrigger.FinishLineTriggered += StopMovement;
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameStarted) return;

        MoveForward();
        //GetSlideInput();
        //HandleSlideInput();
    }

    private void MoveForward()
    {
        if (!isLevelDone)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            GameManager.Instance.CalculateTheProgress(transform.position.z);
        }
       
        //transform.DOMove(transform.position+transform.forward,1f);
    }

    private void ActivateRunAnim()
    {
        leftSideAnim.SetTrigger("Run");
        rightSideAnim.SetTrigger("Run");
    }

    private void StopMovement()
    {

        isLevelDone = true;
        leftSideAnim.SetTrigger("OthersFall");
        rightSideAnim.SetTrigger("OthersFall");
    }

    private void DeactivateThis()
    {
        this.enabled = false;
    }

    private void OnDisable()
    {
        //SwipeManager.rightSwiped -= HandleRightSlide;
        //SwipeManager.leftSwiped -= HandleLeftSlide;
        GameManager.ActionGameStart -= ActivateRunAnim;
        GameManager.ActionGameOver -= DeactivateThis;
        FinishLineTrigger.FinishLineTriggered -= StopMovement;

    }

    private void HandleRightSlide()
    {

    }


    private void HandleLeftSlide()
    {

    }
    /*
    private void GetSlideInput()
    {
        if (Input.GetMouseButtonDown(0))
            mouseRootPos = Input.mousePosition;
        else if (Input.GetMouseButton(0))
        {
            var dragVector = Input.mousePosition - mouseRootPos;
            input = dragVector.x;
        }
        else
            input = 0;
    }

    private void HandleSlideInput()
    {
        if(input > 0)
        {
            var removed = sideLeft.Remove();
            sideRight.Add(removed);
        }
        else if(input < 0)
        {
            var removed = sideRight.Remove();
            sideLeft.Add(removed);
        }
    }*/
}
