using System.Collections.Generic;
using UnityEngine;

public class SmartCoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public int initialCoins = 30;

    public float minX = 0f;
    public float maxX = 80f;
    public float minY = -10f;
    public float maxY = 73f;

    public float stepX = 3f;
    public float stepY = 3f;
    public float raycastLength = 3f;

    public LayerMask groundLayer;

    private void Start()
    {
        SpawnCoinsAcrossMap();
    }

    public void SpawnCoin(Vector3 position)
    {
        // Зсунь монетку трохи вгору над землею
        Vector3 adjustedPos = position + Vector3.up * 0.5f;
        Instantiate(coinPrefab, adjustedPos, Quaternion.identity);
    }

    public float spawnChance = 0.3f;
    public float minCoinDistance = 2f; // Мінімальна відстань між монетами

    private List<Vector2> spawnedPositions = new List<Vector2>();

    public void SpawnCoinsAcrossMap()
    {
        spawnedPositions.Clear();
        int spawned = 0;

        for (float x = minX; x <= maxX; x += stepX)
        {
            for (float y = minY; y <= maxY; y += stepY)
            {
                if (Random.value > spawnChance)
                    continue;

                Vector2 origin = new Vector2(x, y);
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, raycastLength, groundLayer);

                if (hit.collider != null)
                {
                    Vector2 spawnPoint = hit.point + Vector2.up * 0.5f;

                    // 1. Перевірка: чи є вже монетка надто близько
                    bool tooClose = false;
                    foreach (Vector2 pos in spawnedPositions)
                    {
                        if (Vector2.Distance(pos, spawnPoint) < minCoinDistance)
                        {
                            tooClose = true;
                            break;
                        }
                    }

                    // 2. Перевірка: чи немає колайдерів у місці спавну (наприклад, стіна)
                    Collider2D overlap = Physics2D.OverlapCircle(spawnPoint, 0.3f, groundLayer);

                    if (!tooClose && overlap == null)
                    {
                        SpawnCoin(spawnPoint);
                        spawnedPositions.Add(spawnPoint);
                        spawned++;
                        if (spawned >= initialCoins) return;
                    }
                }
            }
        }

        Debug.Log("Spawned coins: " + spawned);
    }

#if UNITY_EDITOR
    // Gizmos — для відображення ліній у редакторі
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        for (float x = minX; x <= maxX; x += stepX)
        {
            for (float y = minY; y <= maxY; y += stepY)
            {
                Vector2 origin = new Vector2(x, y);
                Gizmos.DrawLine(origin, origin + Vector2.down * raycastLength);
            }
        }
    }
#endif
}