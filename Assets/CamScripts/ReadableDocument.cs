using UnityEngine;

public class ReadableDocument : MonoBehaviour, IInteractable
{
    [Header("Documento")]
    public string titulo;

    [TextArea(10, 20)]
    public string texto;
    public int objectiveValue = 1;

    private bool collected;
    private Outline outline;

    private void Start()
    {
        outline = GetComponentInChildren<Outline>();

        if (outline != null)
            outline.enabled = false;
    }

    public void Interact()
    {
        if (collected)
            return;

        collected = true;

//        DocumentUI.Instance.Open(this);
    }

    public void ShowOutline()
    {
        if (outline != null)
            outline.enabled = true;
    }

    public void HideOutline()
    {
        if (outline != null)
            outline.enabled = false;
    }
}