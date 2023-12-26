using System;
using UnityEngine;
/* Dialog trigger, sends info to dialog script
 * To display a proper message */
public class TextTrigger : MonoBehaviour
{
    // Variables
    public string textToShow = "";
    public float displayDuration = 5.0f;
    private bool done = false;
    // Events and references
    public delegate void DisplayTextDelegate(string text, float displayDuration);
    public static event DisplayTextDelegate OnDisplayText;
    // Request to show message after coliding with player
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
}
