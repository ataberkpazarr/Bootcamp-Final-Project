using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SwipeManager : MonoBehaviour
{
    private bool tap, swipeLeft, swipeRight;
    private bool isDraging = false;
    private Vector2 startTouch, swipeDelta;

    public static Action rightSwiped, leftSwiped, dragStopped, dragStarted, leftDragStopped, rightDragStopped;

    private bool alreadyInvokedRight = false;
    private bool alreadyInvokedLeft = false;
    private bool firstTouch = true;


    private float input;
    private Vector3 mouseRootPos;

    private void OnEnable()
    {
        GameManager.ActionGameOver += DeactivateThis;
        GameManager.ActionArrivedMiniGame += DeactivateThis;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted) return;

        HandleWithInput();
    }

    private void HandleWithInput()
    {
        if (firstTouch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouch = Input.mousePosition;
               
            }
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Reset();
            }

            //Did we cross the distance?
            if (swipeDelta.magnitude > 150)
            {
                firstTouch = false;
                Reset();
            }
            
        }
        else if(!firstTouch)
        { 
            if (Input.GetMouseButton(0))
            {

              if (Input.mousePosition.x > Screen.width / 2.0f && !alreadyInvokedRight) //right
              {
                leftDragStopped?.Invoke();
       
                dragStarted?.Invoke();
                rightSwiped?.Invoke();
                alreadyInvokedRight = true;
                alreadyInvokedLeft = false;

              }

              if (Input.mousePosition.x < Screen.width / 2.0f && !alreadyInvokedLeft) // left
              {
                rightDragStopped?.Invoke();
            
                dragStarted?.Invoke();
                leftSwiped?.Invoke();
                alreadyInvokedLeft = true;
                alreadyInvokedRight = false;
              }
            }
            else if (Input.GetMouseButtonUp(0))
            {
  
               rightDragStopped?.Invoke();
               leftDragStopped?.Invoke();
               alreadyInvokedRight = false;
               alreadyInvokedLeft = false;
                firstTouch = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {        
            rightDragStopped?.Invoke();
            leftDragStopped?.Invoke();
            alreadyInvokedRight = false;
            alreadyInvokedLeft = false;
            firstTouch = true;
        }

    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        //isDraging = false;
    }

    private IEnumerator LeftSwipeInvokeRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        leftSwiped?.Invoke();


    }
    private IEnumerator RightSwipeInvokeRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        rightSwiped?.Invoke();


    }

    private void DeactivateThis()
    {
        this.enabled = false;
    }

    private void OnDisable()
    {
        GameManager.ActionGameOver -= DeactivateThis;
        GameManager.ActionArrivedMiniGame -= DeactivateThis;
    }
}