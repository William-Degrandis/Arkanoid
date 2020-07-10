using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    public static event Action<Ball> BallDeath;

    public void Die()
    {
        BallDeath.Invoke(this);
        Destroy(gameObject, 1);
    }
}
