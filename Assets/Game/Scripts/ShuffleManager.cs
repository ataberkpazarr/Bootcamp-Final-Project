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
    [SerializeField] private GameObject suitcaseItself;

    //[SerializeField] private RightSide sideRight;

    private bool isDragStopoped;
    private bool isLeftDragStopped;
    private bool isRightDragStopped;

    // bug fixers
    private GameObject currentShufflingObject;
    private float currentObjectEulerZ;
    private Sequence leftParabolaSeq, rightPrabolaSeq;


    private void OnEnable()
    {
        SwipeManager.rightSwiped += HandleRightSlide;
        SwipeManager.leftSwiped += HandleLeftSlide;
        SwipeManager.dragStopped += StopShuffle;
        SwipeManager.dragStarted += StartShuffle;
        SwipeManager.leftDragStopped += StopLeftShuffle;
        SwipeManager.rightDragStopped += StopRightShuffle;

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
        //currentShufflingObject = null;
    }

    private void StartShuffle()
    {
        isDragStopoped = false;
    }

    private void HandleRightSlide()
    {
        isRightDragStopped = false;
        MoveFromLeftToRight();
    }

    private void StopLeftShuffle()
    {
        isLeftDragStopped = true;
    }


    private void StopRightShuffle()
    {
        isRightDragStopped = true;
    }


    private void MoveFromLeftToRight()
    {
        //if (leftSideSuitcases.Count > 0 && !isDragStopoped)
        if (leftSideSuitcases.Count > 0 && !isRightDragStopped)
        {
            rightPrabolaSeq.Kill();

            UpdateParabolaVertexPos();

            if(currentShufflingObject != leftSideSuitcases.Last())// hareket ettirilecek obje havadaki obje değilse
            {
                currentObjectEulerZ = leftSideSuitcases.Last().transform.eulerAngles.z;// hareket ettirilecek objenin ilk Z rotation ını al
            }

            currentShufflingObject = leftSideSuitcases.Last();
            leftSideSuitcases.Remove(currentShufflingObject); // last in first out logic

            // setting the suitcase's new position on the other stack
            Vector3 newPos = rightSideSuitcasesRoot.transform.position;
            newPos.y += suitcaseDeltaPosY * (rightSideSuitcases.Count + 1);

            // parabolic transfer animation
            leftParabolaSeq = DOTween.Sequence();
            leftParabolaSeq.Append(currentShufflingObject.transform.DOMoveX(tweenParabolaVertex.position.x, animationSpeed)
                .SetEase(Ease.InQuad)
                .OnComplete(() => { currentShufflingObject?.transform.DOMoveX(newPos.x, animationSpeed).SetEase(Ease.OutQuad); }));
            leftParabolaSeq.Join(currentShufflingObject.transform.DOMoveY(tweenParabolaVertex.position.y, animationSpeed)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    currentShufflingObject?.transform.DOMoveY(newPos.y, animationSpeed).SetEase(Ease.InQuad);
                }));
            leftParabolaSeq.Join(currentShufflingObject.transform.DORotate(new Vector3(0, 0, currentObjectEulerZ - 180), animationSpeed * 2)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => { currentShufflingObject.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f); })
                );
            //.OnComplete(() => { parabolaSeq.Join(g.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f)).OnStepComplete(() => FixScaleAndRotationOfNewJoinedSuitcase(g)); }));
            //.OnComplete(() => { parabolaSeq.Join(g.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f)).OnStepComplete(() => { parabolaSeq.Join(g.transform.DORotateQuaternion(Quaternion.identity, animationSpeed)); }); }));

            //parabolaSeq.Join(g.transform.DORotate(new Vector3(0, 0, g.transform.rotation.eulerAngles.z - 180), animationSpeed * 2)
            //    .OnComplete(() => { parabolaSeq.Join(g.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f)); }));
            /////////parabolaSeq.Join(g.transform.DORotate(new Vector3(0, 0, g.transform.rotation.eulerAngles.z - 180), animationSpeed * 2));
            //.OnComplete(() => { parabolaSeq.Join(g.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f)); }));

            //g.transform.Rotate(new Vector3(360,360,360)*Time.deltaTime);
            //g.transform.RotateAround(g.transform.position,g.transform.up,360*Time.deltaTime);

            rightSideSuitcases.Add(currentShufflingObject);

            leftParabolaSeq.Play().OnComplete(MoveFromLeftToRight);
            //parabolaSeq.Play().OnComplete(() => StartCoroutine(LeftToRightRoutine()));
            //parabolaSeq.Play();


        }
    }

    private void FixScaleAndRotationOfNewJoinedSuitcase(GameObject g, float f)
    {

        //Sequence fixSequence = DOTween.Sequence();
        //fixSequence.Append(g.transform.DOPunchScale(Vector3.one / 2, animationSpeed, 2, 0.5f));
        ////fixSequence.Append(g.transform.DOShakeScale(1f));
        //fixSequence.AppendInterval(0.01f);
        //
        //fixSequence.Join(g.transform.DORotate(new Vector3(0, 0, (f - g.transform.rotation.eulerAngles.z)), animationSpeed * 2.5f));
        //
        //
        //
        //
        ////fixSequence.Join(g.transform.DORotate(new Vector3(0,0,180-g.transform.rotation.eulerAngles.z),animationSpeed*2));
        ////fixSequence.Join(g.transform.DORotate(new Vector3(0,0,180-g.transform.rotation.eulerAngles.z),animationSpeed*2));
        ////fixSequence.Join(g.transform.DORotateQuaternion(Quaternion.identity, animationSpeed*2));
        //
        ////fixSequence.Join(g.transform.DORotateQuaternion(suitcaseItself.transform.rotation, animationSpeed/2));
        //fixSequence.Join(g.transform.DOScale(new Vector3(1, 1, 1), animationSpeed * 2));
        //fixSequence.Play();

    }

    private void MoveFromRightToLeft()
    {
        //if (rightSideSuitcases.Count > 0 && !isDragStopoped)
        if (rightSideSuitcases.Count > 0 && !isLeftDragStopped)
        {
            leftParabolaSeq.Kill();

            UpdateParabolaVertexPos();

            if (currentShufflingObject != rightSideSuitcases.Last())
            {
                currentObjectEulerZ = rightSideSuitcases.Last().transform.eulerAngles.z;
            }

            currentShufflingObject = rightSideSuitcases.Last();
            rightSideSuitcases.Remove(currentShufflingObject); // last in first out logic

            // setting the suitcase's new position on the other stack
            Vector3 newPos = leftSideSuitcasesRoot.transform.position;
            newPos.y += suitcaseDeltaPosY * (leftSideSuitcases.Count + 1);

            // parabolic transfer animation
            rightPrabolaSeq = DOTween.Sequence();
            rightPrabolaSeq.Append(currentShufflingObject.transform.DOMoveX(tweenParabolaVertex.position.x, animationSpeed)
                .SetEase(Ease.InQuad)
                .OnComplete(() => { currentShufflingObject?.transform.DOMoveX(newPos.x, animationSpeed).SetEase(Ease.OutQuad); }));
            rightPrabolaSeq.Join(currentShufflingObject.transform.DOMoveY(tweenParabolaVertex.position.y, animationSpeed)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    currentShufflingObject?.transform.DOMoveY(newPos.y, animationSpeed).SetEase(Ease.InQuad);
                }));
            rightPrabolaSeq.Join(currentShufflingObject.transform.DORotate(new Vector3(0, 0, currentObjectEulerZ + 180), animationSpeed * 2)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => { currentShufflingObject.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f); })
                ); ;
            //.OnComplete(() => { parabolaSeq.Join(g.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f)).OnStepComplete(() => FixScaleAndRotationOfNewJoinedSuitcase(g)); }));
            //.OnComplete(() => { parabolaSeq.Join(g.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f)).OnStepComplete(() => { parabolaSeq.Join(g.transform.DORotateQuaternion(Quaternion.identity, animationSpeed)); }); }));



            //parabolaSeq.Join(g.transform.DORotate(new Vector3(0, 0, g.transform.rotation.eulerAngles.z + 180), animationSpeed * 2)
            //   .OnComplete(() => { parabolaSeq.Join(g.transform.DORotateQuaternion(Quaternion.identity , animationSpeed * 2)); }));
            //.OnComplete(() => { parabolaSeq.Join(g.transform.DORotate(new Vector3(0, 0, g.transform.rotation.eulerAngles.z + 180), animationSpeed * 2)); }));

            leftSideSuitcases.Add(currentShufflingObject);
            //parabolaSeq.Play();
            rightPrabolaSeq.Play().OnComplete(() => {currentShufflingObject = null; MoveFromRightToLeft(); });
            //parabolaSeq.Play().OnComplete(()=>StartCoroutine(RightToLeftRoutine()));
        }
    }

    private IEnumerator RightToLeftRoutine()
    {
        yield return new WaitForSeconds(1f);
        MoveFromRightToLeft();

    }

    private IEnumerator LeftToRightRoutine()
    {
        yield return new WaitForSeconds(1f);
        MoveFromLeftToRight();

    }

    private void HandleLeftSlide()
    {
        isLeftDragStopped = false;
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

    #region Gate Methods
    // denemede
    public void AddSuitcase(Side side, int suitcaseAmount)
    {       
        if(side.transform.position.x < 0)// left side
        {
            for (int i = 0; i < suitcaseAmount; i++)
            {
                Vector3 newSuitcasePos;
                if(leftSideSuitcases.Count > 0)
                {
                    if(leftSideSuitcases.Last() == currentShufflingObject)// stack'in sonundaki obje havada ise
                    {
                        
                        if(leftSideSuitcases.Count == 1)// eğer stack'te havada olandan başka bir obje yok ise root a göre konumla
                        {
                            newSuitcasePos = leftSideSuitcasesRoot.transform.position;
                            newSuitcasePos.y += suitcaseDeltaPosY;
                        }
                        else // havada olan objenin bir öncekisini al
                        {
                            newSuitcasePos = leftSideSuitcases[leftSideSuitcases.Count - 2].transform.position;
                            newSuitcasePos.y += suitcaseDeltaPosY;
                        }
                    }
                    else
                    {
                        newSuitcasePos = leftSideSuitcases.Last().transform.position;
                    }
                }
                else
                {
                    newSuitcasePos = leftSideSuitcasesRoot.transform.position;
                }
                
                newSuitcasePos.y += suitcaseDeltaPosY;
                var newSuitcase = ObjectPool.Instance.GetObject(newSuitcasePos);
                newSuitcase.SetActive(true);
                newSuitcase.transform.SetParent(side.transform.parent);
                leftSideSuitcases.Add(newSuitcase);
                //animation
                newSuitcase.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f);
            }
        }
        else// right side
        {
            for (int i = 0; i < suitcaseAmount; i++)
            {
                Vector3 newSuitcasePos;
                if (rightSideSuitcases.Count > 0)
                {
                    if (rightSideSuitcases.Last() == currentShufflingObject)// stack'in sonundaki obje havada ise
                    {

                        if (rightSideSuitcases.Count == 1)// eğer stack'te havada olandan başka bir obje yok ise root a göre konumla
                        {
                            newSuitcasePos = rightSideSuitcasesRoot.transform.position;
                            newSuitcasePos.y += suitcaseDeltaPosY;
                        }
                        else // havada olan objenin bir öncekisini al
                        {
                            newSuitcasePos = rightSideSuitcases[rightSideSuitcases.Count - 2].transform.position;
                            newSuitcasePos.y += suitcaseDeltaPosY;
                        }
                    }
                    else
                    {
                        newSuitcasePos = rightSideSuitcases.Last().transform.position;
                    }
                }
                else
                {
                    newSuitcasePos = rightSideSuitcasesRoot.transform.position;
                }

                newSuitcasePos.y += suitcaseDeltaPosY;
                var newSuitcase = ObjectPool.Instance.GetObject(newSuitcasePos);
                newSuitcase.SetActive(true);
                newSuitcase.transform.SetParent(side.transform.parent);
                rightSideSuitcases.Add(newSuitcase);
                //animation
                newSuitcase.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f);
            }
        }
    }

    public void RemoveSuitcase(Side side, int suitcaseAmount)
    {
        if (side.transform.position.x < 0)// left side
        {
            int suitcasesOldTopIndex = leftSideSuitcases.Count - 1;
            int suitcasesNewTopIndex = leftSideSuitcases.Count - suitcaseAmount;

            for (int i = suitcasesOldTopIndex; i > suitcasesNewTopIndex; i--)
            {
                leftSideSuitcases[i].SetActive(false);// pool a geri dön
                leftSideSuitcases.RemoveAt(i);
            }
        }
        else// right side
        {
            int suitcasesOldTopIndex = rightSideSuitcases.Count - 1;
            int suitcasesNewTopIndex = rightSideSuitcases.Count - suitcaseAmount;
            
            for (int i = suitcasesOldTopIndex; i > suitcasesNewTopIndex; i--)
            {
                rightSideSuitcases[i].SetActive(false);
                rightSideSuitcases.RemoveAt(i);
            }
        }
    }
    #endregion
}
