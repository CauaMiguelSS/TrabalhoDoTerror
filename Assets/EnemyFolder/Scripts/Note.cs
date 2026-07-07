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

    public void Interact()
    {
        NoteUI.Instance.ShowNote(noteText, gameObject);
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
