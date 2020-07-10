using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private SpriteRenderer sr;

    public int Hitpoints = 1;

    public static event Action<Brick> OnebrickDistruction;

    private void Awake()
    {
        this.sr = this.GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);

    }

    private void ApplyCollisionLogic(Ball ball) //when the ball collides with the paddle
    {
        this.Hitpoints--;

        if(this.Hitpoints <= 0)
        {
            BrickManager._instance.remainingBricks.Remove(this);
            OnebrickDistruction?.Invoke(this);
            OnebrickDestroy();
            Destroy(this.gameObject);
        }
        else
        {
            this.sr.sprite = BrickManager._instance.Sprites[this.Hitpoints - 1];
        }
    }

    private void OnebrickDestroy()
    {
        float buffSpawnChance = UnityEngine.Random.Range(0, 100f);
        float debuffSpawnChance = UnityEngine.Random.Range(0, 100f);
        bool alreadySpawned = false;
    
        if (buffSpawnChance <= CollectablesManager._instance.BuffChance)
        {
            alreadySpawned = true;
            Collectable newbuff = this.SpawnCollectable(true);
        }

        if(buffSpawnChance <= CollectablesManager._instance.DefuffChance && alreadySpawned)
        {
            Collectable newDebuff = this.SpawnCollectable(false);
        }
    }

    private Collectable SpawnCollectable(bool isBuff) //buff / debuff spawn
    {
        List<Collectable> collection;

        if(isBuff)
        {
            collection = CollectablesManager._instance.AvailableBuffs;
        }
        else
        {
            collection = CollectablesManager._instance.AvailableDebuffs;
        }

        int buffIndex = UnityEngine.Random.Range(0, collection.Count);
        Collectable prefab = collection[buffIndex];
        Collectable newCollectable = Instantiate(prefab, this.transform.position, Quaternion.identity) as Collectable;

        return newCollectable;
    }

    public void Init(Transform containerTransform, Sprite sprite, Color color, int hitpoints) //bricks initialization
    {
        this.transform.SetParent(containerTransform);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.Hitpoints = hitpoints;
    }
}
