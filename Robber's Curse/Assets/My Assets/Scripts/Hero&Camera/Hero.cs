using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Linq;
using Unity.Mathematics;
using System;

public class Hero : MonoBehaviour
{

    public float speed = 4.0f;
    public float jumpForce = 9f;
    public float Maxhealth = 20.0f;
    public float health = 50.0f;
    public float MaxMana = 50.0f;
    public float mana = 20.0f;
    public float toxic = 0f; //max is 100;
    public float damage = 2.0f;
    public float attackRange = 0.5f;
    public float gold = 0;

    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;
    private bool isAttacking = false;

    public Transform attackPoint;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    public LayerMask enemyLayers;

    ////// SPELLS //////
    bool[] spellLearned = Enumerable.Repeat(true, 4).ToArray();// true means hero knows this spell
    public int spellIndex = 0;
    public GameObject fireballPrefab; //INDEX 1
    public GameObject windSpellPrefab; //INDEX 2
    public GameObject FireCloak; //INDEX 3
    public GameObject DeathSpell; //INDEX 4
    public GameObject FireRain; //INDEX 5

    //particles:
    public ParticleSystem blood;
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }

    // Update is called once per frame
    void Update()
    {


        if (health <= 0)
            m_animator.SetBool("Death", true);
        else
        {
            groundHero();
            moveHero();
            meleeAttack();
            MagicAttack();
            changeSpell();
        }

    }

    private void groundHero()
    {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }
        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }
    }
    private void moveHero()
    {
        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        m_body2d.velocity = new Vector2(inputX * speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        //Change between idle and combat idle
        if (Input.GetKeyDown("f"))
        {
            m_combatIdle = !m_combatIdle;
            if (!m_combatIdle)
            {
                speed += 2.0f;
                damage -= 1.0f;
                jumpForce += 1.5f;
            }
            if (m_combatIdle)
            {
                speed -= 2.0f;
                damage += 1.0f;
                jumpForce -= 1.5f;
            }
        }

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }

    private void meleeAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(MeleeAttackCoroutine());
        }
    }

    private IEnumerator MeleeAttackCoroutine()
    {
        isAttacking = true;
        m_animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);

        Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in HitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }

        isAttacking = false;
    }
    private void changeSpell()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            do
            {
                spellIndex++;
                try
                {
                    if (spellLearned[spellIndex - 1] == true)
                        break;
                }
                catch //spellindex was 5
                {
                    break;
                }
            } while (spellIndex != 0);
            if (spellIndex > 5)
                spellIndex = 0;
        }
    }
    public void MagicAttack()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (spellIndex == 1)
            {
                if (fireballPrefab != null)
                {
                    if (mana >= 8f)
                    {
                        mana -= 8f;
                        Vector3 playerPosition = transform.position;
                        playerPosition.y += 0.5f;
                        Instantiate(fireballPrefab, playerPosition, Quaternion.identity);
                    }
                }
            }
            if (spellIndex == 2)
            {
                if (windSpellPrefab != null)
                {
                    if (mana >= 5f)
                    {
                        mana -= 5f;
                        Vector3 playerPosition = transform.position;
                        playerPosition.y += 0.5f;
                        Instantiate(windSpellPrefab, playerPosition, Quaternion.identity);
                    }
                }
            }
            if (spellIndex == 3)
            {
                if (FireCloak != null)
                {
                    if (mana >= 10f)
                    {
                        mana -= 10f;
                        Vector3 playerPosition = transform.position;
                        playerPosition.y += 0.5f;
                        Instantiate(FireCloak, playerPosition, Quaternion.identity);
                    }
                }
            }
            if (spellIndex == 4)
            {
                if (DeathSpell != null)
                {
                    if (mana >= 20f)
                    {
                        mana -= 20f;
                        float randomX;
                        System.Random random = new System.Random();
                        for (int x = 0; x <= 10; x++)
                        {
                            randomX = random.Next(-10, 10);
                            Vector3 playerPosition = transform.position;
                            playerPosition.x += randomX;
                            randomX = random.Next(-25, -15);
                            playerPosition.y += randomX;
                            Instantiate(DeathSpell, playerPosition, Quaternion.identity);
                        }
                    }
                }
            }
            if (spellIndex == 5)
            {
                if (FireRain != null)
                {
                    if (mana >= 50f)
                    {
                        mana -= 50f;
                        float randomX;
                        System.Random random = new System.Random();
                        for (int x = 0; x <= 30; x++)
                        {
                            randomX = random.Next(-15, 15);
                            Vector3 playerPosition = transform.position;
                            playerPosition.x += randomX;
                            randomX = random.Next(20, 25);
                            playerPosition.y += randomX;
                            playerPosition.y += x;
                            Instantiate(FireRain, playerPosition, Quaternion.identity);
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        blood.Play();
    }



}
