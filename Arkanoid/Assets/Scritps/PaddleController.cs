using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    #region Instance

    public static PaddleController _instance;

    public bool PaddleIsTrasforming { get; set; }

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

    private Camera mainCamera;
    private float paddleStarPos;
    //float paddleWidthPixels = 200;
    //float dleftClamp = 130;
    //float drighClamp = 420;
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;

    public float maxDistance = 5f;
    public float moveSpeed = 5;
    public float expandPaddleDuration = 10;
    public float paddleWidth = 2;
    public float paddleHeight = 0.28f;

    void Start()// Start is called before the first frame update
    {
        mainCamera = FindObjectOfType<Camera>();
        paddleStarPos = this.transform.position.y;
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    void Update()// Update is called once per frame
    {
        PaddleMove();
    }

    public void StartWidthBuff(float newWidth)
    {
        StartCoroutine(AnimatePaddleWidth(newWidth));
    }

    public IEnumerator AnimatePaddleWidth(float width)
    {
        this.PaddleIsTrasforming = true;
        this.StartCoroutine(ResetPaddleExplandTime(this.expandPaddleDuration));

        if(width > this.sr.size.x)
        {
            float currentWidth = this.sr.size.x;

            while(currentWidth < width)
            {
                currentWidth += Time.deltaTime * 2;
                this.sr.size = new Vector2(currentWidth, paddleHeight);
                boxCol.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }
        else
        {
            float currentWidth = this.sr.size.x;

            while(currentWidth > width)
            {
                currentWidth -= Time.deltaTime * 2;
                this.sr.size = new Vector2(currentWidth, paddleHeight);
                boxCol.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }

        this.PaddleIsTrasforming = false;
    }

    private IEnumerator ResetPaddleExplandTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.StartWidthBuff(this.paddleWidth);
    }

    private void PaddleMove()
    {
        float move = Input.GetAxis("Mouse X");

        if (move > 0)
        {
            if (transform.position.x > maxDistance)
                move = 0;
        }
        else
        {
            if (move < 0)
            {
                if (transform.position.x < -maxDistance)
                    move = 0;
            }
        }

        transform.Translate(Vector3.right * move * Time.deltaTime * moveSpeed);

        //float paddleShift = (paddleWidthPixels - ((paddleWidthPixels / 2) * this.sr.size.x)) / 2;
        //float leftClamp = dleftClamp - paddleShift;
        //float rightClamp = drighClamp + paddleShift;
        //float mousePosPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        //float mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosPixels, 0, 0)).x;
        //this.transform.position = new Vector3(mousePosition, paddleStarPos, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Rigidbody2D ballR = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitpoints = collision.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            ballR.velocity = Vector2.zero;

            float difference = paddleCenter.x - hitpoints.x;

            if (hitpoints.x < paddleCenter.x)
            {
                ballR.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), BallManager._instance.Ispeed));
            }
            else
            {
                ballR.AddForce(new Vector2((Mathf.Abs(difference * 200)), BallManager._instance.Ispeed));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position - Vector3.right * maxDistance, transform.position + Vector3.right * maxDistance);
    }
}
