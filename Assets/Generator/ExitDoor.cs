using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Regenerating level...");
            LevelGeneration levelGen = FindObjectOfType<LevelGeneration>();
            if (levelGen != null)
            {
                levelGen.RegenerateLevel();
            }
            else
            {
                Debug.LogWarning("LevelGeneration not found in scene.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
