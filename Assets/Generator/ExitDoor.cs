using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && CoinManager.instance.coins >= 20)
        {
            GameManager.Instance.ShowLevelFinished();
            //LevelGeneration levelGen = FindObjectOfType<LevelGeneration>();
            //if (levelGen != null)
            //{
            //    levelGen.RegenerateLevel();
            //}
            //else
            //{
            //    Debug.LogWarning("LevelGeneration not found in scene.");
            //}
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
