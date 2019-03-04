using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Rewired")]
    public int playerId;
    private Rewired.Player player;

    [Header("Canvas")]
    public Canvas canvas;
    public float maxScale;

    [Header("Colors")]
    public Color primaryColor;
    public Color secondaryColor;

    [Header("Text")]
    public TextMeshProUGUI title;
    public TextMeshProUGUI author;

    private int buttonsDown; //maximum value of 14
    private int previousButtonsDown = 0;
    private AudioManager _audioMan;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
        _audioMan = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        ProcessInput();

        float scaleValue = Map(buttonsDown, 0f, 14f, 0f, maxScale);
        canvas.transform.DOScale(new Vector3(scaleValue, scaleValue, scaleValue), 0.1f);

        //Don't start new clips if the buttons have not changed
        if (previousButtonsDown == buttonsDown)
            return;

        //Music
        if (buttonsDown == 2)
            _audioMan.Play("c2");
        else if (buttonsDown < 2)
            _audioMan.Stop("c2");

        if (buttonsDown == 4)
            _audioMan.Play("e2");
        else if (buttonsDown < 4)
            _audioMan.Stop("e2");

        if (buttonsDown == 6)
            _audioMan.Play("g2");
        else if (buttonsDown < 6)
            _audioMan.Stop("g2");

        if (buttonsDown == 8)
            _audioMan.Play("c3");
        else if (buttonsDown < 8)
            _audioMan.Stop("c3");

        if (buttonsDown == 10)
            _audioMan.Play("e3");
        else if (buttonsDown < 10)
            _audioMan.Stop("e3");

        if (buttonsDown == 12)
            _audioMan.Play("g3");
        else if (buttonsDown < 12)
            _audioMan.Stop("g3");

        if (buttonsDown == 14)
        {
            _audioMan.Play("c4");
            Camera.main.backgroundColor = secondaryColor;
            title.color = primaryColor;
            author.color = primaryColor;
        }
        else if (buttonsDown < 14)
        {
            _audioMan.Stop("c4");
            Camera.main.backgroundColor = primaryColor;
            title.color = secondaryColor;
            author.color = secondaryColor;
        }


        previousButtonsDown = buttonsDown;
    }

    private void ProcessInput()
    {
        buttonsDown = 0;

        Vector2 leftStick = new Vector2();
        Vector2 rightStick = new Vector2();
        leftStick.x = player.GetAxis("xboxLeftStick_h");
        leftStick.y = player.GetAxis("xboxLeftStick_v");
        rightStick.x = player.GetAxis("xboxRightStick_h");
        rightStick.y = player.GetAxis("xboxRightStick_v");

        if (leftStick.magnitude > 0.95f)
            ++buttonsDown;
        if (rightStick.magnitude > 0.95f)
            ++buttonsDown;
        if (player.GetButton("xboxA"))
            ++buttonsDown;
        if (player.GetButton("xboxB"))
            ++buttonsDown;
        if (player.GetButton("xboxX"))
            ++buttonsDown;
        if (player.GetButton("xboxY"))
            ++buttonsDown;
        if (player.GetButton("xboxRB"))
            ++buttonsDown;
        if (player.GetButton("xboxLB"))
            ++buttonsDown;
        if (player.GetButton("xboxRT"))
            ++buttonsDown;
        if (player.GetButton("xboxLT"))
            ++buttonsDown;
        if (player.GetButton("xboxStart"))
            ++buttonsDown;
        if (player.GetButton("xboxSelect"))
            ++buttonsDown;

        int dpadButtonsDown = 0;
        if (player.GetButton("xboxDpadUp"))
            ++dpadButtonsDown;
        if (player.GetButton("xboxDpadRight"))
            ++dpadButtonsDown;
        if (player.GetButton("xboxDpadDown"))
            ++dpadButtonsDown;
        if (player.GetButton("xboxDpadLeft"))
            ++dpadButtonsDown;

        buttonsDown += Mathf.Min(dpadButtonsDown, 2);
    }

    private float Map(float value, float aMin, float aMax, float bMin, float bMax)
    {
        return Mathf.Lerp(bMin, bMax, Mathf.InverseLerp(aMin, aMax, value));
    }
}
