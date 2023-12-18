using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GoodEnding : MonoBehaviour
{
    public SpriteRenderer EndgameBackGround;
    private GameObject playerObject;
    public delegate void DisplayTextDelegate(string[] text, float displayDuration, float afterDelay);
    public static event DisplayTextDelegate OnDisplayText;
    public GameObject button;
    bool done = false;

    void Start()
    {
        button = transform.Find("EndGame/Quit").gameObject;
        button.SetActive(false);
        playerObject = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(CheckForPlayerCoroutine());
    }
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
    IEnumerator CheckForPlayerCoroutine()
    {
        while (true)
        {
            if (done)
                break;

            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                Vector2 playerPosition = playerObject.transform.position;
                float distanceToPlayer = Vector2.Distance(new Vector2(415.83f, 116.02f), playerPosition);
                if (distanceToPlayer <= 5f)
                {
                    StartCoroutine(BlackScreen());
                    Rigidbody2D playerRigidbody = playerObject.GetComponent<Rigidbody2D>();
                    playerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
                    yield return new WaitForSeconds(7f);
                    string[] text = new string[7];
                    text[0] = "Shaking with fear and limping from wounds, you approach the astral beings, who watch you attentively.";
                    text[1] = "With the last of your strength, you beg for healing from the illness weighing upon you.";
                    text[2] = "The intrigued creatures betray no emotions and gaze upon you.";
                    text[3] = "Finally, one speaks: \"By slaying the treacherous wizard, you have proven yourself worthy of redemption.\"";
                    text[4] = "Moments later, you lose consciousness. When you awaken, the sky is clear, and the astral beings are nowhere to be found.";
                    text[5] = "Yet, you feel something remarkable. You feel no pain; your body is healthy and full of strength. The curse has been broken! Once again, you are free to enjoy life.";
                    text[6] = "But what's next? Will you return to your former occupation of robbing houses? Or will you embark on the path of righteousness to spend the rest of your life in a worthy manner?";
                    OnDisplayText.Invoke(text, 10, 5);

                    yield return new WaitForSeconds(75f);
                    button.SetActive(true);
                    done = true;
                }
            }
            else
                Debug.Log("333");
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
