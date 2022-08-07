using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBlock : MonoBehaviour
{
    private BallSpawner ballSpawner;
    private Enemy enemy;

    private void Start()
    {
        ballSpawner = GameObject.FindObjectOfType<BallSpawner>();
        enemy = GameObject.FindObjectOfType<Enemy>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            ballSpawner.BallCount(1);
            enemy.RemoveBlock(gameObject);
            Destroy(gameObject);
        }
    }
}
