using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/*Wizard copy, displays some dialogs, 
 * doesn't fight */
public class WizardCopy : MonoBehaviour
{
    // References
    private GameObject playerObject;
    public Spawner spawner;
    //Events and delegates
    public delegate void DisplayTextDelegate(string[] text, float displayDuration, float afterDelay);
    public static event DisplayTextDelegate OnDisplayText;

    // Start is called on scene load
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(CheckForPlayerCoroutine());
    }
    // If player is nearby, display dialog
    IEnumerator CheckForPlayerCoroutine()
    {
        while (true)
        {
            if (playerObject != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, playerObject.transform.position);

                if (distanceToPlayer <= 3f)
                {
                    string[] text = new string[7];
                    text[0] = "Robber: No, not just you!";
                    text[1] = "Wizard: You look surprised. I must admit, your determination has impressed me a lot. You've reached the very top of the mountain despite numerous adversities.";
                    text[2] = "Robber: Why are you here? Where are the mystical beings and other wizards? ";
                    text[3] = "Wizard: The wizards were too busy summoning astral beings. I killed them all when they were distracted.";
                    text[4] = "Now, all the astral power will belong to me.";
                    text[5] = "I cast a curse on you because I needed an experimental subject. Now I see the curse works, turning people into powerful warriors.";
                    text[6] = "At the same time, it kills them, making them depart before they start to rebel. Everything went according to my plan.";
                    OnDisplayText.Invoke(text, 10, 5);
                    Rigidbody2D playerRigidbody = playerObject.GetComponent<Rigidbody2D>();
                    playerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
                    yield return new WaitForSeconds(75);
                    StartCoroutine(FlyAway());
                    playerRigidbody.constraints = RigidbodyConstraints2D.None;
                    playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    spawner.SensorRadius = 50.0f;
                    break;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
        // Fly away and perish
        IEnumerator FlyAway()
        {
            Vector2 direction = new Vector2(1, 1);
            float timer = 0f;
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            while (timer < 10f)
            {
                this.gameObject.transform.Translate(direction.normalized * 5.0f * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }
            Destroy(this.gameObject);
        }
    }

}
