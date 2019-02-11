using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private enum gameState
    {
        menu,
        game,
        gameOver
    }

    [Header("Timer")]
    public int timerStartingMinutes;
    public int timerStartingSeconds;

    [Header("UI")]
    public TextMeshProUGUI scoreLeft;
    public TextMeshProUGUI scoreRight;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI Z;
    public TextMeshProUGUI W;
    public TextMeshProUGUI U;
    public TextMeshProUGUI M;
    public TextMeshProUGUI gameOver;
    public Color zwColor;
    public Color umColor;
    public Color neutralColor;

    [Header("Rink")]
    public GameObject rink;

    [Header("Prefabs")]
    public GameObject leftPlayer;
    public GameObject rightPlayer;
    public GameObject puck;

    [Header("Spawn Points")]
    public Transform leftPlayerSpawn;
    public Transform rightPlayerSpawn;
    public Transform puckSpawn;

    private int _roundTimeLimit;
    private int _roundStartingTime;

    private gameState _currentGameState;

    private GameObject[] _players;
    private GameObject _puck;

    private int player1Score;
    private int player2Score;

    private bool startMatch;
    private bool restarting;

    private void Start()
    {
        startMatch = false;
        restarting = false;
        SetActiveStateOfZWUM(true);
        _roundTimeLimit = (timerStartingMinutes * 60) + timerStartingSeconds;
        _players = new GameObject[2];
        player1Score = 0;
        player2Score = 0;
        UpdateScores();
        SetActiveStateOfRinkAndScores(false);
    }

    private void Update()
    {
        switch (_currentGameState)
        {
            case gameState.menu:
                if (Input.GetKey(KeyCode.Z))
                    Z.color = zwColor;
                else
                    Z.color = Color.white;

                if (Input.GetKey(KeyCode.W))
                    W.color = zwColor;
                else
                    W.color = Color.white;

                if (Input.GetKey(KeyCode.U))
                    U.color = umColor;
                else
                    U.color = Color.white;

                if (Input.GetKey(KeyCode.M))
                    M.color = umColor;
                else
                    M.color = Color.white;


                //If both players are pressing the keys down, then the game can start
                if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.U) && Input.GetKey(KeyCode.M))
                {
                    if (startMatch)
                        return;
                    StartCoroutine("StartMatch");
                }
                break;
            case gameState.game:
                
                UpdateTimer();
                break;
            case gameState.gameOver:
                if (restarting)
                    return;
                    StartCoroutine("RestartGame");
                break;
        }
    }

    private void UpdateTimer()
    {
        int currentTimerTime = _roundTimeLimit - ((int)Time.time - _roundStartingTime);

        //If the timer is 0, change the game state
        if (currentTimerTime <= 0)
        {
            _currentGameState = gameState.gameOver;
            DespawnPlayersAndPuck();
            SetActiveStateOfRinkAndScores(false);
            SetActiveStateOfGameOverScreen(true);
            ShowGameOverScreen();
        }

        string timerText = ((int)(currentTimerTime / 60)) + ":" + ((int)(currentTimerTime % 60)).ToString("00");
        timer.SetText(timerText);
    }

    private void UpdateScores()
    {
        scoreLeft.text = player1Score + "";
        scoreRight.text = player2Score + "";
    }

    private void DespawnPlayersAndPuck()
    {
        GameObject.Destroy(_players[0]);
        GameObject.Destroy(_players[1]);
        GameObject.Destroy(_puck);
    }

    private void SpawnPlayersAndPuck()
    {
        print(_players.Length);
        _players[0] = GameObject.Instantiate(leftPlayer, leftPlayerSpawn.position, leftPlayerSpawn.rotation);
        _players[1] = GameObject.Instantiate(rightPlayer, rightPlayerSpawn.position, rightPlayerSpawn.rotation);
        _puck = GameObject.Instantiate(puck, puckSpawn.position, puckSpawn.rotation);
    }

    public void Scored(string goalName)
    {
        if (goalName == "leftGoal")
            player2Score += 1;
        else if (goalName == "rightGoal")
            player1Score += 1;

        UpdateScores();
        DespawnPlayersAndPuck();
        SpawnPlayersAndPuck();
    }

    private void SetActiveStateOfRinkAndScores(bool newState)
    {
        rink.SetActive(newState);
        scoreLeft.gameObject.SetActive(newState);
        scoreRight.gameObject.SetActive(newState);
        timer.gameObject.SetActive(newState);
    }

    private void SetActiveStateOfZWUM(bool newState)
    {
        Z.gameObject.SetActive(newState);
        W.gameObject.SetActive(newState);
        U.gameObject.SetActive(newState);
        M.gameObject.SetActive(newState);
    }

    private void SetActiveStateOfGameOverScreen(bool newState)
    {
        gameOver.gameObject.SetActive(newState);
    }

    private void ShowGameOverScreen()
    {
        gameOver.gameObject.SetActive(true);

        if (player1Score > player2Score)
        {
            gameOver.text = "Player 1 wins!";
            gameOver.color = zwColor;
        }
        else if (player2Score > player1Score)
        {
            gameOver.text = "Player 2 wins!";
            gameOver.color = umColor;
        }
        else
        {
            gameOver.text = "Draw!";
            gameOver.color = neutralColor;
        }
    }

    private IEnumerator RestartGame()
    {
        restarting = true;

        yield return new WaitForSeconds(5);

        restarting = false;

        _currentGameState = gameState.menu;
        SetActiveStateOfGameOverScreen(false);
        SetActiveStateOfZWUM(true);
        player1Score = 0;
        player2Score = 0;
    }

    private IEnumerator StartMatch()
    {
        startMatch = true;

        yield return new WaitForSeconds(0.1f);

        startMatch = false;
        _currentGameState = gameState.game;
        SetActiveStateOfZWUM(false);
        SetActiveStateOfRinkAndScores(true);
        SpawnPlayersAndPuck();
        _roundStartingTime = (int)Time.time;
        UpdateScores();

        
    }
}
