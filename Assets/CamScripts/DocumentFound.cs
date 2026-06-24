using UnityEngine;

public class DocumentTrigger : MonoBehaviour
{
    [SerializeField] private LiveSystem _liveSystem;
    private bool _used;

    private void OnTriggerEnter(Collider other)
    {
        if (_used)
            return;

        if (!other.CompareTag("Player"))
            return;

        _used = true;

        _liveSystem.TriggerEvent(LiveEventType.SECRET_DOCUMENT);
    }
}
