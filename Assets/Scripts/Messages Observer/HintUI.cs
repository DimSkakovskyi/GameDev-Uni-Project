using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class HintUI : MonoBehaviour
{
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private float showDuration = 2f;

    private Queue<string> hintQueue = new Queue<string>();
    private bool isShowing = false;

    private void Start()
    {
        if (hintText == null)
            Debug.LogError("[HintUI] hintText не під’єднаний в інспекторі!");

        HintBroadcaster.Instance.OnHintTriggered += QueueHint;
        Debug.Log("[HintUI] Підписався на HintBroadcaster");

        // одразу приховаємо
        SetAlpha(0f);
        hintText.gameObject.SetActive(false);
    }

    private void QueueHint(string message)
    {
        Debug.Log("[HintUI] QueueHint отримав: " + message);
        hintQueue.Enqueue(message);
        if (!isShowing)
            StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        isShowing = true;

        while (hintQueue.Count > 0)
        {
            string message = hintQueue.Dequeue();
            hintText.text = message;
            hintText.gameObject.SetActive(true);
            SetAlpha(1f);

            yield return new WaitForSeconds(showDuration);

            // fade-out
            float fadeTime = 1f;
            float t = 0f;
            while (t < fadeTime)
            {
                float a = Mathf.Lerp(1f, 0f, t / fadeTime);
                SetAlpha(a);
                t += Time.deltaTime;
                yield return null;
            }

            SetAlpha(0f);
            hintText.gameObject.SetActive(false);
        }

        isShowing = false;
    }

    private void SetAlpha(float alpha)
    {
        Color c = hintText.color;
        c.a = alpha;
        hintText.color = c;
    }
}