using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

using UnityEngine.Rendering.Universal;

public class EnemyTrigger : MonoBehaviour
{
    [Header("Ворог")]
    public GameObject enemyPrefab;

    [Header("Темрява та світло")]
    public GameObject darknessOverlay; // Image з CanvasGroup (Alpha 0-1)
    private GameObject playerLight;     // Light2D на гравці
    public GameObject enemyLight;      // Prefab або об'єкт Light2D

    private bool triggered = false;

    private Eclipse eclipseEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player"))
            return;

        triggered = true;

        // ✴️ Знаходимо Eclipse, якщо його ще не присвоїли
        if (eclipseEffect == null)
        {
            eclipseEffect = FindObjectOfType<Eclipse>();
            if (eclipseEffect == null)
            {
                Debug.LogWarning("Eclipse не знайдено в сцені!");
                return;
            }
        }

        eclipseEffect.StartEclipse();

        // Спавн ворога
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

        // Прив’язка світла до ворога
        GameObject enemyLightInstance = Instantiate(enemyLight, enemy.transform.position, Quaternion.identity);
        enemyLightInstance.transform.SetParent(enemy.transform);
        enemyLightInstance.transform.localPosition = Vector3.zero;
        enemyLightInstance.SetActive(true);

        // Пошук світла гравця
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

        // Запуск корутини для затемнення і зникнення ворога
        StartCoroutine(HandleDarknessDuringEnemy(enemy));
    }

    private IEnumerator HandleDarknessDuringEnemy(GameObject enemy)
    {
        yield return StartCoroutine(FadeInDarkness());

        while (enemy != null && IsEnemyVisible(enemy))
        {
            yield return null;
        }

        yield return StartCoroutine(FadeOutDarkness());
    }

    private bool IsEnemyVisible(GameObject enemy)
    {
        Vector2 viewPos = Camera.main.WorldToViewportPoint(enemy.transform.position);
        return viewPos.x >= -0.2f && viewPos.x <= 1.2f && viewPos.y >= -0.2f && viewPos.y <= 1.2f;
    }

    private IEnumerator FadeInDarkness()
    {
        if (darknessOverlay == null)
        {
            Debug.LogError("darknessOverlay не задано!");
            yield break;
        }

        darknessOverlay.SetActive(true);
        CanvasGroup canvasGroup = darknessOverlay.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup відсутній на darknessOverlay");
            yield break;
        }

        float duration = 1f;
        float elapsed = 0f;

        canvasGroup.alpha = 0f;
        Debug.Log("Запуск затемнення...");
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOutDarkness()
    {
        if (darknessOverlay == null) yield break;

        CanvasGroup canvasGroup = darknessOverlay.GetComponent<CanvasGroup>();
        if (canvasGroup == null) yield break;

        float duration = 1f;
        float elapsed = 0f;

        canvasGroup.alpha = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        darknessOverlay.SetActive(false);
    }
}

