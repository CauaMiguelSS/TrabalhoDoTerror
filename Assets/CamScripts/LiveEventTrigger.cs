using UnityEngine;

public class LiveEventTrigger : MonoBehaviour
{
    [SerializeField] private LiveSystem _liveSystem;
    [SerializeField] private LiveEventType _eventType;

    private bool _triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        _triggered = true;
        _liveSystem.TriggerEvent(_eventType);
    }
}