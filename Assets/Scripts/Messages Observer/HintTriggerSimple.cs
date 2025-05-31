using UnityEngine;

public class HintTriggerSimple : MonoBehaviour
{
    [TextArea] public string hintMessage = "Підказка!";
    public float radius = 3f;
    public bool onlyOnce = true;

    private bool hasShown = false;
    private Transform player;

    private void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
            player = playerGO.transform;
        else
            Debug.LogWarning("[HintTriggerSimple] Не знайдено гравця з тегом Player");
    }

    private void Update()
    {
        if (hasShown && onlyOnce) return;
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= radius)
        {
            Debug.Log($"[HintTriggerSimple] Показуємо: {hintMessage}");
            HintBroadcaster.Instance?.TriggerHint(hintMessage);
            hasShown = true;
        }
    }
}