using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    private int coins;
    [SerializeField] private TMP_Text coinsDisplay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Показати початкову кількість монет (наприклад, 0)
        UpdateDisplay();
    }

    public void ChangeCoins(int amount)
    {
        coins += amount;
        Debug.Log("Coins changed: " + coins);
        UpdateDisplay(); // Оновити текст
    }

    private void UpdateDisplay()
    {
        coinsDisplay.text = "Coins: " + coins;
    }
}
