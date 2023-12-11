using System;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    public delegate void DisplayTextDelegate(string text, float displayDuration);
    public static event DisplayTextDelegate OnDisplayText;
    public string textToShow = "";
    public float displayDuration = 5.0f;

    private bool done = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !done)
        {
            if (OnDisplayText != null)
            {
                OnDisplayText(textToShow, displayDuration);
            }
            done = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            done = false;
        }
    }
}
