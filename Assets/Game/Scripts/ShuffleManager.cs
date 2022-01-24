using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShuffleManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> leftSideSuitcases;
    [SerializeField] private List<GameObject> rightSideSuitcases;
    [SerializeField] private GameObject moneyPrefab;

    [SerializeField] private GameObject leftSideSuitcasesRoot;
    [SerializeField] private GameObject rightSideSuitcasesRoot;



    private void OnEnable()
    {
        SwipeManager.rightSwiped += HandleRightSlide;
        SwipeManager.leftSwiped += HandleLeftSlide;

    }

    private void OnDisable()
    {
        SwipeManager.rightSwiped -= HandleRightSlide;
        SwipeManager.leftSwiped -= HandleLeftSlide;
    }

    private void HandleRightSlide()
    {
        if (leftSideSuitcases.Count>0)
        {
            GameObject g = leftSideSuitcases[0];
            leftSideSuitcases.RemoveAt(0); // last in first out logic
            Vector3 newPos;
            if (rightSideSuitcases.Count>0)
            {
                newPos = rightSideSuitcases[0].transform.position;
                newPos.y += 0.3f;
            }
            else
            {
                newPos = rightSideSuitcasesRoot.transform.position;
                newPos.y += 0.3f;
            }
           

            Sequence seq = DOTween.Sequence();
            seq.Append(g.transform.DOMoveX(newPos.x,0.5f));
            seq.Join(g.transform.DOMoveY(newPos.y, 0.5f));
            //g.transform.Rotate(new Vector3(360,360,360)*Time.deltaTime);
            //g.transform.RotateAround(g.transform.position,g.transform.up,360*Time.deltaTime);


            seq.Play();

            if (rightSideSuitcases.Count>0)
            {
                rightSideSuitcases.Insert(0, g); // insert to 0th for last in first out logic

            }
            else
            {
                rightSideSuitcases.Add(g);
            }





        }
    }




    private void HandleLeftSlide()
    {
        if (rightSideSuitcases.Count>0)
        {

            GameObject g = rightSideSuitcases[0];
            rightSideSuitcases.RemoveAt(0); // last in first out logic
          
            Vector3 newPos;
            if (leftSideSuitcases.Count>0)
            {
                newPos = leftSideSuitcases[0].transform.position;
                newPos.y += 0.3f;
            }

            else
            {
                newPos = leftSideSuitcasesRoot.transform.position;
                newPos.y += 0.3f;
            }
           

            Sequence seq = DOTween.Sequence();
            seq.Append(g.transform.DOMoveX(newPos.x, 0.5f));
            seq.Join(g.transform.DOMoveY(newPos.y, 0.5f));

            seq.Play();

            if (leftSideSuitcases.Count>0)
            {
                leftSideSuitcases.Insert(0, g); // insert to 0th for last in first out logic

            }
            else
            {
                leftSideSuitcases.Add(g);
            }


        }
    }
}
