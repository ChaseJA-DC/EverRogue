using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public int sceneIndexToLoad;
    private Collider doorCollider;

    void Start()
    {
        doorCollider = GetComponent<Collider>();
        doorCollider.enabled = false; // Disable at start
    }

    void Update()
    {
        // Check how many enemies are still in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0 && !doorCollider.enabled)
        {
            Debug.Log("All enemies defeated — door is now active.");
            doorCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (doorCollider.enabled && other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneIndexToLoad);
        }
    }
}
