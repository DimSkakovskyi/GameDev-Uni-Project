using UnityEngine;
using TMPro;
using System.Collections;

public class AchievementTracker : MonoBehaviour, IAchievementObserver
{
    [SerializeField] private TMP_Text achievementText;
    [SerializeField] private float showDuration = 2f;

    private void Awake()
    {
        Debug.Log("[AchievementTracker] Awake викликано.");
    }
    private void Start()
    {
        var player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            Debug.Log("[AchievementTracker] Player знайдено, підписуємось.");
            player.RegisterAchievementObserver(this);
        } else
        {
            Debug.LogWarning("[AchievementTracker] Player НЕ знайдено!");
        }

        if (achievementText != null)
        {
            achievementText.enabled = false;
            Color c = achievementText.color;
            c.a = 0f;
            achievementText.color = c;
        }
    }

    public void OnCoinCollected()
    {
        int balance = CoinManager.instance?.coins ?? 0;
        Debug.Log($"[AchievementTracker] Поточний баланс: {balance}");

        if (balance == 5)
            ShowAchievement("5 coins! Great");
        else if (balance == 10)
            ShowAchievement("10 coins, amazing!");
        else if (balance == 20)
            ShowAchievement("it's time to get away");
    }

    private void ShowAchievement(string message)
    {
        if (achievementText != null)
        {
            StopAllCoroutines();
            StartCoroutine(ShowMessageRoutine(message));
        }
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        achievementText.text = message;
        achievementText.enabled = true;

        // FADE IN
        Color c = achievementText.color;
        float fadeInDuration = 0.3f;
        float t = 0f;
        while (t < fadeInDuration)
        {
            c.a = Mathf.Lerp(0f, 1f, t / fadeInDuration);
            achievementText.color = c;
            t += Time.deltaTime;
            yield return null;
        }
        c.a = 1f;
        achievementText.color = c;

        yield return new WaitForSeconds(showDuration);

        // FADE OUT
        float fadeOutDuration = 0.5f;
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            c.a = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            achievementText.color = c;
            elapsed += Time.deltaTime;
            yield return null;
        }

        achievementText.enabled = false;
    }

}