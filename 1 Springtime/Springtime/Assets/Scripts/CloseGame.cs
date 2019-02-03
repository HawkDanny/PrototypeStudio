using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGame : MonoBehaviour {

    private void Awake()
    {
        Application.Quit();
    }
}
