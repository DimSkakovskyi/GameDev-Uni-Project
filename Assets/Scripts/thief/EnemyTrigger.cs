using System.Collections;
using UnityEngine;

using UnityEngine.Rendering.Universal;

public class EnemyTrigger : MonoBehaviour
{
    [Header("Ворог")]
    public GameObject enemyPrefab;

    [Header("Світло")]
    private GameObject playerLight;     
    public GameObject enemyLight;      

    private bool triggered = false;
    public  float triggerDelay = 2f; 

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (triggered || !other.CompareTag("Player"))
            return;

        triggered = true;

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
        SoundEffectManager.Play("Wraith");
        Debug.Log("Ворог створено на позиції: " + spawnPosition);

        GameObject enemyLightInstance = Instantiate(enemyLight, enemy.transform.position, Quaternion.identity);
        if (enemyLightInstance == null)
    Debug.LogWarning("enemyLight не створено!");


        enemyLightInstance.transform.SetParent(enemy.transform);
        enemyLightInstance.transform.localPosition = Vector3.zero;
        enemyLightInstance.SetActive(true);

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
    DarknessManager.instance?.StartDarkness();

    while (enemy != null && IsEnemyVisible(enemy))
    {
        yield return null;
    }

    Destroy(gameObject);
    Debug.Log("Тригер знищено після зникнення ворога з екрану");

    DarknessManager.instance?.StopDarkness();
}

    private bool IsEnemyVisible(GameObject enemy)
    {
        Vector2 viewPos = Camera.main.WorldToViewportPoint(enemy.transform.position);
        return viewPos.x >= -0.2f && viewPos.x <= 1.2f && viewPos.y >= -0.2f && viewPos.y <= 1.2f;
    }
}

