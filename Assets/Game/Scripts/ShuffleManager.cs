using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class ShuffleManager : Singleton<ShuffleManager>
{
    [Header("Sides")]
    [SerializeField] private List<GameObject> leftSideSuitcases;
    [SerializeField] private List<GameObject> rightSideSuitcases;
    [SerializeField] private GameObject leftSideSuitcasesRoot;
    [SerializeField] private GameObject rightSideSuitcasesRoot;
    [Header("Suitcase")]
    [SerializeField] private BoxCollider suitcase;
    private float suitcaseDeltaPosY => suitcase.size.y;
    [Header("Tween Components")]
    [SerializeField] private Transform tweenParabolaVertex;
    [SerializeField] [Range(0f, 1f)] private float animationSpeed = 0.25f;
    [SerializeField] private LeftSide sideLeft;
    //[SerializeField] private RightSide sideRight;

    private bool isDragStopoped;


    private void OnEnable()
    {
        SwipeManager.rightSwiped += HandleRightSlide;
        SwipeManager.leftSwiped += HandleLeftSlide;
        SwipeManager.dragStopped += StopShuffle;
        SwipeManager.dragStarted += StartShuffle;
    }

    private void Start()
    {
        UpdateParabolaVertexPos();
    }

    private void OnDisable()
    {
        SwipeManager.rightSwiped -= HandleRightSlide;
        SwipeManager.leftSwiped -= HandleLeftSlide;
        SwipeManager.dragStopped -= StopShuffle;
        SwipeManager.dragStarted -= StartShuffle;
    }

    private void StopShuffle()
    {
        isDragStopoped = true;
        Debug.Log("aaa");
    }

    private void StartShuffle()
    {
        isDragStopoped = false;
    }

    private void HandleRightSlide()
    {
        MoveFromLeftToRight();
    }
    

    private void MoveFromLeftToRight()
    {
        if (leftSideSuitcases.Count > 0 && !isDragStopoped)
        {
            UpdateParabolaVertexPos();

            GameObject g = leftSideSuitcases.Last();
            leftSideSuitcases.Remove(g); // last in first out logic

            // setting the suitcase's new position on the other stack
            Vector3 newPos = rightSideSuitcasesRoot.transform.position;
            newPos.y += suitcaseDeltaPosY * (rightSideSuitcases.Count + 1);

            // parabolic transfer animation
            Sequence parabolaSeq = DOTween.Sequence();
            parabolaSeq.Append(g.transform.DOMoveX(tweenParabolaVertex.position.x, animationSpeed)
                .SetEase(Ease.InQuad)
                .OnComplete(() => { g.transform.DOMoveX(newPos.x, animationSpeed).SetEase(Ease.OutQuad); }));
            parabolaSeq.Join(g.transform.DOMoveY(tweenParabolaVertex.position.y, animationSpeed)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => { g.transform.DOMoveY(newPos.y, animationSpeed).SetEase(Ease.InQuad);
                }));
            parabolaSeq.Join(g.transform.DORotate(new Vector3(0, 0, g.transform.rotation.eulerAngles.z - 180), animationSpeed * 2)
                .OnComplete(() => { parabolaSeq.Join(g.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f)); }));
            
            //g.transform.Rotate(new Vector3(360,360,360)*Time.deltaTime);
            //g.transform.RotateAround(g.transform.position,g.transform.up,360*Time.deltaTime);

            rightSideSuitcases.Add(g);

            parabolaSeq.Play().OnComplete(MoveFromLeftToRight);
        }
    }

    private void MoveFromRightToLeft()
    {
        if (rightSideSuitcases.Count > 0 && !isDragStopoped)
        {
            UpdateParabolaVertexPos();

            GameObject g = rightSideSuitcases.Last();
            rightSideSuitcases.Remove(g); // last in first out logic

            // setting the suitcase's new position on the other stack
            Vector3 newPos = leftSideSuitcasesRoot.transform.position;
            newPos.y += suitcaseDeltaPosY * (leftSideSuitcases.Count + 1);

            // parabolic transfer animation
            Sequence parabolaSeq = DOTween.Sequence();
            parabolaSeq.Append(g.transform.DOMoveX(tweenParabolaVertex.position.x, animationSpeed)
                .SetEase(Ease.InQuad)
                .OnComplete(() => { g.transform.DOMoveX(newPos.x, animationSpeed).SetEase(Ease.OutQuad); }));
            parabolaSeq.Join(g.transform.DOMoveY(tweenParabolaVertex.position.y, animationSpeed)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    g.transform.DOMoveY(newPos.y, animationSpeed).SetEase(Ease.InQuad);
                }));
            parabolaSeq.Join(g.transform.DORotate(new Vector3(0, 0, g.transform.rotation.eulerAngles.z + 180), animationSpeed * 2)
                .OnComplete(() => { parabolaSeq.Join(g.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f)); }));

            leftSideSuitcases.Add(g);

            parabolaSeq.Play().OnComplete(MoveFromRightToLeft);
        }
    }

    private void HandleLeftSlide()
    {
        MoveFromRightToLeft();
    }

    private void UpdateParabolaVertexPos()
    {
        int maxLength = Mathf.Max(leftSideSuitcases.Count, rightSideSuitcases.Count);
        float newPosY = maxLength * suitcaseDeltaPosY + 2f;
        Vector3 newParabolaVertex = tweenParabolaVertex.position;
        newParabolaVertex.y = newPosY;
        tweenParabolaVertex.position = newParabolaVertex;
    }
}
