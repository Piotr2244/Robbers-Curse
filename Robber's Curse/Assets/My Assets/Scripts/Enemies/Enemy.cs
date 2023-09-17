using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected float speed;
    protected float jumpForce;
    protected float health;
    protected float damage;
    protected bool isAttacking = false;
    [SerializeField] float maxLeft;
    [SerializeField] float maxRight;
    [SerializeField] bool moveRight;
    [SerializeField] protected float attackRange;
    [SerializeField] protected LayerMask playerLayer;

    protected Animator animator;
    protected Rigidbody2D body2d;

    private bool originalDirection = true;



    // Start is called before the first frame update
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

    //Enemy Types:

    //moves from left to right and does melee attacks
    protected void CasualEnemy()
    {
        if (PlayerInRange())
            Attack();
        else
        {
            isAttacking = false;
            movingLeftRight();
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
    }
}
