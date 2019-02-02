using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnFlowers : MonoBehaviour {

    public GameObject[] flowers; //flower prefabs
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        Spawn(1000);
    }

    public void Spawn(int numFlowers)
    {
        int currentFlowerIndex = 0;
        int currentSpawnAngle = 0;
        float distanceFromPlayer = 0.2f;

        for (int i = 0; i < numFlowers; i++)
        {
            Vector3 spawnLocation = new Vector3(_player.transform.position.x + Mathf.Sin(Mathf.Deg2Rad * currentSpawnAngle) * distanceFromPlayer, 0.1f, _player.transform.position.z + Mathf.Cos(Mathf.Deg2Rad * currentSpawnAngle) * distanceFromPlayer);
            GameObject flower = GameObject.Instantiate(flowers[currentFlowerIndex % flowers.Length], spawnLocation, Quaternion.identity);

            flower.transform.DOScale(2.5f, currentFlowerIndex * 0.05f).SetEase(Ease.InOutQuad);

            currentFlowerIndex += Random.Range(1, 4);
            currentSpawnAngle += Random.Range(15, 30);
            distanceFromPlayer += 0.02f;
        }
    }
}
