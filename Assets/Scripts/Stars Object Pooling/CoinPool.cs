using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    public static CoinPool Instance;

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int poolSize = 20;

    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform);
            coin.SetActive(false);
            pool.Add(coin);
        }
    }

    public GameObject GetCoin()
    {
        foreach (var coin in pool)
        {
            if (!coin.activeInHierarchy)
                return coin;
        }

        // Створити нову монету, якщо всі зайняті
        GameObject newCoin = Instantiate(coinPrefab, transform);
        newCoin.SetActive(false);
        pool.Add(newCoin);
        Debug.Log("[Pool] Створено нову монету (розширення пулу)");
        return newCoin;
    }

    public void ReturnCoin(GameObject coin)
    {
        coin.SetActive(false);
    }
}