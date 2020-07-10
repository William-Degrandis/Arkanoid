using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text Target;
    public Text ScoreT;
    public Text LivesT;

    public int Score { get; set; }

    private void Awake()
    {
        Brick.OnebrickDistruction += OneBrickDistruction;
        BrickManager.OnLevelLoaded += OnLevelLoaded;
        GameManager.OnLiveLost += OnLiveLost;
    }

    private void Start()
    {
        OnLiveLost(GameManager._instance.InitialLives);
    }

    private void OnLiveLost(int remainingLives) //displays the remaining lives on the screennc
    {
        LivesT.text = $@"LIVES: 
    {remainingLives}";
    }

    private void OnLevelLoaded()
    {
        UpdateRemainingBrickText();
        UpdateScoreText(0);
    }

    private void UpdateScoreText(int increment) //displays the score on the screen
    {
        this.Score += increment;
        string scoreS = this.Score.ToString().PadLeft(00000);
        ScoreT.text = $@"SCORE: 
    {scoreS}";
    }

    private void OneBrickDistruction(Brick obj)
    {
        UpdateRemainingBrickText();
        UpdateScoreText(10);
    }

    private void UpdateRemainingBrickText()
    {
        Target.text = $@"TARGET:
  {BrickManager._instance.remainingBricks.Count}/{BrickManager._instance.InitialBrickCount}";
    }

    private void OnDisable()
    {
        Brick.OnebrickDistruction -= OneBrickDistruction;
        BrickManager.OnLevelLoaded -= OnLevelLoaded;
    }
}
