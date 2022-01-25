using UnityEngine;
using System;

public class SwipeManager : MonoBehaviour
{
    private bool tap, swipeLeft, swipeRight;
    private bool isDraging = false;
    private Vector2 startTouch, swipeDelta;

    public static Action rightSwiped, leftSwiped, dragStopped, dragStarted;


    private void Update()
    {
        HandleWithInput();
    }

    private void HandleWithInput()
    {
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
                Reset();
            }
        }
        #endregion

        //Calculate the distance
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length < 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }

        //Did we cross the distance?
        if (swipeDelta.magnitude > 100)
        {
            //Which direction?
            float x = swipeDelta.x;
            //Left or Right
            if (x < 0)
            {
                swipeLeft = true;
                leftSwiped.Invoke();
            }
            else
            {
                swipeRight = true;
                rightSwiped.Invoke();

            }

            Reset();
        }
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
       
    }
}