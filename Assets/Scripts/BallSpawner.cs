using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject ballDefault;
    [SerializeField]
    private int numBalls;
    [SerializeField]
    private bool spawnBalls;
    [SerializeField]
    private float interval;
    [SerializeField]
    private float speedModifier;
    [SerializeField]
    private Vector2 ballDistance;
    [SerializeField]
    private float minShootPower;

    [SerializeField]
    private bool inPlay;

    [SerializeField]
    private GameController state;
    
    private Touch touchInput;

    private LineRenderer shootLine;

    private Vector3 initialMousePos;

    private Vector3 finalMousePos;

    private Vector3 angle;

    private List<Transform> balls = new List<Transform>();

    public int numBallsInPlay;

    private void Start()
    {
        shootLine = gameObject.GetComponent<LineRenderer>();
        shootLine.SetPosition(0, transform.position);
        shootLine.SetPosition(1, transform.position);
        shootLine.startColor = new Color(207, 199, 182);
        shootLine.endColor = new Color(207, 199, 182, 0);

        numBallsInPlay = 0;

        CreateBall(ballDefault);
    }

    private void Update()
    {

        if (state.GetState() == GameController.GameState.PlayerTurn)
        {
            if (!inPlay)
            {
                if (Application.isEditor)//check if game is running in editor for testing
                {
                    MouseInput();
                }
                TouchInput();
            }
        }
        angle = initialMousePos - finalMousePos;

        Debug.DrawLine(finalMousePos, angle, Color.red);
        //subtract final mouse point by distance between initial and transform pos
        if (inPlay)
        {

            foreach(Transform ball in balls)//check balls for inPLay bool, if false return to shoot pos
            {
                if (!ball.GetComponent<Ball>().inPlay)
                {
                    ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    ball.position = transform.position;
                }
                else if (ball.GetComponent<Ball>().inPlay)
                {
                    //if the ball is in play and has a y velocity of 0ish increase -y velocity
                    if(ball.GetComponent<Rigidbody2D>().velocity.y <= 0 && ball.GetComponent<Rigidbody2D>().velocity.y >= -0.25f)
                    {
                        ball.GetComponent<Rigidbody2D>().velocity = new Vector2(ball.GetComponent<Rigidbody2D>().velocity.x, -0.5f);
                    }
                }
            }
            if(numBallsInPlay <= 0 && state.GetState() == GameController.GameState.PlayerTurn)
            {
                inPlay = false;
                state.ChangeState(GameController.GameState.EnemyTurn);
            }
        }

        //if the player won, reset balls
        if(state.GetState() == GameController.GameState.PlayerWin)
        {
            ResetBalls();
        }
    }

    public void TouchInput()
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        if (Input.touchCount > 0)
        {
            touchInput = Input.GetTouch(0);

            if(touchInput.phase == TouchPhase.Began)//player touch screen
            {
                initialMousePos = touchPos;
            }

            if(touchInput.phase == TouchPhase.Ended)
            {
                if(Mathf.Abs(angle.y * angle.x) > minShootPower)
                {
                    shootLine.SetPosition(1, transform.position);
                    StartCoroutine(ShootBalls(interval));
                }
            }

            if(touchInput.phase == TouchPhase.Moved)
            {
                finalMousePos = touchPos;
                Vector3 initialToTransform = initialMousePos - transform.position;
                shootLine.SetPosition(0, transform.position);
                Vector3 aimLine = Vector3.Reflect(touchPos - initialToTransform, Vector3.right);
                aimLine.y *= -1;
                shootLine.SetPosition(1, aimLine / 2);
            }
        }
    }

    public void MouseInput()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        if (Input.GetMouseButtonDown(0))
        {
            //when mousebutton clicked store the position
            initialMousePos = mousePos;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(Mathf.Abs(angle.y * angle.x) > minShootPower)
            {
                shootLine.SetPosition(1, transform.position);
                StartCoroutine(ShootBalls(interval));
            }
        }

        if (Input.GetMouseButton(0))
        {
            finalMousePos = mousePos;
            Vector3 initialToTransform = initialMousePos - transform.position;
            shootLine.SetPosition(0, transform.position);
            Vector3 aimLine = Vector3.Reflect(mousePos - initialToTransform, Vector3.right);
            aimLine.y *= -1;
            shootLine.SetPosition(1, aimLine / 2);
        }
    }

    private void CreateBall(GameObject ball)
    {
        GameObject newBall = Instantiate(ball, transform.position, Quaternion.identity);
        balls.Add(newBall.transform);
    }

    private void DeleteBall(GameObject ball)
    {
        balls.Remove(ball.transform);
        Destroy(ball);
    }
    IEnumerator ShootBalls(float interval)
    {
        inPlay = true;

        for (int i = 0; i < balls.Count; i++)
        {
            //take the vector from initialMousePos and FinalMousePos(subtract both mouse positions by the distance from ball position)
            Vector2 mouseShootLine = finalMousePos - initialMousePos;
            Vector2 ballShootLine = new Vector2(mouseShootLine.x - balls[i].position.x, mouseShootLine.y - balls[i].position.y);
            //translate it to the ball position
            balls[i].GetComponent<Rigidbody2D>().velocity = -ballShootLine * speedModifier;
            balls[i].GetComponent<Ball>().inPlay = true;
            numBallsInPlay++;
            yield return new WaitForSeconds(interval);
        }

    }
    private void ResetBalls()
    {
        for(int i = 0; i < balls.Count; i++)
        {
            DeleteBall(balls[i].gameObject);
        }
        if(balls.Count <= 0)
        {
            CreateBall(ballDefault);
        }
    }
    public void BallCount(int dif)
    {
        numBalls += dif;
        CreateBall(ballDefault);
    }
    public int GetBallCount()
    {
        return balls.Count;
    }
    public void SetBallCount(int count)
    {
        numBalls = count;
    }

}
