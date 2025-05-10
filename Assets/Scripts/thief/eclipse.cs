using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eclipse : MonoBehaviour
{
    public float eclipseSpeed = 1f;
    private Image image;
    private Color color;
    private Coroutine eclipseCoroutine;

    private void Awake()
    {
        image = GetComponent<Image>();
        color = image.color;
    }

    public void StartEclipse()
    {
        // Завжди зупиняємо попередню корутину перед новою
        if (eclipseCoroutine != null)
            StopCoroutine(eclipseCoroutine);

        // Скидаємо прозорість
        color.a = 0f;
        image.color = color;

        eclipseCoroutine = StartCoroutine(HandleEclipse());
    }

    private IEnumerator HandleEclipse()
    {
        while (color.a < 1f)
        {
            color.a += eclipseSpeed * Time.deltaTime;
            color.a = Mathf.Clamp01(color.a);
            image.color = color;
            yield return null;
        }
    }
}