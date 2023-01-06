using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject hudPanel;
    public GameObject shopPanel;
    public GameObject shop;
    public GameObject gameOverPanel;
    public GameObject spawner;
    public GameObject player;
    public GameObject center;
    [HideInInspector] public State gameState;
    private int wave;
    private bool victory = false;
    [HideInInspector] public int Wave
    { get { return wave; }
        set { wave = value;
        hudPanel.transform.Find("Wave").GetComponent<Text>().text = "WAVE " + (wave+1); }}
    private float score;
    [HideInInspector] public float Score
    { get { return score; }
        set { score = value;
        hudPanel.transform.Find("Score").GetComponent<Text>().text = "SCORE : " + score; }}
    
    public enum State
    {
        MainMenu,
        Play,
        Shop,
        GameOver
    }
    
    void Awake()
    {
        EnterState(State.MainMenu);
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) { Application.Quit(); }

        switch (gameState)
        {
            case State.MainMenu:
                break;

            case State.Play:
                break;
            
            case State.Shop:
                break;

            case State.GameOver:
                break;
        }
    }
    
    public void ChangeState(State state)
    {
        ExitState(gameState);
        EnterState(state);
        gameState = state;
    }

    public void ChangeState(string stateName)
    {
        ChangeState((State)Enum.Parse(typeof(State), stateName));
    }

    void EnterState(State state)
    {
        switch (state)
        {
            case State.MainMenu:
                mainMenuPanel.SetActive(true);
                mainMenuPanel.transform.Find("HighScoreTitle/HighScore").GetComponent<Text>().text = PlayerPrefs.GetFloat("HighScore", 0).ToString();
                center.GetComponent<PlayButton>().enabled = true;
                break;

            case State.Play:                
                player.SetActive(true);
                spawner.SetActive(true);
                hudPanel.SetActive(true);
                center.GetComponent<Center>().enabled = true;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                break;

            case State.Shop:
                shop.SetActive(true);
                shopPanel.SetActive(true);
                hudPanel.SetActive(true);
                center.SetActive(false);
                break;

            case State.GameOver:
                center.SetActive(false);
                gameOverPanel.SetActive(true);
                
                var gameOverTitle = gameOverPanel.transform.Find("Text").GetComponent<Text>();

                if (victory)
                {
                    gameOverTitle.text = "YOU WIN!";
                    gameOverTitle.color = new Color(224, 121, 0);
                }
                else
                {
                    gameOverTitle.text = "GAME OVER";
                    gameOverTitle.color = new Color(152, 0, 0);
                }

                var gameOverScore = gameOverPanel.transform.Find("Score");
                var gameOverHighScore = gameOverScore.transform.Find("HighScore").GetComponent<Text>();

                gameOverScore.GetComponent<Text>().text = "SCORE: " + score;

                var highscore = PlayerPrefs.GetFloat("HighScore", 0);
                if (score > highscore)
                {
                    PlayerPrefs.SetFloat("HighScore", score);
                    gameOverHighScore.text = "NEW HIGHSCORE!";
                    gameOverHighScore.color = new Color(224, 121, 0);
                }
                else
                {
                    gameOverHighScore.text = "HIGHSCORE: " + highscore;
                    gameOverHighScore.color = Color.white;
                }
                break;
        }
    }

    void ExitState(State state)
    {
        switch (state)
        {
            case State.MainMenu:
                mainMenuPanel.SetActive(false);
                center.GetComponent<PlayButton>().enabled = false;
                break;

            case State.Play:
                player.SetActive(false);
                spawner.SetActive(false);
                hudPanel.SetActive(false);
                center.GetComponent<Center>().enabled = false;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                break;

            case State.Shop:
                shop.SetActive(false);
                shopPanel.SetActive(false);
                hudPanel.SetActive(false);
                center.SetActive(true);
                break;

            case State.GameOver:
                center.SetActive(true);
                gameOverPanel.SetActive(false);
                ResetStats();
                break;
        }
    }

    public void Win()
    {
        victory = true;
        ChangeState(State.GameOver);
    }

    private void ResetStats()
    {
        Score = 0;
        Wave = 0;
        center.GetComponent<Center>().HP = center.GetComponent<Center>().startingMaxHealth;
        center.GetComponent<Center>().maxHealth = center.GetComponent<Center>().startingMaxHealth;
        player.GetComponent<Player>().Shockwaves = player.GetComponent<Player>().startingShockwaves;
        spawner.GetComponent<Spawner>().healChanceModifier = 0f;
        spawner.GetComponent<Spawner>().burstChanceModifier = 0f;
        victory = false;
    }
}
