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

    private void Update()
    {
        HandleWithInput();
    }

    private void HandleWithInput()
    {

        if (Input.GetMouseButton(0))
        {

            //dragStarted.Invoke();

            if (Input.mousePosition.x > Screen.width / 2.0f && !alreadyInvokedRight) //right
            {
                leftDragStopped?.Invoke();
                //dragStopped.Invoke();
                dragStarted?.Invoke();
                rightSwiped?.Invoke();
                alreadyInvokedRight = true;
                alreadyInvokedLeft = false;

            }

            if (Input.mousePosition.x < Screen.width / 2.0f && !alreadyInvokedLeft) // left
            {
                rightDragStopped?.Invoke();
                //dragStopped.Invoke();
                dragStarted?.Invoke();
                leftSwiped?.Invoke();
                alreadyInvokedLeft = true;
                alreadyInvokedRight = false;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //dragStopped?.Invoke();
            rightDragStopped?.Invoke();
            leftDragStopped?.Invoke();
            alreadyInvokedRight = false;
            alreadyInvokedLeft = false;
        }

        /*
        tap = swipeLeft = swipeRight = false;

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            dragStarted.Invoke();
            tap = true;
            isDraging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            dragStopped.Invoke();
            isDraging = false;
            Reset();
        }
        #endregion

        #region Mobile Input
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                //Reset();
            }
        }
        #endregion

        //Calculate the distance
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length < 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
                //startTouch = Input.touches[0].position;
            }
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
                //startTouch = Input.mousePosition;
            }
        }

        //Did we cross the distance?
        if (swipeDelta.magnitude > 150)
        {
            //Which direction?
            float x = swipeDelta.x;
            //Left or Right
            if (x < 0)
            {
                leftSwiped.Invoke();
                //StartCoroutine(LeftSwipeInvokeRoutine());
                swipeLeft = true;
            }
            else
            {
                rightSwiped.Invoke();
                //StartCoroutine(RightSwipeInvokeRoutine());
                swipeRight = true;

            }

            Reset();
        }*/
    }

    private void Reset()
    {
        //startTouch = swipeDelta = Vector2.zero;
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