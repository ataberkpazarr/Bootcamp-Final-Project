using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Specs")]
    [SerializeField] private float speed = 5f;
    [Header("Sides")]
    [SerializeField] private Side sideLeft, sideRight;
    // inputs
    private Vector3 mouseRootPos;
    private float input;


    void Update()
    {
        MoveForward();
        GetSlideInput();
        HandleSlideInput();
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

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
    }
}
