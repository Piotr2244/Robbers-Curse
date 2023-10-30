using UnityEngine;

public class EnterTavern : MonoBehaviour
{
    public Transform tavernZone;
    public Canvas tavernText;
    public Canvas tavernCanvas;
    public Canvas tavernCanvasText;
    private bool isInside = false;
    public LayerMask heroLayer;

    void Update()
    {

        if (isInside)
        {
            if (Input.GetKeyDown(KeyCode.E))
                QuitTavern();
        }
        else
        {
            Collider2D[] HeroNear = Physics2D.OverlapCircleAll(tavernZone.position, 1, heroLayer);

            foreach (Collider2D hero in HeroNear)
            {
                tavernText.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    EnterTheTavern();
                }
            }
        }
    }

    private void EnterTheTavern()
    {
        if (tavernCanvas != null)
        {
            isInside = true;
            tavernCanvas.gameObject.SetActive(true);
            tavernCanvasText.gameObject.SetActive(true);
        }
    }
    private void QuitTavern()
    {
        if (tavernCanvas != null)
        {
            isInside = false;
            tavernCanvas.gameObject.SetActive(false);
            tavernCanvasText.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (tavernZone == null)
            return;
        Gizmos.DrawWireSphere(tavernZone.position, 1);
    }
}
