using UnityEngine;

public class PoisonHintTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        if (HintBroadcaster.Instance != null)
        {
            HintBroadcaster.Instance.TriggerHint("Poison Zone!");
        }
    }
}
