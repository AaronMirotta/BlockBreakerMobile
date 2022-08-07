using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    [SerializeField]
    private int maxHp;

    [SerializeField]
    private GameObject hitEffect;

    [SerializeField]
    private Text hpText;

    private int currentHp;

    private Transform ball;

    private Enemy enemy;

    private void Start()
    {
        currentHp = maxHp;
        hpText.text = currentHp.ToString();
        enemy = GameObject.FindObjectOfType<Enemy>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            int ballPower = collision.gameObject.GetComponent<Ball>().dmg;

            ball = collision.transform;

            BlockHit();
            Dmg(ballPower);
        }
    }

    private void Dmg(int dmg)
    {
        currentHp -= dmg;
        hpText.text = currentHp.ToString();
        
        if (currentHp <= 0)
        {
            enemy.TakeDmg(1);
            enemy.RemoveBlock(gameObject);
            Destroy(gameObject);
        }
    }

    private void BlockHit()//instantiate particle effect
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, -10);

        Vector2 dir = ball.position - transform.position;
        Quaternion lookDir = Quaternion.LookRotation(dir);
        GameObject effect = Instantiate(hitEffect.gameObject, transform.position,lookDir);
        effect.GetComponentInChildren<ParticleSystem>().Play();
    }

    public void SetHp(int hp)
    {
        maxHp = hp;
    }
    public void AddHp(int hp)
    {
        maxHp += hp;
    }

    public int GetHp()
    {
        return currentHp;
    }

    public float GetYPos()
    {
        return transform.position.y;
    }
}
