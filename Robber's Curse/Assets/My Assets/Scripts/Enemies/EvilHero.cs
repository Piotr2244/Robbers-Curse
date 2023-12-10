using UnityEngine;
using System.Collections;

public class EvilHero : Enemy
{

    //public RockSpawner spawner;
    public bool isFighting = false;
    public GameObject barricade;
    public GameObject Closingbarricade;
    private bool isRemoving = false;
    private bool closingGate = false;
    private float jumpCooldown = 0;
    private bool canJump = true;
    private float jumpForce = 10;

    public delegate void ChangeTrack(int index = 7);
    public static event ChangeTrack ChangeMusic;
    public delegate void RestoreTrack();
    public static event RestoreTrack RestoreMusic;

    private bool startedChasing = false;
    private bool empowered = false;

    public ParticleSystem toxic;

    public AudioSource toxicSound;
    public EvilHero()
    {
        speed = 2.0f;
        attackRange = 1.0f;
        health = 100.0f;
        attackSpeed = 1.0f;
        damage = 4.0f;
    }
    private void Start()
    {
        body2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(CheckPlayerAbove());
        toxicSound = transform.GetComponent<AudioSource>();
    }
    void Update()
    {
        //animator.SetBool("Move", true);
        SmartEnemy();
        if (jumpCooldown > 0)
        {
            jumpCooldown -= Time.deltaTime;
        }

        if (!isAlive)
        {
            if (!isRemoving)
                StartCoroutine(RemoveBarricade());
            animator.SetBool("Dead", true);
        }

        if (isChasing && !startedChasing)
        {
            startedChasing = true;
            if (!closingGate)
                StartCoroutine(CloseGate());
            SensorRadius = 25;
        }

        if (health <= 20 && !empowered)
        {
            empowered = true;
            jumpForce = 13;
            speed += 3;
            damage += 3;
            toxicSound.Play();
            toxic.Play();
        }
    }

    private IEnumerator RemoveBarricade()
    {
        isRemoving = true;
        RestoreMusic();
        for (int x = 0; x < 2000; x++)
        {
            if (barricade != null)
            {
                barricade.transform.position += new Vector3(0, -0.03f, 0);
                yield return new WaitForSeconds(0.1f);
            }
        }

    }

    private IEnumerator CloseGate()
    {
        ChangeMusic(7);
        closingGate = true;
        for (int x = 0; x < 200; x++)
        {
            if (Closingbarricade != null)
            {
                Closingbarricade.transform.position += new Vector3(0, 0.01f, 0);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    private IEnumerator CheckPlayerAbove()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (animator.GetBool("Move") == false)
                continue;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && transform.position.y < player.transform.position.y && canJump)
            {
                //animator.SetBool("Jump", true);
                jump();
                canJump = false;
                StartCoroutine(ResetJumpCooldown());
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
                animator.SetBool("Jump", false);
            }

        }
    }

    private IEnumerator ResetJumpCooldown()
    {
        yield return new WaitForSeconds(3f);
        canJump = true;
    }

    private void jump()
    {
        if (jumpCooldown <= 0)
        {
            animator.SetBool("Jump", true);
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            jumpCooldown = 3f;
        }
    }
}
