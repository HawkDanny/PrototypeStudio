using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBirds : MonoBehaviour {

    private lb_BirdController controller;

    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("BirdManager").GetComponent<lb_BirdController>();
        controller.SpawnAmount(500);
    }
}
