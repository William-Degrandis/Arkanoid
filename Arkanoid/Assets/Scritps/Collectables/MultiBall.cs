using System.Linq;

public class MultiBall : Collectable
{
    protected override void ApplyEffect()
    {
        foreach (Ball ball in BallManager._instance.Balls.ToList())
        {
            BallManager._instance.SpawnBalls(ball.gameObject.transform.position, 2);
        }
    }
}

