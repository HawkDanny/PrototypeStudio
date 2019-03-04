using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    [Header("Sounds")]
    public Sound[] sounds; //TODO: MAKE THIS A DICTIONARY
    [Header("Streams")]
    public List<string> streams; //Streams are clips that are meant to loop, but only one can play at a time //TODO: MAKE THIS A DICTIONARY
    

    private Sound[] _streams;


    private void Awake()
    {
        _streams = new Sound[streams.Count];

        foreach (Sound s in sounds)
        {
            if (s.parent == null)
                s.source = gameObject.AddComponent<AudioSource>();
            else
                s.source = s.parent.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            //Start playing immediately if it's persistent
            if (s.playOnGameAwake)
                s.source.Play();
        }
    }

    /// <summary>
    /// Plays the sound clip that corresponds to the name
    /// </summary>
    /// <param name="name">The name value of one of the sounds in the array</param>
    public void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);

        //If the clip is in a stream, stop the clip in the stream and replace it
        int streamIndex = streams.IndexOf(s.stream);
        if (s.stream != "")
        {
            if (_streams[streamIndex] != null) //If not first
                _streams[streamIndex].source.Stop();
            _streams[streamIndex] = s;
        }

        s.source.Play();
    }

    /// <summary>
    /// Stops the sound clip that corresponds to the name
    /// </summary>
    /// <param name="name">The name value of one of the sounds in the array</param>
    public void Stop(string name)
    {
        System.Array.Find(sounds, sound => sound.name == name).source.Stop();
    }

    /// <summary>
    /// Set the volume of a specific audio clip
    /// </summary>
    /// <param name="name">The name value of one of the sounds in the array</param>
    /// <param name="volume">The volume from 0 to 1</param>
    public void SetVolume(string name, float volume)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        s.source.volume = volume;
    }

    /// <summary>
    /// Stop all of the streams in the game
    /// </summary>
    public void StopAllStreams()
    {
        foreach (Sound s in _streams)
            s.source.Stop();
    }
}
