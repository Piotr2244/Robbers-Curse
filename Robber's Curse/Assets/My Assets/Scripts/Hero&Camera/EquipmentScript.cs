using UnityEngine;
using UnityEngine.UI;

public class EquipmentScript : MonoBehaviour
{
    public Hero hero;

    private Text goldText;
    private Text strengthText;
    private Text speedText;
    private Text jumpText;

    private void Awake()
    {
        goldText = transform.Find("GoldBar/gold").GetComponent<Text>();
        strengthText = transform.Find("StrengthBar/strength").GetComponent<Text>();
        speedText = transform.Find("SpeedBar/speed").GetComponent<Text>();
        jumpText = transform.Find("JumpBar/jump").GetComponent<Text>();
    }

    private void Update()
    {
        if (hero != null)
        {
            goldText.text = hero.gold.ToString();
            strengthText.text = hero.damage.ToString();
            speedText.text = hero.speed.ToString();
            jumpText.text = hero.jumpForce.ToString();
        }
    }
}
