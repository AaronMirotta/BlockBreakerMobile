using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    private BallSpawner ballSpawner;

    private void Start()
    {
        ballSpawner = GameObject.FindObjectOfType<BallSpawner>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            collision.gameObject.GetComponent<Ball>().inPlay = false;//when a ball collides with the floor change the inPlay bool to false
            ballSpawner.numBallsInPlay--;
        }
    }
}
