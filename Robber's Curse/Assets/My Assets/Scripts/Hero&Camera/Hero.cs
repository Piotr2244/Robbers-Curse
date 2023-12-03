using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Linq;
using Unity.Mathematics;
using System;
using static Hero;

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

    public float manaRegen = 1;
    public float HpRegen = 0;

    private bool grounded = false;
    private bool combatIdle = false;
    private bool isAttacking = false;
    private bool isDead = false;

    public Transform attackPoint;
    private Animator animator;
    private Rigidbody2D body2d;
    private Sensor_Bandit groundSensor;
    public LayerMask enemyLayers;

    ////// SPELLS //////
    public bool[] spellLearned = Enumerable.Repeat(false, 5).ToArray();// true means hero knows this spell
    public int spellIndex = 0;
    public GameObject fireballPrefab; //INDEX 1
    public GameObject windSpellPrefab; //INDEX 2
    public GameObject FireCloak; //INDEX 3
    public GameObject DeathSpell; //INDEX 4
    public GameObject FireRain; //INDEX 5

    //particles:
    public ParticleSystem blood;
    public ParticleSystem toxicSplash;

    //current hero state
    private SingleState CurrentState;

    //State delegate
    public delegate void SendStateUpdate(int AtributeIndex, float ChangeValue, float DelayValue = 0);
    public static event SendStateUpdate UpdateState;

    //temp values
    private bool ToxicBoostActive = false;
    private float SpellOverloadCountdown = 0.0f;
    private bool isReturningToLive = false;

    //sounds
    public AudioSource audioSrc;
    public AudioClip MeleeSound;
    public AudioClip FireBallSound;
    public AudioClip MagicWindSound;
    public AudioClip FireCloakSound;
    public AudioClip SoulSpellSound;
    public AudioClip jumpSound;
    public AudioClip stepSound;
    public AudioClip landSound;

    //LOADING STATS FROM ANOTHER SCENE
    public bool LoadFromPrev = false;
    public HeroStatus heroStatus;
    private void LoadStats()
    {
        if (LoadFromPrev)
        {
            GameObject statusHolder = GameObject.FindGameObjectWithTag("StatusHolder");
            if (statusHolder != null)
            {
                heroStatus = statusHolder.GetComponent<HeroStatus>();

                speed = heroStatus.speed;
                jumpForce = heroStatus.jumpForce;
                Maxhealth = heroStatus.Maxhealth;
                health = heroStatus.health;
                MaxMana = heroStatus.MaxMana;
                mana = heroStatus.mana;
                toxic = heroStatus.toxic;
                damage = heroStatus.damage;
                attackRange = heroStatus.attackRange;
                gold = heroStatus.gold;
            }
        }
    }
    private void Awake()
    {
        SingleState.ChangeState += GetStateAtributes;
        SingleState.UndoState += UndoStateAtributes;
        Coin.OnItemCollision += CollectGold;
        //DontDestroyOnLoad(gameObject);
        LoadStats();

    }
    void Start()
    {
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        StartCoroutine(Regeneration());
        StartCoroutine(FullToxineLevelHandler()); //
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            animator.SetBool("Death", true);
            if (toxic < 95)
            {
                if (!isReturningToLive)
                {
                    isReturningToLive = true;
                    StartCoroutine(ReturnToLive());
                }
            }
            else if (!isDead)
            {
                isDead = true;
                FinalDeath();
            }


        }
        else
        {
            groundHero();
            moveHero();
            meleeAttack();
            MagicAttack();
            changeSpell();
            ToxicBoost();
        }

    }

    private void groundHero()
    {
        //Check if character just landed on the ground
        if (!grounded && groundSensor.State())
        {
            audioSrc.clip = landSound;
            audioSrc.Play();
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }
        //Check if character just started falling
        if (grounded && !groundSensor.State())
        {
            grounded = false;
            animator.SetBool("Grounded", grounded);
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
        body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);

        //Set AirSpeed in animator
        animator.SetFloat("AirSpeed", body2d.velocity.y);

        //Change between idle and combat idle
        if (Input.GetKeyDown("f"))
        {
            combatIdle = !combatIdle;
            if (!combatIdle)
            {
                speed += 2.0f;
                damage -= 1.0f;
                jumpForce += 1.5f;
            }
            if (combatIdle)
            {
                speed -= 2.0f;
                damage += 1.0f;
                jumpForce -= 1.5f;
            }
            UpdateState.Invoke(1, 0.5f, 5);
        }

        //Jump
        else if (Input.GetKeyDown("space") && grounded)
        {
            audioSrc.clip = jumpSound;
            audioSrc.Play();
            animator.SetTrigger("Jump");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
            UpdateState.Invoke(1, 0.01f, 2);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            animator.SetInteger("AnimState", 2);
            //audioSrc.clip = stepSound;
            //audioSrc.Play();
        }

        //Combat Idle
        else if (combatIdle)
            animator.SetInteger("AnimState", 1);

        //Idle
        else
            animator.SetInteger("AnimState", 0);
    }

    private void meleeAttack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {

            StartCoroutine(MeleeAttackCoroutine());
        }
    }

    private IEnumerator Regeneration()
    {
        int secondCounter = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            secondCounter++;
            if (secondCounter % 10 == 0)
            {
                if (HpRegen > 0)
                    if (health + HpRegen < Maxhealth)
                        health += HpRegen;
                if (manaRegen > 0)
                    if (mana + manaRegen < MaxMana)
                        mana += manaRegen;
            }
            if (SpellOverloadCountdown > 0)
                SpellOverloadCountdown -= 1;
        }
    }

    private IEnumerator MeleeAttackCoroutine()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        audioSrc.clip = MeleeSound;
        audioSrc.Play();
        Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in HitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
            UpdateState.Invoke(1, 0.05f, 2);

        }
        UpdateState.Invoke(1, 0.01f, 2);
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
    private void ToxicBoost()
    {
        if (Input.GetKeyDown(KeyCode.R) && ToxicBoostActive == false)
        {

            toxicSplash.Play();
            ToxicBoostActive = true;
            StartCoroutine(ToxicBoostCorutine());
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
                        audioSrc.clip = FireBallSound;
                        audioSrc.Play();
                        mana -= 8f;
                        Vector3 playerPosition = transform.position;
                        playerPosition.y += 0.5f;
                        Instantiate(fireballPrefab, playerPosition, Quaternion.identity);
                        SpellOverloadController(3);
                    }
                }
            }
            if (spellIndex == 2)
            {
                if (windSpellPrefab != null)
                {
                    if (mana >= 5f)
                    {
                        audioSrc.clip = MagicWindSound;
                        audioSrc.Play();
                        mana -= 5f;
                        Vector3 playerPosition = transform.position;
                        playerPosition.y += 0.5f;
                        Instantiate(windSpellPrefab, playerPosition, Quaternion.identity);
                        SpellOverloadController(3);
                    }
                }
            }
            if (spellIndex == 3)
            {
                if (FireCloak != null)
                {
                    if (mana >= 10f)
                    {
                        audioSrc.clip = FireCloakSound;
                        audioSrc.Play();
                        mana -= 10f;
                        Vector3 playerPosition = transform.position;
                        playerPosition.y += 0.5f;
                        Instantiate(FireCloak, playerPosition, Quaternion.identity);
                        SpellOverloadController(10);
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
                        audioSrc.clip = SoulSpellSound;
                        audioSrc.Play();
                        for (int x = 0; x <= 10; x++)
                        {
                            randomX = random.Next(-10, 10);
                            Vector3 playerPosition = transform.position;
                            playerPosition.x += randomX;
                            randomX = random.Next(-25, -15);
                            playerPosition.y += randomX;
                            Instantiate(DeathSpell, playerPosition, Quaternion.identity);
                        }
                        SpellOverloadController(15);
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
                            audioSrc.clip = FireBallSound;
                            audioSrc.Play();
                            randomX = random.Next(-15, 15);
                            Vector3 playerPosition = transform.position;
                            playerPosition.x += randomX;
                            randomX = random.Next(20, 25);
                            playerPosition.y += randomX;
                            playerPosition.y += x;
                            Instantiate(FireRain, playerPosition, Quaternion.identity);
                        }
                        SpellOverloadController(20);
                    }
                }
            }
        }
    }

    public void GetStateAtributes(SingleState state)
    {
        CurrentState = state;
        speed += CurrentState.speed;
        jumpForce += CurrentState.jump;
        damage += CurrentState.strength;
        HpRegen += CurrentState.hpRegen;
        manaRegen += CurrentState.ManaRegen;
    }
    public void UndoStateAtributes()
    {
        if (CurrentState == null)
            return;
        speed -= CurrentState.speed;
        jumpForce -= CurrentState.jump;
        damage -= CurrentState.strength;
        HpRegen -= CurrentState.hpRegen;
        manaRegen -= CurrentState.ManaRegen;
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
        UpdateState.Invoke(2, 1f);
    }

    private void OnDisable()
    {
        SingleState.ChangeState -= GetStateAtributes;
        SingleState.UndoState -= UndoStateAtributes;
        Coin.OnItemCollision -= CollectGold;
    }

    private IEnumerator ToxicBoostCorutine()
    {
        audioSrc.clip = FireCloakSound;
        audioSrc.Play();
        toxic += 1;
        jumpForce += 6;
        damage += 2;
        speed += 2;
        if (mana + 100 < MaxMana)
            mana += 100;
        else
            mana = MaxMana;
        UpdateState.Invoke(3, 1, 0);
        yield return new WaitForSeconds(15);
        toxic += 1;
        jumpForce -= 6;
        damage -= 2;
        speed -= 2;
        UpdateState.Invoke(3, 1, 20);
        ToxicBoostActive = false;
        audioSrc.clip = FireCloakSound;
        audioSrc.Play();

    }

    private IEnumerator ReturnToLive()
    {
        yield return new WaitForSeconds(3);
        health = Maxhealth;
        animator.SetBool("Recover", true);
        toxicSplash.Play();
        toxic += 5;
        yield return new WaitForSeconds(1);
        audioSrc.clip = FireCloakSound;
        audioSrc.Play();
        animator.SetBool("Recover", false);
        isReturningToLive = false;
        UpdateState.Invoke(3, 1, 0);
        UpdateState.Invoke(3, 1, 10);
        UpdateState.Invoke(3, 1, 20);
        UpdateState.Invoke(3, 1, 30);

    }

    private void FinalDeath()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (gameManager != null)
        {
            GameOver gameOverScript = gameManager.GetComponent<GameOver>();
            if (gameOverScript != null)
            {
                gameOverScript.StartCoroutine("GameIsOver");
            }
        }
        for (int x = 0; x <= 50; x++)
        {
            toxicSplash.Play();
            System.Random random = new System.Random();
            int randomX = random.Next(-10, 10);
            Vector3 playerPosition = transform.position;
            playerPosition.x += randomX;
            randomX = random.Next(-15, -5);
            playerPosition.y += randomX;
            Instantiate(DeathSpell, playerPosition, Quaternion.identity);
        }
    }

    private IEnumerator FullToxineLevelHandler()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (toxic >= 100)
            {
                yield return new WaitForSeconds(1);
                health -= 0.5f;
            }
        }
    }
    private void SpellOverloadController(float time)
    {
        if (SpellOverloadCountdown > 0)
        {
            UpdateState.Invoke(3, 0.1f);
            toxic += 0.1f;
        }
        if (SpellOverloadCountdown > 20)
        {
            UpdateState.Invoke(3, 0.2f);
            toxic += 0.2f;
        }
        SpellOverloadCountdown += time;
    }

    private void CollectGold(Coin coin)
    {
        gold += coin.value;
        Destroy(coin.gameObject);
    }
}
