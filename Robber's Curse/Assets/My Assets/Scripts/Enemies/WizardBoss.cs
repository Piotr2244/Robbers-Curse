using System.Collections;
using UnityEngine;
/* Main enemy, first talks with player with dialog
 * display, then, depending on hero choice, attacks
 * the player or ends game */
public class WizardBoss : Enemy
{
    // Components and references
    public GameObject barricade;
    public GameObject Closingbarricade;
    public GameObject Fire;
    public GameObject DecisionCanvas;
    public SpriteRenderer EndgameBackGround;
    public GameObject button;
    private GameObject playerObject;
    // Variables
    public bool isFighting = false;
    private bool isRemoving = false;
    private bool closingGate = false;
    public string[] plotStory;
    private bool startedChasing = false;
    private bool afterDialog = false;
    // Events and Delegates
    public delegate void ChangeTrack(int index = 7);
    public static event ChangeTrack ChangeMusic;
    public delegate void RestoreTrack();
    public static event RestoreTrack RestoreMusic;
    public delegate void DisplayTextDelegate(string[] text, float displayDuration, float afterDelay);
    public static event DisplayTextDelegate OnDisplayText;

    // Constructor
    public WizardBoss()
    {
        speed = 2.0f;
        attackRange = 1.3f;
        health = 200.0f;
        attackSpeed = 0.2f;
        damage = 0.5f;
    }
    // Start is called on scene load
    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        EndgameBackGround = transform.Find("EndGame/BlackScreen").GetComponent<SpriteRenderer>();
        body2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(CheckForPlayerCoroutine());
        button = transform.Find("EndGame/Quit").gameObject;
        button.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (afterDialog)
            SmartEnemy();
        if (!isAlive)
        {
            if (!isRemoving)
                StartCoroutine(RemoveBarricade());
            //animator.SetBool("Dead", true);
        }
        if (isChasing && !startedChasing)
        {
            startedChasing = true;
            if (!closingGate)
                StartCoroutine(CloseGate());
            SensorRadius = 50;
            gameObject.tag = "Enemy";
        }
    }
    // Removing barricade to leave the battlefield
    private IEnumerator RemoveBarricade()
    {
        isRemoving = true;
        Fire.SetActive(false);
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
        ChangeMusic(9);
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
    // Check if player is in range, if so, start dialog display methods
    IEnumerator CheckForPlayerCoroutine()
    {
        while (true)
        {


            if (playerObject != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, playerObject.transform.position);

                if (distanceToPlayer <= 5f)
                {
                    OnDisplayText.Invoke(plotStory, 5, 15);
                    Rigidbody2D playerRigidbody = playerObject.GetComponent<Rigidbody2D>();
                    playerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;

                    yield return new WaitForSeconds(31);
                    DecisionCanvas.SetActive(true);
                    break;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    // Exit game
    public void ExitGameClick()
    {
        Application.Quit();
    }
    // Make screen black slowly
    private IEnumerator BlackScreen()
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime;
            Color newColor = EndgameBackGround.color;
            newColor.a = alpha;
            EndgameBackGround.color = newColor;
            yield return new WaitForSeconds(0.05f);
        }
    }
    // Endgame display if player decides to run from final boss
    private IEnumerator RunEndingDisplay()
    {
        StartCoroutine(BlackScreen());
        yield return new WaitForSeconds(7f);
        string[] text = new string[6];
        text[0] = "You knew you wouldn't be able to defeat the wizard. Joining him, however, would have been the ruin of the world.";
        text[1] = "With tears in your eyes, you turned away and fled as fast as you could.";
        text[2] = "You left the mountain area, never returning to its vicinity.";
        text[3] = "After a few weeks, the curse took its toll, the illness completely ravaged you, draining all life forces and killing you once and for all";
        text[4] = "You died not knowing if the wizard succeeded in taking over the world.";
        text[5] = "You perish, but perhaps it's better than serving evil. Yet, are you certain you couldn't have defeated the wizard?";
        OnDisplayText.Invoke(text, 10, 5);
        yield return new WaitForSeconds(65f);
        button.SetActive(true);
    }
    // Endgame display if player decides to joun the final boss
    private IEnumerator JoinEndingDisplay()
    {
        StartCoroutine(BlackScreen());
        yield return new WaitForSeconds(7f);
        string[] text = new string[6];
        text[0] = "The wizard, with the help of astral beings, cured your illness as promised.";
        text[1] = "In a few weeks, the curse affected hundreds of unfortunate adventure seekers who quickly began to feel its effects.";
        text[2] = "The sorcerer promised the infected a cure in exchange for their loyality and service.";
        text[3] = "Every single one agreed. With you at the helm, the cursed warriors conquered each kingdom, one by one.";
        text[4] = "The wizard seized control of the entire known world, establishing his reign of terror.";
        text[5] = "You achieved the goal of your journey, but was it worth what befell the rest of the world?";
        OnDisplayText.Invoke(text, 10, 5);
        yield return new WaitForSeconds(5f);
        button.SetActive(true);
    }
    // Choose to run from enemy
    public void RunButton()
    {
        DecisionCanvas.SetActive(false);
        StartCoroutine(RunEndingDisplay());
    }
    // Choose to join the enemy
    public void JoinButton()
    {
        DecisionCanvas.SetActive(false);
        StartCoroutine(JoinEndingDisplay());
    }
    // Choose to figh the enemy
    public void FightButton()
    {
        DecisionCanvas.SetActive(false);
        Rigidbody2D playerRigidbody = playerObject.GetComponent<Rigidbody2D>();
        playerRigidbody.constraints = RigidbodyConstraints2D.None;
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        afterDialog = true;

    }
}
