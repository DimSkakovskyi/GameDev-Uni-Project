using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinOnUi : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    private void OnEnable()
    {
        // Get the number of collected coins from the CoinManager
        int totalCoins = CoinManager.instance.coins;

        // Display the coins on the Game Over screen
        coinText.text = "Coins Collected: " + totalCoins;
    }
}
