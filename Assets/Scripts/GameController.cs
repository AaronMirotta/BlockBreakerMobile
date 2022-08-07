using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameState { GameStart, PlayerTurn, EnemyTurn, PlayerWin, PlayerLose, GameEnd}

    [SerializeField]
    private GameState currentState;

    [SerializeField]
    private BallSpawner ballSpawner;

    [SerializeField]
    private EnemySpawner enemySpawner;

    private Enemy currentEnemy;

    private void Start()
    {
        currentState = GameState.GameStart;
    }
    public void ChangeState(GameState state)
    {
        currentState = state;
    }
    public GameState GetState()
    {
        return currentState;
    }
}
