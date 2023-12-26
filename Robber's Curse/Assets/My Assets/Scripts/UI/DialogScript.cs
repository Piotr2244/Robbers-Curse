using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/* Class connected with canvas, displays
 * messages and dialogs on screen */
public class DialogScript : MonoBehaviour
{
    // Text on scene reference
    public TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        TextTrigger.OnDisplayText += DisplayText;
        MainMenu.OnDisplayText += DisplayDialog;
        EvilHero.OnDisplayText += DisplayDialog;
        WizardBoss.OnDisplayText += DisplayDialog;
        WizardCopy.OnDisplayText += DisplayDialog;
        GoodEnding.OnDisplayText += DisplayDialog;
    }

    private void OnDisable()
    {
        TextTrigger.OnDisplayText -= DisplayText;
        MainMenu.OnDisplayText -= DisplayDialog;
        EvilHero.OnDisplayText -= DisplayDialog;
        WizardBoss.OnDisplayText -= DisplayDialog;
        WizardCopy.OnDisplayText -= DisplayDialog;
        GoodEnding.OnDisplayText -= DisplayDialog;
    }
    // Display a single text
    public void DisplayText(string text, float displayTime = 10.0f)
    {
        StartCoroutine(DisplayTextCoroutine(text, displayTime));
    }
    // Show text for some time
    private IEnumerator DisplayTextCoroutine(string text, float displayTime)
    {
        Debug.Log(text);
        textMeshPro.text = text;
        yield return new WaitForSeconds(displayTime);
        textMeshPro.text = "";
    }
    // Display a longer dialog
    public void DisplayDialog(string[] dialog, float displayTime, float afterPause = 0)
    {
        StartCoroutine(DisplayDialogCoroutine(dialog, displayTime, afterPause));
    }
    // Display dialog fragments part after part
    private IEnumerator DisplayDialogCoroutine(string[] dialog, float displayTime, float afterPause = 0)
    {
        foreach (string line in dialog)
        {
            textMeshPro.text += line + "\n";
            yield return new WaitForSeconds(displayTime);
        }
        yield return new WaitForSeconds(afterPause);
        textMeshPro.text = "";

    }
}
