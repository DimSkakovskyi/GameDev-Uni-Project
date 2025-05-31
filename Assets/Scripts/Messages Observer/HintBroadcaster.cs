using System;
using UnityEngine;

public class HintBroadcaster : MonoBehaviour
{
    public static HintBroadcaster Instance { get; private set; }

    public event Action<string> OnHintTriggered;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void TriggerHint(string message)
    {
        OnHintTriggered?.Invoke(message);
    }
}