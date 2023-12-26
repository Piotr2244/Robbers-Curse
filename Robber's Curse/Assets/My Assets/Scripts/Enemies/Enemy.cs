using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * This class contains main functionalities for game enemies
 */
public class Enemy : MonoBehaviour
{
    // Main variables
    protected float speed = 0;
    public float health = 0;
    protected float damage = 0;
    public bool isAlive = true;
    public float attackRange;
    public float attackSpeed; // The lower speed, the faster attacks, works like a delay between attacks
    // Variables
    protected bool isAttacking = false;
    private bool canAttack = true;
    public float maxLeft;
    public float maxRight;
    public bool moveRight;
    public bool fromSpawner = false;
    public int coinAmount = 0;
    public bool moneyDroped = false;
    protected bool canJump = false;
    protected bool isBoss = false;
    protected bool isChasing = false;
    public float SensorRadius = 0; //For smart enemies  
    // Components and references
    public LayerMask playerLayer;
    protected Animator animator;
    protected Rigidbody2D body2d;
    public Transform attackPoint;
    public GameObject coin;
    public AudioSource src;
    public ParticleSystem blood;
    // Events and delegates
    public delegate void SendStateUpdate(int AtributeIndex, float ChangeValue, float DelayValue = 0);
    public static event SendStateUpdate UpdateState;

    // Start is called on scene load
    void Start()
    {
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        maxLeft = (-maxLeft) + transform.position.x;
        maxRight += transform.position.x;
        src = transform.GetComponent<AudioSource>();
    }

    // Moves from left to right and does melee attacks
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
            StartCoroutine(perish());
        }
    }

    // Follows the player if he is nearby
    protected void SmartEnemy()
    {
        if (isAlive)
        {
            StillAlive();
            if (PlayerInRange())
                Attack();
            else
            {
                animator.SetBool("Stand", false);
                isAttacking = false;
                FollowPlayer();
            }
        }
        else
        {
            dropMoney();
            Death();
        }
    }

    // Simple movement method
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

    // Gets current player localisation on X axis to follow him
    protected void FollowPlayer()
    {
        if (!isAttacking)
        {
            float currentPositionX = transform.position.x;
            GetComponent<Rigidbody2D>().isKinematic = false;
            animator.SetBool("Attack", false);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, SensorRadius);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    isChasing = true;
                    animator.SetBool("Stand", false);
                    if (currentPositionX > collider.transform.position.x)
                        transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    if (currentPositionX < collider.transform.position.x)
                        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    animator.SetBool("Move", true);
                    Vector3 direction = collider.transform.position - transform.position;
                    direction.y = 0;
                    transform.position += direction * speed * Time.deltaTime;
                }
                else
                    animator.SetBool("Stand", true);
            }
        }

    }
    // Checks if the player is in range
    protected bool PlayerInRange()
    {
        Vector2 pos = transform.position;
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(pos, attackRange, playerLayer);
        return hitPlayers.Length > 0;
    }

    // Target the player (if he is in range) and try to attack him
    protected void Attack()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        isAttacking = true;
        animator.SetBool("Attack", true);
        if (animator != null && canJump)
        {
            animator.SetBool("Jump", false);
        }

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
    // Take damage from the player
    public void TakeDamage(float damage)
    {
        blood.Play();
        health -= damage;
    }
    // Check if the enemy is still alive
    protected void StillAlive()
    {
        if (health <= 0)
            isAlive = false;
    }

    // Hurt the player if he is in range for the determined amount of time
    private IEnumerator MeleeAttackCoroutine(float delay)
    {
        canAttack = false;
        isAttacking = true;
        yield return new WaitForSeconds(delay);

        Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D enemy in HitEnemies)
        {
            try
            {
                src.Play();
            }
            catch { }
            enemy.GetComponent<Hero>().TakeDamage(damage);
            if (isBoss)
                UpdateState.Invoke(1, 1f, 1f);
        }

        isAttacking = false;
        canAttack = true;
    }
    // Draw attack range
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // Change animators after death
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
    // Drop coins after death
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

    // Destroy the enemy after his defeat
    private IEnumerator perish()
    {
        if (!fromSpawner)
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }
    }


}