using UnityEngine;
using System.Collections;

/* 
 * Third Boss, follows the player and attacks on short range.
 * Enemy can jump and empower his assets if his health is low
 */
public class EvilHero : Enemy
{
    // References
    public GameObject barricade;
    public GameObject Closingbarricade;
    // Variables
    public bool isFighting = false;
    private bool isRemoving = false;
    private bool closingGate = false;
    private bool startedChasing = false;
    private bool empowered = false;
    private bool afterDialog = false;
    private float jumpCooldown = 0;
    private float jumpForce = 10;
    public string[] plotStory;
    // Events and delegates
    public delegate void ChangeTrack(int index = 7);
    public static event ChangeTrack ChangeMusic;
    public delegate void RestoreTrack();
    public static event RestoreTrack RestoreMusic;
    public delegate void DisplayTextDelegate(string[] text, float displayDuration, float afterDelay);
    public static event DisplayTextDelegate OnDisplayText;
    // Components
    public ParticleSystem toxic;
    public AudioSource toxicSound;


    // Constructor
    public EvilHero()
    {
        speed = 2.0f;
        attackRange = 1.0f;
        health = 120.0f;
        attackSpeed = 1.0f;
        damage = 5.0f;
        canJump = true;
        isBoss = true;
    }
    // Start is called on scene load
    private void Start()
    {
        body2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(CheckPlayerAbove());
        toxicSound = transform.GetComponent<AudioSource>();
        StartCoroutine(CheckForPlayerCoroutine());
    }
    // Update is called once per frame
    void Update()
    {
        if (afterDialog)
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
            gameObject.tag = "Enemy";
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
    // Removing barricade to leave the battlefield
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
    // Closing the gate that prevents from escaping the battle 
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
    // Check if player is above, if so, perform jump
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
    // Reset jump cooldown to prevent jumping to often
    private IEnumerator ResetJumpCooldown()
    {
        yield return new WaitForSeconds(3f);
        canJump = true;
    }
    // Perform jump
    private void jump()
    {
        if (jumpCooldown <= 0)
        {
            animator.SetBool("Jump", true);
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            jumpCooldown = 3f;
        }
    }
    // Check if player is near, if so, do some dialog functions
    IEnumerator CheckForPlayerCoroutine()
    {
        while (true)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, playerObject.transform.position);
                if (distanceToPlayer <= 5f)
                {
                    OnDisplayText.Invoke(plotStory, 10, 1);
                    Rigidbody2D playerRigidbody = playerObject.GetComponent<Rigidbody2D>();
                    playerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
                    yield return new WaitForSeconds(41);
                    playerRigidbody.constraints = RigidbodyConstraints2D.None;
                    playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    afterDialog = true;
                    break;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
