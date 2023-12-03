using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerSign : MonoBehaviour
{
    public bool unlocked = false;
    public string sceneName = "";

    public HeroStatus heroStatus;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && unlocked)
        {
            heroStatus.SaveStatus();
            SceneManager.LoadScene(sceneName);
        }
    }
}
