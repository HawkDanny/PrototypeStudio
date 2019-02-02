using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    //If this was a bigger project this would be a dictionary
    [Header("Streams")]
    public int numberOfStreams; //Streams are clips that are meant to loop, but only one can play at a time //TODO: MAKE THIS A DICTIONARY
    [Header("Sounds")]
    public Sound[] sounds;

    private Sound[] streams;


    private void Awake()
    {
        streams = new Sound[numberOfStreams];

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
        if (s.stream != -1)
        {
            if (streams[s.stream] != null) //If not first
                streams[s.stream].source.Stop();
            streams[s.stream] = s;
        }

        s.source.Play();
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
        foreach (Sound s in streams)
            s.source.Stop();
    }
}
