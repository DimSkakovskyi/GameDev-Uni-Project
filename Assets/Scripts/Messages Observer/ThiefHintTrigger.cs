using UnityEngine;

public class ThiefHintTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        if (HintBroadcaster.Instance != null)
        {
            HintBroadcaster.Instance.TriggerHint("GREY THIEF OF COINS!");
        }
    }
}