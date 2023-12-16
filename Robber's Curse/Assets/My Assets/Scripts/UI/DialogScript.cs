using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogScript : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    void Start() { }

    private void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        TextTrigger.OnDisplayText += DisplayText;
        MainMenu.OnDisplayText += DisplayDialog;
        EvilHero.OnDisplayText += DisplayDialog;
        WizardBoss.OnDisplayText += DisplayDialog;
    }

    private void OnDisable()
    {
        TextTrigger.OnDisplayText -= DisplayText;
        MainMenu.OnDisplayText -= DisplayDialog;
        EvilHero.OnDisplayText -= DisplayDialog;
        WizardBoss.OnDisplayText -= DisplayDialog;
    }

    public void DisplayText(string text, float displayTime = 10.0f)
    {
        StartCoroutine(DisplayTextCoroutine(text, displayTime));
    }

    private IEnumerator DisplayTextCoroutine(string text, float displayTime)
    {
        Debug.Log(text);
        textMeshPro.text = text;
        yield return new WaitForSeconds(displayTime);
        textMeshPro.text = "";
    }

    public void DisplayDialog(string[] dialog, float displayTime, float afterPause = 0)
    {
        StartCoroutine(DisplayDialogCoroutine(dialog, displayTime, afterPause));
    }

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
