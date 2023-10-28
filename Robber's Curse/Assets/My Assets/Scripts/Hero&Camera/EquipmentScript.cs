using UnityEngine;
using UnityEngine.UI;

public class EquipmentScript : MonoBehaviour
{
    public Hero hero;

    private Text goldText;
    private Text strengthText;
    private Text speedText;
    private Text jumpText;
    private SpriteRenderer spell1;
    private SpriteRenderer spell2;
    private SpriteRenderer spell3;
    private SpriteRenderer spell4;
    private SpriteRenderer spell5;

    private void Awake()
    {
        goldText = transform.Find("GoldBar/gold").GetComponent<Text>();
        strengthText = transform.Find("StrengthBar/strength").GetComponent<Text>();
        speedText = transform.Find("SpeedBar/speed").GetComponent<Text>();
        jumpText = transform.Find("JumpBar/jump").GetComponent<Text>();
        spell1 = transform.Find("SpellBar/1").GetComponent<SpriteRenderer>();
        spell2 = transform.Find("SpellBar/2").GetComponent<SpriteRenderer>();
        spell3 = transform.Find("SpellBar/3").GetComponent<SpriteRenderer>();
        spell4 = transform.Find("SpellBar/4").GetComponent<SpriteRenderer>();
        spell5 = transform.Find("SpellBar/5").GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        if (hero != null)
        {
            goldText.text = hero.gold.ToString();
            strengthText.text = hero.damage.ToString();
            speedText.text = hero.speed.ToString();
            jumpText.text = hero.jumpForce.ToString();
            changeSpell();
        }
    }

    private void changeSpell()
    {
        spell1.enabled = false;
        spell2.enabled = false;
        spell3.enabled = false;
        spell4.enabled = false;
        spell5.enabled = false;
        switch (hero.spellIndex)
        {
            case 1:
                spell1.enabled = true;
                break;
            case 2:
                spell2.enabled = true;
                break;
            case 3:
                spell3.enabled = true;
                break;
            case 4:
                spell4.enabled = true;
                break;
            case 5:
                spell5.enabled = true;
                break;
            default:
                break;
        }
    }
}
