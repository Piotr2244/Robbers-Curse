using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private SpriteRenderer screen;
    private GameObject MenuButton;

    private void Start()
    {
        screen = transform.Find("Screen/screenFiller").GetComponent<SpriteRenderer>();
        MenuButton = transform.Find("Screen/Main Menu").gameObject;
        MenuButton.SetActive(false);
    }
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
}
