using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using NaughtyAttributes;

public class ParkourGenerator : MonoBehaviour
{
    [SerializeField] private GameObject parkourPrefab;
    [SerializeField] private Transform sideTile;
    [SerializeField] private int generateCount;
    private Vector3 lastTilePos;
    private float deltaPosZ;
    private List<GameObject> generatedTiles = new List<GameObject>();

#if UNITY_EDITOR
    [Button]
    private void GenerateParkour()
    {
        foreach (var item in generatedTiles)
        {
            DestroyImmediate(item);
        }

        generatedTiles.Clear();

        lastTilePos = parkourPrefab.transform.position;
        deltaPosZ = sideTile.transform.localScale.z;

        for (int i = 0; i < generateCount; i++)
        {
            var newTileToAdd = Instantiate(parkourPrefab, transform);
            print(newTileToAdd.name);
            lastTilePos.z += deltaPosZ;
            newTileToAdd.transform.position = lastTilePos;
            newTileToAdd.transform.rotation = Quaternion.identity;

            generatedTiles.Add(newTileToAdd);
        }
    }

    [Button]
    private void ClearGeneratedParkour()
    {
        foreach (var item in generatedTiles)
        {
            DestroyImmediate(item);
        }

        generatedTiles.Clear();
    }
#endif
}
