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

    private void Update()
    {
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
                ////Which direction?
                //float x = swipeDelta.x;
                ////Left or Right
                //if (x < 0)
                //{
                //    leftSwiped.Invoke();
                //    //StartCoroutine(LeftSwipeInvokeRoutine());
                //    swipeLeft = true;
                //}
                //else
                //{
                //    rightSwiped.Invoke();
                //    //StartCoroutine(RightSwipeInvokeRoutine());
                //    swipeRight = true;
                //}
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

}