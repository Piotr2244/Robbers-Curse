using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Hero : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 8.5f;
    public float Maxhealth = 20.0f;
    public float health = 20.0f;
    public float MaxMana = 10.0f;
    public float mana = 10.0f;
    public float toxic = 0f; //max is 100;
    [SerializeField] float damage = 2.0f;
    public float attackRange = 0.5f;

    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;
    private bool isAttacking = false;

    public Transform attackPoint;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    public LayerMask enemyLayers;

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
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);


        //Attack


        //Change between idle and combat idle
        if (Input.GetKeyDown("f"))
        {
            m_combatIdle = !m_combatIdle;
            if (!m_combatIdle)
            {
                m_speed += 2.0f;
                damage -= 1.0f;
                m_jumpForce += 1.5f;
            }
            if (m_combatIdle)
            {
                m_speed -= 2.0f;
                damage += 1.0f;
                m_jumpForce -= 1.5f;
            }
        }

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
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
