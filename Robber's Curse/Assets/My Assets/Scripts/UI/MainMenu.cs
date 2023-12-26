using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/* Main menu controller, alows to chose menu
 * options by clicking a proper button */
public class MainMenu : MonoBehaviour
{
    // Variables and references
    private GameObject button1;
    private GameObject button2;
    private GameObject button3;
    private GameObject button4;
    private GameObject credits;
    private GameObject gameInfo;
    private GameObject title;
    private float pos1, pos2, pos3, pos4;
    private SpriteRenderer blackScreen;
    public string[] plotStory;
    public SpriteRenderer[] sprites;
    // Events and delegates
    public delegate void DisplayTextDelegate(string[] text, float displayDuration, float afterDelay);
    public static event DisplayTextDelegate OnDisplayText;

    // Start is called before the first frame update
    void Start()
    {
        button1 = transform.Find("MainButtons/Start").gameObject;
        button2 = transform.Find("MainButtons/Info").gameObject;
        button3 = transform.Find("MainButtons/Credits").gameObject;
        button4 = transform.Find("MainButtons/Exit").gameObject;
        title = transform.Find("MainButtons/Text (TMP)").gameObject;
        credits = transform.Find("CreditsText").gameObject;
        gameInfo = transform.Find("InfoText").gameObject;
        credits.SetActive(false);
        gameInfo.SetActive(false);

        pos1 = button1.transform.position.x;
        pos2 = button2.transform.position.x;
        pos3 = button3.transform.position.x;
        pos4 = button4.transform.position.x;
        blackScreen = transform.Find("BlackScreen").GetComponent<SpriteRenderer>();
        StartCoroutine(MakeMenu());

    }
    // Make buttons move on camera position
    private IEnumerator MoveToPositionX(GameObject gm)
    {
        Vector3 targetPosition = new Vector3(0, gm.transform.position.y, gm.transform.position.z);
        while (Vector3.Distance(gm.transform.position, targetPosition) > 0.01f)
        {

            gm.transform.position = Vector3.MoveTowards(gm.transform.position, targetPosition, 15f * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
    }
    // Make menu visible and show buttons
    IEnumerator MakeMenu()
    {
        Color newColor = blackScreen.color;
        yield return new WaitForSeconds(1f);
        float alpha = 1f;
        blackScreen.color = newColor;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime;
            newColor.a = alpha;
            blackScreen.color = newColor;
            yield return new WaitForSeconds(0.02f);
        }
        ReturnToMenu();
    }
    // Put buttons on their previous position
    private void ReturnToPosition(GameObject gm, float position)
    {
        gm.transform.position = new Vector3(position, gm.transform.position.y, gm.transform.position.z);
    }
    // Start game
    public void StartGameClick()
    {
        StartCoroutine(StartGame());
    }
    // Make game start performance, display plot and images, then start game
    public IEnumerator StartGame()
    {
        Color newColor = blackScreen.color;
        yield return new WaitForSeconds(1f);
        float alpha = 0f;
        blackScreen.color = newColor;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime;
            newColor.a = alpha;
            blackScreen.color = newColor;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(1f);
        OnDisplayText.Invoke(plotStory, 15, 20);
        yield return new WaitForSeconds(5f);
        StartCoroutine(ShowImage(sprites[0])); //hero
        yield return new WaitForSeconds(15f);
        StartCoroutine(ShowImage(sprites[1])); //wizard
        yield return new WaitForSeconds(15f);
        StartCoroutine(HideImage(sprites[1]));
        yield return new WaitForSeconds(5f);
        StartCoroutine(HideImage(sprites[0]));
        yield return new WaitForSeconds(55f);

        SceneManager.LoadScene("Level 1");
    }
    //Exit game
    public void ExitGameClick()
    {
        Application.Quit();
    }
    // Show credist tab
    public void ShowCredits()
    {
        credits.SetActive(true);
        ReturnToPosition(button1, pos1);
        ReturnToPosition(button2, pos2);
        ReturnToPosition(button3, pos3);
        ReturnToPosition(button4, pos4);
        title.SetActive(false);
    }
    // Show menu once again
    public void ReturnToMenu()
    {
        credits.SetActive(false);
        gameInfo.SetActive(false);
        title.SetActive(true);
        StartCoroutine(MoveToPositionX(button1));
        StartCoroutine(MoveToPositionX(button2));
        StartCoroutine(MoveToPositionX(button3));
        StartCoroutine(MoveToPositionX(button4));
    }
    // Show game info tab
    public void showInfo()
    {
        gameInfo.SetActive(true);
        ReturnToPosition(button1, pos1);
        ReturnToPosition(button2, pos2);
        ReturnToPosition(button3, pos3);
        ReturnToPosition(button4, pos4);
        title.SetActive(false);
    }
    // slowly show images 
    private IEnumerator ShowImage(SpriteRenderer image)
    {
        float duration = 1.0f;
        float timer = 0f;

        Color startColor = image.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            image.color = Color.Lerp(startColor, targetColor, timer / duration);
            yield return null;
        }

        image.color = targetColor;
    }
    // slowly hide images
    private IEnumerator HideImage(SpriteRenderer image)
    {
        float duration = 3.0f;
        float timer = 0f;

        Color startColor = image.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        while (timer < duration)
        {
            timer += Time.deltaTime;
            image.color = Color.Lerp(startColor, targetColor, timer / duration);
            yield return null;
        }
        image.color = targetColor;
    }
}
