using UnityEngine;

public class LeafInteractable : MonoBehaviour, IInteractable
{
    [Header("UI Canvas Settings")]
    [SerializeField] private GameObject _textPanel;

    private Outline _outline;

    void Awake()
    {
        if (TryGetComponent(out _outline))
        {
            _outline.enabled = false;
        }

        if (_textPanel != null)
        {
            _textPanel.SetActive(false);
        }
    }

    public void ShowOutline()
    {
        if (_outline != null) _outline.enabled = true;
    }

    public void HideOutline()
    {
        if (_outline != null) _outline.enabled = false;
    }

    public void Interact()
    {
        if (_textPanel == null) return;

        // Inverte o estado atual do painel (se ativo desativa, se desativo ativa)
        bool isPanelActive = !_textPanel.activeSelf;
        _textPanel.SetActive(isPanelActive);
    }
}