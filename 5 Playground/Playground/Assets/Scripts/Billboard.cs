using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Update()
    {
        //this.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.y, 0f);
        this.transform.LookAt(Camera.main.transform);
        Vector3 currentRotation = this.transform.rotation.eulerAngles;
        this.transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f); //not the best code but I'm over it
    }
}
