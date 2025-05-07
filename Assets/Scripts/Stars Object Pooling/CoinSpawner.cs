/* using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private int coinCount = 20;
    [SerializeField] private float startX = -10f;
    [SerializeField] private float endX = 40f;
    [SerializeField] private float spawnY = -0.5f;

    private void Start()
    {
        float step = (endX - startX) / (coinCount - 1);

        for (int i = 0; i < coinCount; i++)
        {
            float x = startX + i * step;
            Vector2 pos = new Vector2(x, spawnY + Random.Range(-0.5f, 0.5f));
            CoinPool.instance.GetCoin(pos);
        }
    }
} */