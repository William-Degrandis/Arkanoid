using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Instance

    public static GameManager _instance;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public GameObject gameOverScreen;
    public GameObject victoryScreen;

    public int InitialLives = 3;

    public int Lives { get; set; }

    public bool GameStarted { get; set; }

    public static event Action<int> OnLiveLost;

    private void Start()
    {
        this.Lives = this.InitialLives;
        Screen.SetResolution(540, 960, false);
        Ball.BallDeath += BallDeath;
        Brick.OnebrickDistruction += OneBrickDistruction;
    }

    private void OneBrickDistruction(Brick obj)
    {
        if (BrickManager._instance.remainingBricks.Count <= 0)
        {
            BallManager._instance.ResetBalls();
            GameManager._instance.GameStarted = false;
            BrickManager._instance.LoadNextLevel();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void BallDeath(Ball obj)
    {
        if(BallManager._instance.Balls.Count <= 0)
        {
            this.Lives--;

            if(this.Lives < 1)
            {
                gameOverScreen.SetActive(true);
            }
            else
            {
                OnLiveLost.Invoke(this.Lives);
                BallManager._instance.ResetBalls();
                GameStarted = false;
                BrickManager._instance.LoadLevel(BrickManager._instance.CurrentLevel);
            }
        }
    }

    internal void ShowVictoryScreen() //show victory screen
    {
        victoryScreen.SetActive(true);
    }

    private void OnDisable()
    {
        Ball.BallDeath -= BallDeath;
    }
}
