using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

using UnityEngine.Rendering.Universal;

public class EnemyTrigger : MonoBehaviour
{
    [Header("Ворог")]
    public GameObject enemyPrefab;

    [Header("Світло")]
    private GameObject playerLight;     // Light2D на гравці
    public GameObject enemyLight;       // Prefab або об'єкт Light2D

    private bool triggered = false;

    private Eclipse eclipseEffect;

    public  float triggerDelay = 2f; 

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (triggered || !other.CompareTag("Player"))
            return;

        triggered = true;

        // Спавн ворога з випадкової сторони екрану
        Camera cam = Camera.main;
        Vector2 screenMin = cam.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 screenMax = cam.ViewportToWorldPoint(new Vector2(1, 1));

        int side = Random.Range(0, 4);
        Vector3 spawnPosition = Vector3.zero;

        switch (side)
        {
            case 0: spawnPosition = new Vector3(Random.Range(screenMin.x, screenMax.x), screenMax.y + 1f, 0); break;
            case 1: spawnPosition = new Vector3(Random.Range(screenMin.x, screenMax.x), screenMin.y - 1f, 0); break;
            case 2: spawnPosition = new Vector3(screenMin.x - 1f, Random.Range(screenMin.y, screenMax.y), 0); break;
            case 3: spawnPosition = new Vector3(screenMax.x + 1f, Random.Range(screenMin.y, screenMax.y), 0); break;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Ворог створено на позиції: " + spawnPosition);



        // Світло для ворога
        GameObject enemyLightInstance = Instantiate(enemyLight, enemy.transform.position, Quaternion.identity);
        if (enemyLightInstance == null)
    Debug.LogWarning("enemyLight не створено!");


        enemyLightInstance.transform.SetParent(enemy.transform);
        enemyLightInstance.transform.localPosition = Vector3.zero;
        enemyLightInstance.SetActive(true);

        // Світло гравця
        if (playerLight == null)
        {
            Light2D lightComponent = other.GetComponentInChildren<Light2D>(true);
            if (lightComponent != null)
                playerLight = lightComponent.gameObject;
        }

        if (playerLight != null)
            playerLight.SetActive(true);
        else
            Debug.LogWarning("Світло гравця не знайдено!");

        
        StartCoroutine(HandleDarknessWithManager(enemy));
    }

   private IEnumerator HandleDarknessWithManager(GameObject enemy)
{
    // Починаємо затемнення
    DarknessManager.instance?.StartDarkness();

    // Чекаємо, поки ворог буде на екрані
    while (enemy != null && IsEnemyVisible(enemy))
    {
        yield return null;
    }

    // Видаляємо сам тригер, коли ворог вийшов за межі
    Destroy(gameObject);
    Debug.Log("Тригер знищено після зникнення ворога з екрану");

    // Потім запускаємо спад затемнення
    DarknessManager.instance?.StopDarkness();
}

    private bool IsEnemyVisible(GameObject enemy)
    {
        Vector2 viewPos = Camera.main.WorldToViewportPoint(enemy.transform.position);
        return viewPos.x >= -0.2f && viewPos.x <= 1.2f && viewPos.y >= -0.2f && viewPos.y <= 1.2f;
    }
}

