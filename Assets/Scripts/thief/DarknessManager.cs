using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarknessManager : MonoBehaviour
{
    public static DarknessManager instance;

    public float fadeSpeed = 1f;

    public CanvasGroup darknessCanvas; // посилання на CanvasGroup з Image

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

   public void StartDarkness()
{
    StopAllCoroutines();
    StartCoroutine(FadeTo(1f)); // ← тільки один аргумент
}

public void StopDarkness()
{
    StopAllCoroutines();
    StartCoroutine(FadeTo(0f)); // ← тільки один аргумент
}

    private IEnumerator FadeTo(float targetAlpha)
{
    if (darknessCanvas == null)
    {
        Debug.LogError("[DarknessManager] CanvasGroup не призначено!");
        yield break;
    }

    darknessCanvas.gameObject.SetActive(true);

    float startAlpha = darknessCanvas.alpha;
    float direction = Mathf.Sign(targetAlpha - startAlpha); // +1 або -1

    while (!Mathf.Approximately(darknessCanvas.alpha, targetAlpha))
    {
        darknessCanvas.alpha += direction * fadeSpeed * Time.deltaTime;
        darknessCanvas.alpha = Mathf.Clamp01(darknessCanvas.alpha);
        yield return null;
    }

    darknessCanvas.alpha = targetAlpha;

    if (Mathf.Approximately(targetAlpha, 0f))
        darknessCanvas.gameObject.SetActive(false);
}
}

