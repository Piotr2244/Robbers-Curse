using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
/* Game over controller */
public class GameOver : MonoBehaviour
{
    // References
    private SpriteRenderer screen;
    private GameObject MenuButton;
    // Start is called before the first frame update
    private void Start()
    {
        screen = transform.Find("Screen/screenFiller").GetComponent<SpriteRenderer>();
        MenuButton = transform.Find("Screen/Main Menu").gameObject;
        MenuButton.SetActive(false);
    }
    // Make screen dark and show button to leave game
    IEnumerator GameIsOver()
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime;
            Color newColor = screen.color;
            newColor.a = alpha;
            screen.color = newColor;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3f);
        MenuButton.SetActive(true);
    }
    // Exit game
    public void ExitGameClick()
    {
        Application.Quit();
    }
}
