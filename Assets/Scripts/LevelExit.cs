using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public int sceneToLoadIndex = 2;
 // Set in Inspector
    private bool levelClear = false;

    void Update()
    {
        // Check if all enemies are gone
        if (!levelClear && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            levelClear = true;
            Debug.Log("All enemies defeated! Exit is now active.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (levelClear && other.CompareTag("Player"))
        {
            Debug.Log("Player entered exit. Loading scene: " + sceneToLoadIndex);
            SceneManager.LoadScene(sceneToLoadIndex);
        }
    }
}
