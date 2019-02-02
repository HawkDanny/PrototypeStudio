using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound : System.Object {

    public string name;

    public AudioClip clip;

    [Range(0.0f, 1.0f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

    //Set if the audio is going to be used spatially
    public GameObject parent;

    //True if the audio will always be playing (even if it will sometimes be muted)
    public bool playOnGameAwake;

    public int stream;
}
