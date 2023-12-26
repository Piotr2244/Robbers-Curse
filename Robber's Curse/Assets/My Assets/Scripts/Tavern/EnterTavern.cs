using UnityEngine;
/* On scene tavern object, alows hero
 * to enter the tavern */
public class EnterTavern : MonoBehaviour
{
    // Variables and references
    public Transform tavernZone;
    public Canvas tavernText;
    public Canvas tavernCanvas;
    private bool isInside = false;
    public LayerMask heroLayer;
    // Update is called once per frame
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
    // Enter tavern and make enter sign visible
    private void EnterTheTavern()
    {
        if (tavernCanvas != null)
        {
            isInside = true;
            tavernCanvas.gameObject.SetActive(true);
        }
    }
    // Leave tavern
    private void QuitTavern()
    {
        if (tavernCanvas != null)
        {
            isInside = false;
            tavernCanvas.gameObject.SetActive(false);
        }
    }
    // draw enter tavern zone in etitor
    private void OnDrawGizmosSelected()
    {
        if (tavernZone == null)
            return;
        Gizmos.DrawWireSphere(tavernZone.position, 1);
    }
}
