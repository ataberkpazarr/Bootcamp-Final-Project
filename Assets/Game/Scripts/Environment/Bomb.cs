using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
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
    }
}
