using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject defaultEnemyPrefab;

    private GameController state;

    private int enemyHp;

    private int maxBlockHp;

    public GameObject curEnemy;

    public bool enemySpawned;

    public int currentLevel;

    private void Start()
    {
        state = GameObject.FindObjectOfType<GameController>();
        currentLevel = 1;
        enemySpawned = false;
        maxBlockHp = 5;
        StartCoroutine(SpawnBasicEnemy(0f));
    }

    private void Update()
    {
        if (curEnemy)
        {
            if (state.GetState() == GameController.GameState.PlayerWin)//if enemy hp = 0 destroy and spawn new enemy
            {
                enemySpawned = false;
                currentLevel++;
                StartCoroutine(SpawnBasicEnemy(2f));
                Destroy(curEnemy);
            }
        }
    }
    private void NextLevel(Enemy nextEnemy)
    {
        //increase enemy hp by 5
        enemyHp += 5;
        nextEnemy.SetHp(enemyHp);

        //every 2 levels increase block max hp by 2
        if(currentLevel % 2 == 0)
        {
            Debug.Log("Block hp increase");
            maxBlockHp += 2;
            nextEnemy.SetMaxBlockHp(maxBlockHp);
        }
    }
    public IEnumerator SpawnBasicEnemy(float time)
    {
        yield return new WaitForSeconds(time);

        curEnemy = Instantiate(defaultEnemyPrefab, transform.position, Quaternion.identity);

        NextLevel(curEnemy.GetComponent<Enemy>());

        state.ChangeState(GameController.GameState.GameStart);
        enemySpawned = true;
        //enemy has been spawned, player turn
        state.ChangeState(GameController.GameState.EnemyTurn);
    }
}
