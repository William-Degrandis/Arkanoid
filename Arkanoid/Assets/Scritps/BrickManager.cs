using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BrickManager : MonoBehaviour
{
    #region Instance

    public static BrickManager _instance;

    public static event Action OnLevelLoaded;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    private int maxRow = 17;
    private int maxCol = 12;
    //initial position of the spawn of the bricks (position X and Y)
    private float initialBrickSpawnPosX = -1.96f;
    private float initialBrickSpawnPosY = 2.95f;
    private float shiftAmount = 0.365f;
    public int CurrentLevel;

    private GameObject bricksContainer;

    public Brick brickPrefabs;

    public Sprite[] Sprites;

    public Color[] Brickcolors;

    public int InitialBrickCount { get; set; }

    public List<Brick> remainingBricks { get; set; }
    public List<int[,]> LevelData { get; set; }

    private void Start()
    {
        this.bricksContainer = new GameObject("brickContainer");
        this.remainingBricks = new List<Brick>();
        this.LevelData = this.LoadLevelsData();
        this.GenerateBricks();
    }

    public void LoadNextLevel() //load the next level
    {
        this.CurrentLevel++;
        if(this.CurrentLevel >= this.LevelData.Count)
        {
            GameManager._instance.ShowVictoryScreen();
        }
        else
        {
            this.LoadLevel(this.CurrentLevel);
        }
    }

    public void LoadLevel(int level) //load the current level
    {
        this.CurrentLevel = level;
        this.ClearRemainingBricks();
        this.GenerateBricks();
    } 

    private void ClearRemainingBricks()
    {
        foreach(Brick brick in this.remainingBricks.ToList())
        {
            if(brick != null)
            Destroy(brick.gameObject);
        }
    }

    private void GenerateBricks() //generate bricks in the current level
    {
        int[,] currentLevelData = this.LevelData[this.CurrentLevel];
        float currentSpawnX = initialBrickSpawnPosX;
        float currentSpawnY = initialBrickSpawnPosY;
        float zShift = 0;

        for (int row = 0; row < this.maxRow; row++)
        {
            for (int col = 0; col < this.maxCol; col++)
            {
                int brickType = currentLevelData[row, col];

                if(brickType > 0)
                {
                    Brick newBrick = Instantiate(brickPrefabs, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(bricksContainer.transform, this.Sprites[brickType - 1], this.Brickcolors[brickType], brickType);

                    this.remainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }
                currentSpawnX += shiftAmount;

                if(col + 1 == this.maxCol)
                {
                    currentSpawnX = initialBrickSpawnPosX;
                }
            }
            currentSpawnY -= shiftAmount;
        }
        this.InitialBrickCount = this.remainingBricks.Count;
        OnLevelLoaded?.Invoke();
    } 

    private List<int[,]> LoadLevelsData() //load the list of layers through a text file
    {
        TextAsset text = Resources.Load("levels") as TextAsset;

        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> levelData = new List<int[,]>();
        int[,] currentlevel = new int[maxRow, maxCol];
        int currentRow = 0;

        for(int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];

            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for(int col = 0; col < bricks.Length; col++)
                {
                    currentlevel[currentRow, col] = int.Parse(bricks[col]);
                }

                currentRow++;
            }
            else
            {
                currentRow = 0;
                levelData.Add(currentlevel);
                currentlevel = new int[maxRow, maxCol];
            }
        }
        return levelData;
    }
}
