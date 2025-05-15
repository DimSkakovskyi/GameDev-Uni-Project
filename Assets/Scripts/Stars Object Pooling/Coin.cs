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
            coinManager?.ChangeCoins(value);
            gameObject.SetActive(false); // повертаємо в пул
        }
    }
}