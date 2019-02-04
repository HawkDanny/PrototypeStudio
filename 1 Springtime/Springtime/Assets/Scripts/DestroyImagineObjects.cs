using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyImagineObjects : MonoBehaviour {

    private void Start()
    {
        GameObject.Destroy(GameObject.FindGameObjectWithTag("Imagine_Objects"));
    }
}
