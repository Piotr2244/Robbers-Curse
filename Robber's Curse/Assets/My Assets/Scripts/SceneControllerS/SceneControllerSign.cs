using UnityEngine;
using UnityEngine.SceneManagement;
/* Class that changes the scene */
public class SceneControllerSign : MonoBehaviour
{
    // Variables and references
    public bool unlocked = false;
    public string sceneName = "";
    public HeroStatus heroStatus;
    // Change scene after collision with hero
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && unlocked)
        {
            heroStatus.SaveStatus();
            SceneManager.LoadScene(sceneName);
        }
    }
}
