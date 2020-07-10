using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) //collision with the tag "deathwall"
    {
        if(collision.tag == "Ball")
        {
            Ball ball = collision.GetComponent<Ball>();
            BallManager._instance.Balls.Remove(ball);
            ball.Die();
        }
    }
}
