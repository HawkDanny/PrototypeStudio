using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public TextMeshProUGUI textbox;
    public Event[] events;

    private AudioManager _audioMan;
    private lb_BirdController _birds; //This is gross

    //The index that corresponds with the events array
    private int currentEventIndex;
    private float timeAtPreviousEventEnd; //Time.time at the end of the previous event

    private void Start()
    {
        _audioMan = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        currentEventIndex = 0;
        timeAtPreviousEventEnd = 0;

        _birds = GameObject.FindGameObjectWithTag("BirdManager").GetComponent<lb_BirdController>();
    }

    private void Update()
    {
        Event currentEvent = events[currentEventIndex];

        if (currentEvent.waitForTrigger)
        {
            //Do nothing
        }
        //If the current event's lead time is up
        else if (Time.time > timeAtPreviousEventEnd + currentEvent.leadTime)
        {
            //Display the text
            FlashText(currentEvent.text, currentEvent.duration, currentEvent.maxTextSize);

            //If there is audio associated with the event, play it
            if (currentEvent.audioClips.Length > 0)
                for (int i = 0; i < currentEvent.audioClips.Length; i++)
                    _audioMan.Play(currentEvent.audioClips[i]);

            //Increment the event count and set the timeAtPreviousEventEnd equal to the time it will be at the end of the event
            currentEventIndex = Mathf.Min(currentEventIndex + 1, events.Length - 1);
            timeAtPreviousEventEnd = Time.time + currentEvent.duration;


            //Spawn any gameobjects
            for (int i = 0; i < currentEvent.spawnObjects.Length; i++)
                GameObject.Instantiate(currentEvent.spawnObjects[i]);
        }
    }

    /// <summary>
    /// Show custom text on the screen for a given duration
    /// </summary>
    /// <param name="text">The text to be shown</param>
    /// <param name="duration">How long in seconds the text is on the screen</param>
    /// <param name="waitBeforeTrigger">How long to wait after being called before the text appears</param>
    public void FlashText(string text, float duration, int maxTextSize = 1000, float waitTimeBeforePlay = 0f)
    {
        StartCoroutine(DisplayText(textbox, text, duration, maxTextSize, waitTimeBeforePlay));
    }

    //Coroutine called from FlashText
    private IEnumerator DisplayText(TextMeshProUGUI textbox, string text, float duration, int maxTextSize, float waitTimeBeforePlay)
    {
        //Wait first if we have to
        yield return new WaitForSeconds(waitTimeBeforePlay);
        textbox.text = text;
        textbox.fontSizeMax = maxTextSize;
        textbox.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        textbox.gameObject.SetActive(false);
    }
}
