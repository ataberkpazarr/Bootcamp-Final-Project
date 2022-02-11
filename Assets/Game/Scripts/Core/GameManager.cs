using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public static UnityAction ActionGameStart, ActionGameOver, ActionLevelPassed, ActionArrivedMiniGame;
    private bool _isGameStarted;
    private bool timeToMove=false;
    public bool IsGameStarted => _isGameStarted;

    [SerializeField] private Transform platform;
    public float PlatformLenght => platform.localScale.z;
    [SerializeField] private Transform finishPlatformStartingPos;
    [Header("Mini Game")]
    [SerializeField] private GameObject PlayersSeated;
    [SerializeField] private GameObject Stack;
    [SerializeField] private GameObject planor;
    [SerializeField] private GameObject confetti1;
    [SerializeField] private GameObject confetti2;

    List<GameObject> FinalStackList;
    float lastPos;
    float targetPos;

    private void OnEnable()
    {
        ActionGameStart += StartTheGame;
        FinishLineTrigger.FinishLineTriggered += StartMiniGame;
    }

    void Start()
    {
        _isGameStarted = false;
    }

    void Update()
    {
        if (timeToMove)
        {
            Sequence seq = DOTween.Sequence();
            Planor.Instance.StartMovement();
            //float lastPos = planor.transform.position.z;
            //float targetPos = planor.transform.position.z + 1.2f;

            if (FinalStackList.Count > 0)
            {
                //planor.transform.Translate(Vector3.forward * 5 * Time.deltaTime);

                if (targetPos - planor.transform.position.z <= 0)
                {

                    GameObject ga = FinalStackList[0].gameObject;
                    ga.transform.SetParent(null);
                    FinalStackList.RemoveAt(0);
                    targetPos = planor.transform.position.z + 1.2f;
                    planor.transform.DOMoveY(planor.transform.position.y - 0.3f, 0.25f);
                    /*
                    for (int k = 0; k < FinalStackList.Count; k++)
                    {
                        GameObject g = FinalStackList[k];
                        //g.transform.DOMoveY(g.transform.position.y - 0.3f, 0.25f);
                        seq.Append(g.transform.DOMoveY(g.transform.position.y - 0.3f, 0.25f));

                    }
                    seq.Play().OnComplete(() => seq.Kill());
                    */

                }
            }
            else if (FinalStackList.Count<=0)
            {
                timeToMove = false;
                //confetti1.SetActive(true);
                //confetti2.SetActive(true);

            }
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    private void StartTheGame()
    {
        _isGameStarted = true;
    }

    public void CalculateTheProgress(float playerPosZ)
    {
        float currentProgress = playerPosZ / PlatformLenght;
        CanvasController.Instance.UpdateProgressBar(currentProgress);
    }

    #region Mini Game
    private void StartMiniGame()
    {
        FinalStackList = new List<GameObject>();
        List<GameObject> leftSuitCases=ShuffleManager.Instance.GetAllLeftCases();
        List<GameObject> rightSuitCases=ShuffleManager.Instance.GetAllRightCases();
        Sequence seq = DOTween.Sequence();

        for (int i = leftSuitCases.Count-1; i > -1; i--)
        {
            /*
            if (i==leftSuitCases.Count)
            {
                GameObject g = leftSuitCases[i].gameObject;

                seq.Append(g.transform.DOMove(Stack.transform.position + new Vector3(0,FinalStackList.Count*0.3f,0),1f));
                seq.Join(planor.transform.DOMoveY(planor.transform.position.y+0.3f,1f));
                FinalStackList.Add(g);
                leftSuitCases.RemoveAt(i);
                //seq.Append();
            }*/

            GameObject g = leftSuitCases[i].gameObject;

            seq.Append(g.transform.DOMove(Stack.transform.position + new Vector3(0, FinalStackList.Count * 0.3f, 0), 0.1f));
            seq.Join(planor.transform.DOMoveY(planor.transform.position.y + 0.3f*FinalStackList.Count, 0.1f));
            FinalStackList.Add(g);
            //leftSuitCases.RemoveAt(i);
            
        }
        for (int i = rightSuitCases.Count-1; i > -1; i--)
        {
            GameObject g = rightSuitCases[i].gameObject;

            seq.Append(g.transform.DOMove(Stack.transform.position + new Vector3(0, FinalStackList.Count * 0.3f, 0), 0.1f));
            seq.Join(planor.transform.DOMoveY(planor.transform.position.y + 0.3f * FinalStackList.Count, 0.1f));
            FinalStackList.Add(g);

        }
        //CanvasController.Instance.DisableProgressBar();

        seq.Play().OnComplete(()=> CameraController.Instance.SetMiniGameCam()).OnStepComplete(()=>DoMiniGame());

        //PlayersSeated.SetActive(true);

    }

    private void DoMiniGame()
    {
        PlayersSeated.SetActive(true);
        for (int i = 0; i < FinalStackList.Count; i++)
        {
            GameObject g = FinalStackList[i].gameObject;
            g.transform.SetParent(Stack.transform);

        }
        //planor.transform.DOMoveZ(planor.transform.position.z+14,1f);
        planor.transform.DOMoveZ(finishPlatformStartingPos.position.z, 1f).OnComplete(()=>ContinueMiniGame());
    }

    private void ContinueMiniGame()
    {
        /*
        Sequence seq = DOTween.Sequence();
        Planor.Instance.StartMovement();
        float lastPos = planor.transform.position.z;
        float targetPos = planor.transform.position.z + 1.2f;
        */
        timeToMove = true;
        float lastPos = planor.transform.position.z;
        float targetPos = planor.transform.position.z + 1.2f;
        planor.transform.DOMoveZ(planor.transform.position.z + FinalStackList.Count*1.3f, 5f).
            OnComplete(()=> { CameraController.Instance.SetConfettiCamera(); Planor.Instance.StopMovement(); }).OnStepComplete(()=>StartCoroutine(LoadNextLevelRoutine()));

        
        //while (FinalStackList.Count > 0)
        //{
            //planor.transform.Translate(Vector3.forward * 5 * Time.deltaTime);
            /*
            if ( targetPos - planor.transform.position.z  <= 0)
            {
                
                GameObject ga = FinalStackList[0].gameObject;
                ga.transform.SetParent(null);
                FinalStackList.RemoveAt(0);
                targetPos = planor.transform.position.z + 1.2f;
                for (int k = 0; k < FinalStackList.Count; k++)
                {
                    GameObject g = FinalStackList[k];
                    //g.transform.DOMoveY(g.transform.position.y - 0.3f, 0.25f);
                    seq.Append(g.transform.DOMoveY(g.transform.position.y - 0.3f, 0.25f));

                }
                seq.Play().OnComplete(() => seq.Kill());

            }*/

        //}

    }
    #endregion

    private IEnumerator LoadNextLevelRoutine()
    {
        yield return new WaitForSeconds(4f);
        LoadNextLevel();


    }
    private void OnDisable()
    {
        ActionGameStart -= StartTheGame;
        FinishLineTrigger.FinishLineTriggered -= StartMiniGame;

    }
}
