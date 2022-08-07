using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDebug : MonoBehaviour
{
    //track variables that pertain to the current level

    private EnemySpawner enemySpawner;

    private BallSpawner ballSpawner;

    [SerializeField]
    private Text currentLevelTxt;

    [SerializeField]
    private Text numBallsTxt;

    private void Start()
    {
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
        ballSpawner = GameObject.FindObjectOfType<BallSpawner>();
    }

    private void Update()
    {
        numBallsTxt.text = "Balls: " + ballSpawner.GetBallCount().ToString();
        currentLevelTxt.text = "Level: " + enemySpawner.currentLevel.ToString();
    }
}
