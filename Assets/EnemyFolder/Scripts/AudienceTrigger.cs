using UnityEngine;

public class AudienceTrigger : MonoBehaviour
{
    [SerializeField] private int viewersReward = 40;
    [SerializeField] private LiveEventType eventType = LiveEventType.NONE;

    private bool _activated;

    private void OnTriggerEnter(Collider other)
    {
        if (_activated)
            return;

        if (!other.CompareTag("Player"))
            return;

        _activated = true;

        Debug.Log("Audience Trigger ativado!");

        if (LiveSystem.Instance != null)
        {
            LiveSystem.Instance.AddAudience(viewersReward);

            if (eventType != LiveEventType.NONE)
                LiveSystem.Instance.TriggerEvent(eventType);
        }
    }
}