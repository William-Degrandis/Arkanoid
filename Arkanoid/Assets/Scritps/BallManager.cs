using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    #region Instance

    public static BallManager _instance;

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

    [SerializeField]
    private Ball ballPrefab;

    private Ball InitialBall;
    private Rigidbody2D InitialBallR;

    public float Ispeed = 250;
    public List<Ball> Balls { get; set; }

    private void Start()
    {
        BallStarted();
    }

    private void Update()
    {
        if(!GameManager._instance.GameStarted)
        {
            Vector3 paddlePosition = PaddleController._instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + .27f, 0);
            InitialBall.transform.position = ballPosition;

            if(Input.GetMouseButtonDown(0)) //when you press the left mouse button throws the ball
            {
                InitialBallR.isKinematic = false;
                InitialBallR.AddForce(new Vector2(0, Ispeed));
                GameManager._instance.GameStarted = true;
            }
        }
    }

    public void SpawnBalls(Vector2 position, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Ball spawnball = Instantiate(ballPrefab, position, Quaternion.identity) as Ball;

            Rigidbody2D spawnballsRb = spawnball.GetComponent<Rigidbody2D>();
            spawnballsRb.isKinematic = false;
            spawnballsRb.AddForce(new Vector2(0, Ispeed));
            this.Balls.Add(spawnball);
        }
    }

    public void ResetBalls() //reset the ball
    {
        foreach (var ball in this.Balls.ToList())
        {
            Destroy(ball.gameObject);
        }

        BallStarted();
    }

    private void BallStarted() //the ball above the paddle is initialized
    {
        Vector3 paddlePosition = PaddleController._instance.gameObject.transform.position;
        Vector3 startingPosition = new Vector3(paddlePosition.x, paddlePosition.y + .25f, 0);
        InitialBall = Instantiate(ballPrefab, startingPosition, Quaternion.identity);

        InitialBallR = InitialBall.GetComponent<Rigidbody2D>();
        this.Balls = new List<Ball>
        {
            InitialBall
        };
    }
}

