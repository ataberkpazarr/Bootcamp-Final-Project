using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private GameObject timerPanel;
    [Header("Specs")]
    [SerializeField] private float explosionTime = 3f;
    [SerializeField] private float explosionRange = 1f;
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

    private void Start()
    {
        StartCoroutine(Explode());
        //TimerPanel.Instance.ActivateTimer();
        InGamePanel.Instance.ActivateTimer();
        Timer.Instance.Begin();

    }

    private IEnumerator Explode()
    {       
        yield return new WaitForSeconds(explosionTime);

        var nearbyObjects = Physics.OverlapSphere(transform.position, explosionRange);
        var nearbySuitcases = new List<GameObject>();

        // alandaki suitcase leri al
        foreach (var item in nearbyObjects)
        {
            if (item.CompareTag("Suitcase") && item.gameObject.activeSelf)
                nearbySuitcases.Add(item.gameObject);
        }

        ShuffleManager.Instance.HandleWithBombExplosion(nearbySuitcases, gameObject);
        Instantiate(explosionParticle,this.transform.position, Quaternion.identity);

    }
}
