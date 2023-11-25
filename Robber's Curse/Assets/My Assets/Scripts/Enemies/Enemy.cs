using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected float speed;
    protected float jumpForce;
    public float health;
    protected float damage;
    public bool isAlive = true;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackSpeed; // the lower speed, the faster attacks, works like a delay between attacks

    protected bool isAttacking = false;
    private bool canAttack = true;
    public float maxLeft;
    public float maxRight;
    public bool moveRight;

    public LayerMask playerLayer;
    protected Animator animator;
    protected Rigidbody2D body2d;
    public Transform attackPoint;

    public int coinAmount = 0;
    public bool moneyDroped = false;
    public GameObject coin;


    //particles:
    public ParticleSystem blood;

    void Start()
    {
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        maxLeft = (-maxLeft) + transform.position.x;
        maxRight += transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
    }

    //moves from left to right and does melee attacks
    protected void CasualEnemy()
    {
        if (isAlive)
        {
            StillAlive();
            if (PlayerInRange())
                Attack();
            else
            {
                isAttacking = false;
                movingLeftRight();
            }
        }
        else
        {
            dropMoney();
            Death();
        }


    }
    protected void movingLeftRight()
    {
        if (!isAttacking)
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
            animator.SetBool("Attack", false);
            float currentPositionX = transform.position.x;
            float direction = 1.0f;
            if (!moveRight)
            {
                direction = -1.0f;
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            if (moveRight && currentPositionX >= maxRight)
            {
                moveRight = false;
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else if (!moveRight && currentPositionX <= maxLeft)
            {
                moveRight = true;
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            float newPositionX = currentPositionX + (speed * direction * Time.deltaTime);
            transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
        }
    }
    protected bool PlayerInRange()
    {
        Vector2 pos = transform.position;
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(pos, attackRange, playerLayer);
        return hitPlayers.Length > 0;
    }
    protected void Attack()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        isAttacking = true;
        animator.SetBool("Attack", true);
        Vector2 pos = transform.position;
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(pos, attackRange, playerLayer);

        if (hitPlayers[0].transform.position.x < this.transform.position.x && moveRight)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        if (hitPlayers[0].transform.position.x > this.transform.position.x && !moveRight)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        if (canAttack)
            StartCoroutine(MeleeAttackCoroutine(attackSpeed));

    }
    public void TakeDamage(float damage)
    {
        blood.Play();
        health -= damage;
    }
    protected void StillAlive()
    {
        if (health <= 0)
            isAlive = false;
    }

    private IEnumerator MeleeAttackCoroutine(float delay)
    {
        canAttack = false;
        isAttacking = true;
        yield return new WaitForSeconds(delay);

        Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D enemy in HitEnemies)
        {
            enemy.GetComponent<Hero>().TakeDamage(damage);
        }

        isAttacking = false;
        canAttack = true;

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void Death()
    {
        GetComponent<Rigidbody2D>().isKinematic = false;
        try
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        catch
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void dropMoney()
    {
        if (!moneyDroped)
            if (coin != null)
            {
                moneyDroped = true;
                Vector3 enemyPos = transform.position;
                enemyPos.y += 0.5f;
                for (int x = 0; x < coinAmount; x++)
                    Instantiate(coin, enemyPos, Quaternion.identity);
            }
    }

}

