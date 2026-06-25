using UnityEngine;

public class DocumentFound : MonoBehaviour
{
    [SerializeField] private int objectiveValue = 1;

    [Header("Live Event")]
    [SerializeField]
    private LiveEventType eventType =
        LiveEventType.SECRET_DOCUMENT;

    private bool used;

    private void OnTriggerEnter(Collider other)
    {
        if (used)
            return;

        if (!other.CompareTag("Player"))
            return;

        used = true;

        ObjectiveSystem.Instance.AddProgress(objectiveValue);

        LiveSystem.Instance.TriggerEvent(eventType);

        Destroy(gameObject);
    }
}