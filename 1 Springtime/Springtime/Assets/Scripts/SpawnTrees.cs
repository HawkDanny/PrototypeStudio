using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnTrees : MonoBehaviour {

    public GameObject[] trees; //Tree prefabs
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        Spawn(1000);
    }

    public void Spawn(int numTrees)
    {
        int currentTreeIndex = 0;
        int currentSpawnAngle = 0;
        float distanceFromPlayer = 15f;

        for (int i = 0; i < numTrees; i++)
        {
            Vector3 spawnLocation = new Vector3(_player.transform.position.x + Mathf.Sin(Mathf.Deg2Rad * currentSpawnAngle) * distanceFromPlayer, 0f, _player.transform.position.z + Mathf.Cos(Mathf.Deg2Rad * currentSpawnAngle) * distanceFromPlayer);
            Vector3 spawnRotation = new Vector3(0f, Random.Range(0f, 360f), 0f);
            GameObject tree = GameObject.Instantiate(trees[currentTreeIndex % trees.Length], spawnLocation, Quaternion.Euler(spawnRotation));

            tree.transform.DOScale(1.5f, currentTreeIndex * 0.05f).SetEase(Ease.InOutQuad);

            currentTreeIndex += Random.Range(1, 4);
            currentSpawnAngle += Random.Range(30, 60);
            distanceFromPlayer += 0.1f;
        }
    }
}
