using UnityEngine;

public class Note : MonoBehaviour, IInteractable
{
    [Header("Conteúdo da Nota")]
    [TextArea(5, 15)]
    public string noteText;

    private Outline _out;

    private void Start()
    {
        _out = GetComponentInChildren<Outline>();

        if (_out != null)
            _out.enabled = false;
    }

    private bool _used;

    public void Interact()
    {
        if (_used)
            return;
        _used = true;
        ObjectiveSystem.Instance.RegisterDocumentFound();
        LiveSystem.Instance.TriggerEvent(LiveEventType.SECRET_DOCUMENT);
        NoteUI.Instance.ShowNote(noteText, gameObject);
        Destroy(gameObject);
    }

    public void ShowOutline()
    {
        if (_out != null)
            _out.enabled = true;
    }

    public void HideOutline()
    {
        if (_out != null)
            _out.enabled = false;
    }

}
