using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //Merger of EnemyTurn, EnemyHealth, BlockSpawner

    [Header("Difficulty Options")]
    
    [SerializeField]
    private List<GameObject> blockTypes = new List<GameObject>();
    
    [SerializeField]
    private float enemyHealth;

    [SerializeField]
    private int maxBlockHp;

    [Header("Gameplay Options")]
    
    [SerializeField]
    private float moveDist;

    [SerializeField]
    private float hpPool;

    [SerializeField]
    private float loseYPos;//the y position that will cause the player to lose

    [SerializeField]
    private int multiLayerSpawnCount;//number of layers to spawn when the board is clear

    [SerializeField]
    private List<Transform> blockSpawnPoints = new List<Transform>();
    
    private Slider hpSlider;

    private List<GameObject> blocksInPlay = new List<GameObject>();

    private GameController gameController;

    private bool gameOver;
    
    private void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
        hpSlider = GameObject.FindObjectOfType<Slider>();

        hpSlider.maxValue = enemyHealth;
        hpSlider.value = enemyHealth;

        hpPool = enemyHealth;

        gameOver = false;
    }

    private void Update()
    {
        if(gameController.GetState() == GameController.GameState.EnemyTurn)
        {
            if(enemyHealth <= 0)
            {
                //player Win
                Debug.Log("PlayerWin");

                //check if there are blocks left on the board
                if(blocksInPlay.Count > 0)
                {
                    ResetBoard();//delete all blocks on the board
                }

                gameController.ChangeState(GameController.GameState.PlayerWin);
                gameOver = true;
                
            }

            //check all the blocks to see if any have reached the bottom
            foreach(GameObject block in blocksInPlay)
            {
                if (block.GetComponent<Block>())
                {
                    if (block.GetComponent<Block>().GetYPos() <= loseYPos)
                    {
                        Debug.Log("Player Lose!");
                        gameController.ChangeState(GameController.GameState.PlayerLose);
                        gameOver = true;

                        //display a lose screen with a retry or exit option
                    }
                }
            }
            Debug.Log(gameController.GetState());
            MoveLayers(moveDist);
           
            if(!gameOver)//if the player loses / wins dont bother spawning anymore blocks
            {
                if (hpPool > 0)
                {
                    if (blocksInPlay.Count <= 0)
                    {
                        //spawn 3 layers instead of just one
                        StartCoroutine(MultiLayerSpawn(multiLayerSpawnCount, 0.25f));
                    }
                    else
                    {
                        int randNumBlocks = Random.Range(1, 8);
                        SpawnLayer(randNumBlocks, 8);
                    }
                }
                gameController.ChangeState(GameController.GameState.PlayerTurn);
            }
        }
    }

    private void SpawnLayer(int numBlocks, int numSpawnPoints)
    {
        bool[] canSpawnBlock = new bool[numSpawnPoints];

        if (hpPool <= 8)
        {
            //if hpPool is less than equal to 8 spawn hpPool # of blocks
            for (int i = 0; i < hpPool; i++)//randomly choosing the spawnpoints that will spawn a block
            {
                int randBlock = Random.Range(0, numSpawnPoints);

                if (!canSpawnBlock[randBlock])
                {
                    canSpawnBlock[randBlock] = true;
                }
                else if (canSpawnBlock[randBlock])
                {
                    i--;
                }
            }
        }
        else
        {
            for (int i = 0; i < numBlocks; i++)//randomly choosing the spawnpoints that will spawn a block
            {
                int randBlock = Random.Range(0, numSpawnPoints);

                if (!canSpawnBlock[randBlock])
                {
                    canSpawnBlock[randBlock] = true;
                }
                else if (canSpawnBlock[randBlock])
                {
                    i--;
                }
            }
        }
        //loop through canSpawnBlock; if true, instantiate a random block from block pool
        for (int i = 0; i < canSpawnBlock.Length; i++)
        {
            if (canSpawnBlock[i])
            {
                float randBlock = Random.value;
                if(randBlock < 0.2)//20% change to spawn the bonus ball block
                {
                    GameObject newBlock = Instantiate(blockTypes[1], blockSpawnPoints[i].position, Quaternion.identity);
                    blocksInPlay.Add(newBlock);
                }
                else if(randBlock > 0.2)
                {
                    GameObject newBlock = Instantiate(blockTypes[0], blockSpawnPoints[i].position, Quaternion.identity);
                    int blockHp = Random.Range(1, maxBlockHp);
                    newBlock.GetComponent<Block>().SetHp(blockHp);
                    hpPool -= 1;
                    blocksInPlay.Add(newBlock);
                }
            }
        }
    }
    private IEnumerator MultiLayerSpawn(int numLayers, float time)
    {
        for(int i = 0; i < numLayers; i++)
        {
            int randNumBlocks = Random.Range(1, 8);
            MoveLayers(moveDist);
            SpawnLayer(randNumBlocks, 8);
            yield return new WaitForSeconds(time);
        }
    }
    private void ResetBoard()
    {
        foreach(GameObject block in blocksInPlay)
        {
            RemoveBlock(block);
        }
    }
    public void MoveLayers(float distY)
    {
        foreach(GameObject blocks in blocksInPlay)
        {
            //lerp y axis down to a specified distance
            //get current y pos, subtract distY then lerp to product
            float destY = blocks.transform.position.y - distY;
            blocks.transform.position = new Vector2(blocks.transform.position.x, destY);
        }
    }

    public void RemoveBlock(GameObject block)
    {
        blocksInPlay.Remove(block);
        Destroy(block);
    }

    public void TakeDmg(int dmg)
    {
        enemyHealth -= dmg;
        hpSlider.value -= dmg;
    }
    public void SetHp(int hp)
    {
        enemyHealth = hp;
    }
    public void SetMaxBlockHp(int blockHp)
    {
        maxBlockHp = blockHp;
    }

    public int GetHp() { return (int)enemyHealth; }
    public int GetMaxBlockHp() { return maxBlockHp; }
}
