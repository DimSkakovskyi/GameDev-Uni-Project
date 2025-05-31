using UnityEngine;

public class CoinHintTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        if (HintBroadcaster.Instance != null)
        {
            HintBroadcaster.Instance.TriggerHint("Pick up coins and get away!");
        }
    }
}
