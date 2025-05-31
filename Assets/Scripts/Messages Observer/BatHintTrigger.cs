using UnityEngine;

public class BatHintTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        if (HintBroadcaster.Instance != null)
        {
            HintBroadcaster.Instance.TriggerHint("There are grey monsters!");
        }
    }
}