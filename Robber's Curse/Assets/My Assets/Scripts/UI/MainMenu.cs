using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject button1;
    private GameObject button2;
    private GameObject button3;
    private GameObject button4;
    private GameObject credits;
    private GameObject gameInfo;
    private GameObject title;
    private float pos1, pos2, pos3, pos4;
    private SpriteRenderer blackScreen;
    void Start()
    {
        Debug.Log("0");
        //StopAllCoroutines();
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
        Debug.Log("1");
        blackScreen = transform.Find("BlackScreen").GetComponent<SpriteRenderer>();
        StartCoroutine(MakeMenu());
        Debug.Log("2");

    }
    private IEnumerator MoveToPositionX(GameObject gm)
    {
        Vector3 targetPosition = new Vector3(0, gm.transform.position.y, gm.transform.position.z);
        Debug.Log(gm.transform.position.x);
        while (Vector3.Distance(gm.transform.position, targetPosition) > 0.01f)
        {

            gm.transform.position = Vector3.MoveTowards(gm.transform.position, targetPosition, 15f * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator MakeMenu()
    {
        Debug.Log("4");
        Color newColor = blackScreen.color;
        Debug.Log("5");
        yield return new WaitForSeconds(1f);
        Debug.Log("6");
        float alpha = 1f;
        blackScreen.color = newColor;
        while (alpha > 0f)
        {
            Debug.Log("7");
            alpha -= Time.deltaTime;
            newColor.a = alpha;
            blackScreen.color = newColor;
            yield return new WaitForSeconds(0.02f);
        }
        ReturnToMenu();
    }

    private void ReturnToPosition(GameObject gm, float position)
    {
        gm.transform.position = new Vector3(position, gm.transform.position.y, gm.transform.position.z);
    }
    public void StartGameClick()
    {
        StartCoroutine(StartGame());
    }
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
        SceneManager.LoadScene("Level 1");
    }

    public void ExitGameClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
    }

    public void ShowCredits()
    {
        credits.SetActive(true);
        ReturnToPosition(button1, pos1);
        ReturnToPosition(button2, pos2);
        ReturnToPosition(button3, pos3);
        ReturnToPosition(button4, pos4);
        title.SetActive(false);
    }

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

    public void showInfo()
    {
        gameInfo.SetActive(true);
        ReturnToPosition(button1, pos1);
        ReturnToPosition(button2, pos2);
        ReturnToPosition(button3, pos3);
        ReturnToPosition(button4, pos4);
        title.SetActive(false);
    }
}
