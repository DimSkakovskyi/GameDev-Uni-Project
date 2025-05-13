using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyer : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction;
    public int numcoin = 1;

   void Start()
{
    Camera cam = Camera.main;

    Vector2 center = cam.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));

    Vector2 screenMin = cam.ViewportToWorldPoint(new Vector2(0, 0));
    Vector2 screenMax = cam.ViewportToWorldPoint(new Vector2(1, 1));

    Vector2 spawnPos = Vector2.zero;

    float offset = 2f; 

    int edge = Random.Range(0, 4); 

    switch (edge)
    {
        case 0: 
            spawnPos = new Vector2(screenMin.x - offset, Random.Range(screenMin.y, screenMax.y));
            break;
        case 1: 
            spawnPos = new Vector2(screenMax.x + offset, Random.Range(screenMin.y, screenMax.y));
            break;
        case 2:
            spawnPos = new Vector2(Random.Range(screenMin.x, screenMax.x), screenMax.y + offset);
            break;
        case 3: 
            spawnPos = new Vector2(Random.Range(screenMin.x, screenMax.x), screenMin.y - offset);
            break;
    }

    transform.position = spawnPos;

    Vector2 directionToCenter = (center - spawnPos).normalized;
    direction = directionToCenter;
}
void Update()
{
    transform.Translate(direction * speed * Time.deltaTime);

    Vector2 viewPos = Camera.main.WorldToViewportPoint(transform.position);
    if (viewPos.x < -0.2f || viewPos.x > 1.2f || viewPos.y < -0.2f || viewPos.y > 1.2f)
    {
        Destroy(gameObject);
    }
}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
             Debug.Log("[EnemyFlyer] EnemyFlyer зіткнувся з гравцем");
            if (CoinManager.instance != null)
            {
                CoinManager.instance.ChangeCoins(-numcoin);
                Debug.Log("[EnemyFlyer] Вкрадено 5 монету!");
            }
            else{
                CoinManager.instance.ChangeCoins(1);
                Debug.Log("[EnemyFlyer] отримано 1 монету!");
            }

            Destroy(gameObject); 
        }
    }
}


