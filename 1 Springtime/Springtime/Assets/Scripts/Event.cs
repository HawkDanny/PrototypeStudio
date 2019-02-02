using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Event : System.Object {

    public string text; //The text to be shown on screen
    public int maxTextSize; //Size of the text
    public float duration; //How long in seconds the text is shown
    public float leadTime; //The time in seconds since the last text was shown before this text will be shown
    //public float waitTimeBeforePlay; //The time in seconds between the play method being called and the audio actually playing
    public bool waitForTrigger; //True if the audio only plays when triggered

    public GameObject[] spawnObjects;
    public string[] audioClips;
}
