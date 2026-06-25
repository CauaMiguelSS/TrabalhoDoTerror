using UnityEngine;

public class DocumentTrigger : MonoBehaviour
{
    [SerializeField] private int _objectiveValue = 1;

    private bool _used;

    private void OnTriggerEnter(Collider other)
    {
        if (_used)
            return;

        if (!other.CompareTag("Player"))
            return;

        _used = true;

        ObjectiveSystem.Instance.AddProgress(_objectiveValue);
        LiveSystem.Instance.TriggerEvent(LiveEventType.SECRET_DOCUMENT);

        Destroy(gameObject);
    }
}