using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public float beatLength;
    public float fudging;
    private AudioManager audioMan;
    private Dancer[] dancers;
    private float previousShift;
    private float currentShift;
    private AudioSource convo;
    private bool notBeingPressed;

    private void Start()
    {
        audioMan = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Dancer");

        dancers = new Dancer[temp.Length];
        for (int i = 0; i < dancers.Length; i++)
            dancers[i] = temp[i].GetComponent<Dancer>();

        previousShift = 0;
        currentShift = 0;

        convo = audioMan.gameObject.GetComponent<AudioSource>();
        notBeingPressed = false;
    }

    private void Update()
    {
        //Left
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ShiftPressed();
            foreach (Dancer d in dancers)
                d.LeftStepDown();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            foreach (Dancer d in dancers)
                d.LeftStepUp();
        }

        //Right
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            ShiftPressed();
            foreach (Dancer d in dancers)
                d.RightStepDown();
        }

        if (Input.GetKeyUp(KeyCode.RightShift))
        {
            foreach (Dancer d in dancers)
                d.RightStepUp();
        }

        //If they don't press shift at all it will pitch down
        if (!notBeingPressed && Time.time - currentShift > beatLength * 2f)
        {
            notBeingPressed = true;
            DOTween.To(() => convo.pitch, x => convo.pitch = x, 0.7f, 0.1f);
        }
    }

    private void ShiftPressed()
    {
        //Too slow
        if (currentShift - previousShift > beatLength + fudging)
        {
            DOTween.To(() => convo.pitch, x => convo.pitch = x, 0.7f, 0.1f);
        }
        else if (currentShift - previousShift < beatLength - fudging) //Too fast
        {
            DOTween.To(() => convo.pitch, x => convo.pitch = x, 1.3f, 0.1f);
        }
        else
        {
            DOTween.To(() => convo.pitch, x => convo.pitch = x, 1.0f, 0.1f);
        }

        previousShift = currentShift;
        currentShift = Time.time;
    }
}
