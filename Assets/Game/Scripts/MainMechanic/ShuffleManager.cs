using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;

public class ShuffleManager : Singleton<ShuffleManager>
{
    [Header("Sides")]
    [SerializeField] private List<GameObject> leftSideSuitcases;
    [SerializeField] private List<GameObject> rightSideSuitcases;
    [SerializeField] private GameObject leftSideSuitcasesRoot;
    [SerializeField] private GameObject rightSideSuitcasesRoot;
    [Header("Suitcase")]
    [SerializeField] private BoxCollider suitcase;
    private float SuitcaseDeltaPosY => suitcase.size.y;
    [Header("Bomb")]
    [SerializeField] private GameObject bombPrefab;
    [Header("Tween Components")]
    [SerializeField] private Transform tweenParabolaVertex;
    [SerializeField] [Range(0f, 1f)] private float animationSpeed = 0.25f;
    [SerializeField] private GameObject suitcaseItself;

    //[SerializeField] private RightSide sideRight;

    public static Action<Vector3> suitCasesAdded;

    private bool isDragStopoped;
    private bool isLeftDragStopped;
    private bool isRightDragStopped;

    // bug fixers
    private GameObject currentShufflingObject;
    private float currentObjectEulerZ;
    private Sequence leftParabolaSeq, rightPrabolaSeq;
    // for money manager
    private int CurrentSuitcaseAmount => leftSideSuitcases.Count + rightSideSuitcases.Count;

    private bool timeForStair=false;
    private bool activeLeftStair = false;
    private bool activeRightStair = false;


    private void OnEnable()
    {
        SwipeManager.rightSwiped += HandleRightSlide;
        SwipeManager.leftSwiped += HandleLeftSlide;
        SwipeManager.dragStopped += StopShuffle;
        SwipeManager.dragStarted += StartShuffle;
        SwipeManager.leftDragStopped += StopLeftShuffle;
        SwipeManager.rightDragStopped += StopRightShuffle;
        Side.TimeForStair += LetItDoStair;

    }

    private void Start()
    {
        UpdateParabolaVertexPos();
        MoneyManager.Instance.UpdateMoney(CurrentSuitcaseAmount);
    }

    private void OnDisable()
    {
        SwipeManager.rightSwiped -= HandleRightSlide;
        SwipeManager.leftSwiped -= HandleLeftSlide;
        SwipeManager.dragStopped -= StopShuffle;
        SwipeManager.dragStarted -= StartShuffle;
        SwipeManager.leftDragStopped -= StopLeftShuffle;
        SwipeManager.rightDragStopped -= StopRightShuffle;
        Side.TimeForStair -= LetItDoStair;
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

    public int GetTotalAmountOfRightCases()
    {
        return rightSideSuitcases.Count;
    }

    public int GetTotalAmountOfLeftCases()
    {
        return leftSideSuitcases.Count;
    }

    public List<GameObject> GetAllLeftCases()
    {
        return leftSideSuitcases;
    }

    public List<GameObject> GetAllRightCases()
    {
        return rightSideSuitcases;
    }
    private void LetItDoStair(Side side)
    {
        timeForStair = true;
        if (side.transform.position.x<0) //left
        {
            activeLeftStair = true;
        }
        else // right
        {
            activeRightStair = true;
        }
    }

    private void MoveFromLeftToRight()
    {
        //if (leftSideSuitcases.Count > 0 && !isDragStopoped)
        if (leftSideSuitcases.Count > 0 && !isRightDragStopped &&!activeLeftStair)
        {
            rightPrabolaSeq.Kill();

            UpdateParabolaVertexPos();

            if (currentShufflingObject != leftSideSuitcases.Last())// hareket ettirilecek obje havadaki obje değilse
            {
                currentObjectEulerZ = leftSideSuitcases.Last().transform.eulerAngles.z;// hareket ettirilecek objenin ilk Z rotation ını al
            }

            currentShufflingObject = leftSideSuitcases.Last();
            leftSideSuitcases.Remove(currentShufflingObject); // last in first out logic

            // setting the suitcase's new position on the other stack
            Vector3 newPos = rightSideSuitcasesRoot.transform.position;
            newPos.y += SuitcaseDeltaPosY * (rightSideSuitcases.Count + 1);

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


            leftParabolaSeq.Join(currentShufflingObject.transform.DORotate(new Vector3(0, currentShufflingObject.transform.eulerAngles.y, currentObjectEulerZ - 180), animationSpeed * 2)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => { currentShufflingObject?.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f); })
                );


            rightSideSuitcases.Add(currentShufflingObject);
            currentShufflingObject.transform.SetParent(rightSideSuitcasesRoot.transform.parent);// parent'ı Side objesi oldu(bomba olayında gerekliydi)



            
            if (timeForStair)
            {

                leftParabolaSeq.Play().OnComplete(MoveFromLeftToRight).OnStepComplete(() => FixPosition(currentShufflingObject, rightSideSuitcases));

            }
            else
            {
                leftParabolaSeq.Play().OnComplete(MoveFromLeftToRight);

            }
  

        }
    }

    private void FixPosition(GameObject newlyAddedCase,List<GameObject> suitcases)
    {
        if (suitcases.Last().transform.position.y - suitcases[suitcases.Count - 2].transform.position.y > 0.3f)
        {
            newlyAddedCase.transform.DOMoveY(newlyAddedCase.transform.position.y - 0.3f, animationSpeed / suitcases.Count);
        }

    }
 

    public void FixPossiblePositionErrorsAfterBridge(Side side)
    {
        // bu olurken sagdan sola, soldan saga atmaya izin vermiyoruz 
        timeForStair = false;
        activeRightStair = true;
        activeLeftStair = true;

        
        if (side.transform.position.x<0) // left
        {
            
            for (int i = 0; i < leftSideSuitcases.Count; i++)
            {
                if (i==0)
                {
                    GameObject g = leftSideSuitcases[i].gameObject;
                    g.transform.DOMoveY(leftSideSuitcasesRoot.transform.position.y,animationSpeed);

                }
                else
                {
                    GameObject g = leftSideSuitcases[i].gameObject;
                    g.transform.DOMoveY(leftSideSuitcasesRoot.transform.position.y+(i*SuitcaseDeltaPosY), animationSpeed);

                }

            }
        }
        else // right
        {
            for (int i = 0; i < rightSideSuitcases.Count; i++)
            {
                if (i == 0)
                {
                    GameObject g = rightSideSuitcases[i].gameObject;
                    g.transform.DOMoveY(rightSideSuitcasesRoot.transform.position.y, animationSpeed);

                }
                else
                {
                    GameObject g = rightSideSuitcases[i].gameObject;
                    g.transform.DOMoveY(rightSideSuitcasesRoot.transform.position.y + (i * SuitcaseDeltaPosY), animationSpeed);

                }

            }
        }


        activeRightStair = false;
        activeLeftStair = false;



    }

    private void MoveFromRightToLeft()
    {
        //if (rightSideSuitcases.Count > 0 && !isDragStopoped)
        if (rightSideSuitcases.Count > 0 && !isLeftDragStopped &&!activeRightStair)
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
            newPos.y += SuitcaseDeltaPosY * (leftSideSuitcases.Count + 1);

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



            rightPrabolaSeq.Join(currentShufflingObject.transform.DORotate(new Vector3(0, currentShufflingObject.transform.eulerAngles.y, currentObjectEulerZ + 180), animationSpeed * 2)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => { currentShufflingObject?.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f); })
                ); ;


            leftSideSuitcases.Add(currentShufflingObject);
            currentShufflingObject.transform.SetParent(leftSideSuitcasesRoot.transform.parent);
 
            if (timeForStair)
            {
                rightPrabolaSeq.Play().OnComplete(MoveFromRightToLeft).OnStepComplete(() => FixPosition(currentShufflingObject, leftSideSuitcases));

            }
            else
            {
                rightPrabolaSeq.Play().OnComplete(MoveFromRightToLeft);
            }

        }
    }



    private void HandleLeftSlide()
    {
        isLeftDragStopped = false;
        MoveFromRightToLeft();
    }

    private void UpdateParabolaVertexPos()
    {
        int maxLength = Mathf.Max(leftSideSuitcases.Count, rightSideSuitcases.Count);
        float newPosY = maxLength * SuitcaseDeltaPosY + 2.5f;
        Vector3 newParabolaVertex = tweenParabolaVertex.position;
        newParabolaVertex.y = newPosY;
        tweenParabolaVertex.position = newParabolaVertex;
    }

    #region Gate Methods
    // denemede
    public void AddSuitcase(Side side, int suitcaseAmount)
    {
        Vector3 newSuitcasePos;
        if (side.transform.position.x < 0)// left side
        {
            for (int i = 0; i < suitcaseAmount; i++)
            {               
                if (leftSideSuitcases.Count > 0)
                {
                    if (leftSideSuitcases.Last() == currentShufflingObject)// stack'in sonundaki obje havada ise
                    {
                        if (leftSideSuitcases.Count == 1)// eğer stack'te havada olandan başka bir obje yok ise root a göre konumla
                        {
                            newSuitcasePos = leftSideSuitcasesRoot.transform.position;
                            newSuitcasePos.y += SuitcaseDeltaPosY;
                        }
                        else // havada olan objenin bir öncesindekini al
                        {
                            newSuitcasePos = leftSideSuitcases[leftSideSuitcases.Count - 2].transform.position;
                            newSuitcasePos.y += SuitcaseDeltaPosY;
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

                newSuitcasePos.y += SuitcaseDeltaPosY;
                var newSuitcase = ObjectPool.Instance.GetObject(newSuitcasePos);
                newSuitcase.SetActive(true);
                newSuitcase.transform.SetParent(side.transform);
                leftSideSuitcases.Add(newSuitcase);
                //animation
                newSuitcase.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f);
            }

            suitCasesAdded.Invoke(leftSideSuitcases.Last().transform.position);
            /*
            GameObject lastAdded = leftSideSuitcases.Last();
            RectTransform rec = lastAdded.GetComponent<RectTransform>();
            suitCasesAdded.Invoke(rec.position);
            */
        }
        else// right side
        {
            for (int i = 0; i < suitcaseAmount; i++)
            {
                if (rightSideSuitcases.Count > 0)
                {
                    if (rightSideSuitcases.Last() == currentShufflingObject)// stack'in sonundaki obje havada ise
                    {
                        if (rightSideSuitcases.Count == 1)// eğer stack'te havada olandan başka bir obje yok ise root a göre konumla
                        {
                            newSuitcasePos = rightSideSuitcasesRoot.transform.position;
                            newSuitcasePos.y += SuitcaseDeltaPosY;
                        }
                        else // havada olan objenin bir öncesindekini al
                        {
                            newSuitcasePos = rightSideSuitcases[rightSideSuitcases.Count - 2].transform.position;
                            newSuitcasePos.y += SuitcaseDeltaPosY;
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

                newSuitcasePos.y += SuitcaseDeltaPosY;
                var newSuitcase = ObjectPool.Instance.GetObject(newSuitcasePos);
                newSuitcase.SetActive(true);
                newSuitcase.transform.SetParent(side.transform);
                rightSideSuitcases.Add(newSuitcase);
                //animation
                newSuitcase.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f);
            }
            /*
            GameObject lastAdded = rightSideSuitcases.Last();
            RectTransform rec = lastAdded.GetComponent<RectTransform>();
            suitCasesAdded.Invoke(rec.position);
            */

            suitCasesAdded.Invoke(rightSideSuitcases.Last().transform.position);

        }

        MoneyManager.Instance.UpdateMoney(CurrentSuitcaseAmount);
    }

    public void RemoveSuitcase(Side side, int suitcaseAmount)
    {
        if (side.transform.position.x < 0 && leftSideSuitcases.Any())// left side
        {
            int suitcasesOldTopIndex = leftSideSuitcases.Count - 1;
            int suitcasesNewTopIndex = Mathf.Clamp(leftSideSuitcases.Count - suitcaseAmount, 0, int.MaxValue);

            for (int i = suitcasesOldTopIndex; i >= suitcasesNewTopIndex; i--)
            {
                ObjectPool.Instance.PullBackTheObject(leftSideSuitcases[i]);// pool a geri dön
                leftSideSuitcases.RemoveAt(i);
            }
        }
        else if (side.transform.position.x > 0 && rightSideSuitcases.Any())// right side
        {
            int suitcasesOldTopIndex = rightSideSuitcases.Count - 1;
            int suitcasesNewTopIndex = Mathf.Clamp(rightSideSuitcases.Count - suitcaseAmount, 0, int.MaxValue);

            for (int i = suitcasesOldTopIndex; i >= suitcasesNewTopIndex; i--)
            {
                ObjectPool.Instance.PullBackTheObject(rightSideSuitcases[i]);
                rightSideSuitcases.RemoveAt(i);
            }
        }

        MoneyManager.Instance.UpdateMoney(CurrentSuitcaseAmount);
    }


    public void RemoveSuitcaseFromBottomVersion2(Side side)
    {
        if (side.transform.position.x < 0 && leftSideSuitcases.Any())// left side
        {
            ObjectPool.Instance.PullBackTheObject(leftSideSuitcases[0]);// pool a geri dön
            leftSideSuitcases.RemoveAt(0);

            for (int i = 0; i < leftSideSuitcases.Count; i++)
            {
                GameObject g = leftSideSuitcases[i];
                g.transform.DOMoveY(g.transform.position.y - 0.3f, animationSpeed/ leftSideSuitcases.Count);

            }

            FixPossiblePositionErrorsAfterBridge(side);

        }
        else if (side.transform.position.x > 0 && rightSideSuitcases.Any())// right side
        {
            ObjectPool.Instance.PullBackTheObject(rightSideSuitcases[0]);
            rightSideSuitcases.RemoveAt(0);

            for (int i = 0; i < rightSideSuitcases.Count; i++)
            {
                GameObject g = rightSideSuitcases[i];
                g.transform.DOMoveY(g.transform.position.y - 0.3f, animationSpeed/rightSideSuitcases.Count);

            }

            FixPossiblePositionErrorsAfterBridge(side);

        }
        MoneyManager.Instance.UpdateMoney(CurrentSuitcaseAmount);

    }


    #endregion

    #region Bomb Methods
    // normal bir çanta gibi stack'e eklenir
    public void AddBomb(Side side, int bombAmount)
    {
        Vector3 bombPos;
        if (side.transform.position.x < 0)// left side
        {
            for (int i = 0; i < bombAmount; i++)
            {
                if (leftSideSuitcases.Count > 0)
                {
                    if (leftSideSuitcases.Last() == currentShufflingObject)// stack'in sonundaki obje havada ise
                    {
                        if (leftSideSuitcases.Count == 1)// eğer stack'te havada olandan başka bir obje yok ise root a göre konumla
                        {
                            bombPos = leftSideSuitcasesRoot.transform.position;
                            bombPos.y += SuitcaseDeltaPosY;
                        }
                        else // havada olan objenin bir öncesindekini al
                        {
                            bombPos = leftSideSuitcases[leftSideSuitcases.Count - 2].transform.position;
                            bombPos.y += SuitcaseDeltaPosY;
                        }
                    }
                    else
                    {
                        bombPos = leftSideSuitcases.Last().transform.position;
                    }
                }
                else
                {
                    bombPos = leftSideSuitcasesRoot.transform.position;
                }

                bombPos.y += SuitcaseDeltaPosY;
                var bomb = Instantiate(bombPrefab, side.transform);
                bomb.transform.position = bombPos;
                leftSideSuitcases.Add(bomb);
                //animation
                bomb.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f);
            }
        }
        else// right side
        {
            for (int i = 0; i < bombAmount; i++)
            {
                if (rightSideSuitcases.Count > 0)
                {
                    if (rightSideSuitcases.Last() == currentShufflingObject)// stack'in sonundaki obje havada ise
                    {

                        if (rightSideSuitcases.Count == 1)// eğer stack'te havada olandan başka bir obje yok ise root a göre konumla
                        {
                            bombPos = rightSideSuitcasesRoot.transform.position;
                            bombPos.y += SuitcaseDeltaPosY;
                        }
                        else // havada olan objenin bir öncesindekini al
                        {
                            bombPos = rightSideSuitcases[rightSideSuitcases.Count - 2].transform.position;
                            bombPos.y += SuitcaseDeltaPosY;
                        }
                    }
                    else
                    {
                        bombPos = rightSideSuitcases.Last().transform.position;
                    }
                }
                else
                {
                    bombPos = rightSideSuitcasesRoot.transform.position;
                }

                bombPos.y += SuitcaseDeltaPosY;
                var bomb = Instantiate(bombPrefab, side.transform);
                bomb.transform.position = bombPos;
                rightSideSuitcases.Add(bomb);
                //animation
                bomb.transform.DOPunchScale(Vector3.one / 2, 0.25f, 2, 0.5f);
            }
        }
    }

    public void HandleWithBombExplosion(List<GameObject> explodedSuitcases, GameObject bombItself)
    {
        float explosionPosX = bombItself.transform.parent.position.x;

        // handle with bomb itself
        if (explosionPosX < 0)// bomb on the left side
            leftSideSuitcases.Remove(bombItself);
        else if (explosionPosX > 0)
            rightSideSuitcases.Remove(bombItself);
        Destroy(bombItself);
        InGamePanel.Instance.DeactivateTimer();
        // handle with remaining suitcases (candy crush effect)
        // patlamayan çantaları topla
        List<GameObject> unexplodedSuitcasesOnTop = new List<GameObject>();
        if (explosionPosX < 0)// bomb exploded on the left stack
        {
            //patlayan son çantanın indeksini bul
            int highestExplodedIndex = 0;
            foreach (var item in explodedSuitcases)
            {
                if (leftSideSuitcases.IndexOf(item) > highestExplodedIndex)
                {
                    highestExplodedIndex = leftSideSuitcases.IndexOf(item);
                }
            }
            // patlamayanları ekle
            for (int i = highestExplodedIndex + 1; i < leftSideSuitcases.Count; i++)
            {
                unexplodedSuitcasesOnTop.Add(leftSideSuitcases[i]);
            }
        }
        else if (explosionPosX > 0)// bomb exploded ont he right stack
        {
            int highestExplodedIndex = 0;
            foreach (var item in explodedSuitcases)
            {
                if (rightSideSuitcases.IndexOf(item) > highestExplodedIndex)
                {
                    highestExplodedIndex = rightSideSuitcases.IndexOf(item);
                }
            }

            for (int i = highestExplodedIndex + 1; i < rightSideSuitcases.Count; i++)
            {
                unexplodedSuitcasesOnTop.Add(rightSideSuitcases[i]);
            }
        }



        // handle with bomb explosion 
        foreach (var item in explodedSuitcases)
        {
            // remove items from lists (bomb's parent will be one of the sides)
            if (item.transform.parent.position.x < 0)// left side suitcase
            {
                leftSideSuitcases.Remove(item);
            }
            else if (item.transform.parent.position.x > 0)
            {
                rightSideSuitcases.Remove(item);
            }
            ObjectPool.Instance.PullBackTheObject(item);
        }

        // handle with remaining suitcases (candy crush effect)
        // patlamayan çantaları yeniden konumla
        foreach (var item in unexplodedSuitcasesOnTop)
        {
            var unexplodedNewPos = item.transform.position;
            unexplodedNewPos.y -= (explodedSuitcases.Count + 1) * SuitcaseDeltaPosY;
            item.transform.DOMoveY(unexplodedNewPos.y, 0.5f).SetEase(Ease.OutBounce);

        }

        if (explosionPosX < 0)
            StartCoroutine(RefreshPositions(leftSideSuitcases, 0.5f));
        else if (explosionPosX > 0)
            StartCoroutine(RefreshPositions(rightSideSuitcases, 0.5f));

        MoneyManager.Instance.UpdateMoney(CurrentSuitcaseAmount);
    }
    #endregion

    private IEnumerator RefreshPositions(List<GameObject> sideToRefresh, float refreshDelta)
    {
        yield return new WaitForSeconds(refreshDelta);

        for (int i = 0; i < sideToRefresh.Count; i++)
        {
            var refreshedPos = sideToRefresh[i].transform.position;
            refreshedPos.y = leftSideSuitcasesRoot.transform.position.y + SuitcaseDeltaPosY * (i + 1);
            sideToRefresh[i].transform.position = refreshedPos;
        }
    }
}