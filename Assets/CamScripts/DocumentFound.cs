using UnityEngine;
public class DocumentFound : MonoBehaviour, IInteractable
{
    [SerializeField] private int objectiveValue = 1;
    [SerializeField] private LiveEventType eventType = LiveEventType.SECRET_DOCUMENT;

    private bool _used;
    private Outline _outline;
    private void Start()
    {
        _outline = GetComponentInChildren<Outline>();

        if (_outline != null)
            _outline.enabled = false;
    }

    public void Interact()
    {
        Debug.Log("DOCUMENTO PEGO");

        if (_used)
            return;
        _used = true;
        ObjectiveSystem.Instance.RegisterDocumentFound();
        LiveSystem.Instance.TriggerEvent(eventType);
        Destroy(gameObject);
    }

    public void ShowOutline()
    {
        if (_outline != null)
            _outline.enabled = true;
    }

    public void HideOutline()
    {
        if (_outline != null)
            _outline.enabled = false;
    }
}