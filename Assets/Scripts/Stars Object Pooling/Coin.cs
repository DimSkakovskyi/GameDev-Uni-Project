using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    private bool hasTriggered;
    private CoinManager coinManager;

    private void Start()
    {
        coinManager = CoinManager.instance;
    }

    private void OnEnable()
    {
        if (coinManager == null)
        {
            Debug.LogError("coinManager == null — дія не виконана");
        }

        hasTriggered = false;
        Debug.Log($"[Coin] Активовано на позиції {transform.position}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Зіткнення з {collision.gameObject.name}");

        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            Debug.Log($"Зібрано! Додаємо {value} монет");

            // Додаємо монету в систему
            coinManager?.ChangeCoins(value);

            // Повідомляємо PlayerMovement про збирання (досягнення)
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.CollectCoin();
            }

            // Повертаємо об'єкт у пул
            gameObject.SetActive(false);
        }
    }
}