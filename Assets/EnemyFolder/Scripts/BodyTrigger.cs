using UnityEngine;

public class BodyTrigger : MonoBehaviour
{
    private bool used;

    private void OnTriggerEnter(Collider other)
    {
        if (used)
            return;

        if (!other.CompareTag("Player"))
            return;

        used = true;

        LiveSystem.Instance.TriggerEvent(LiveEventType.STRANGE_MOVEMENT);
    }
}
