/*using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class DocumentUI : MonoBehaviour
{
    public static DocumentUI Instance;

    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _contentText;

    private ReadableDocument _currentDocument;

    private void Awake()
    {
        Instance = this;
        _panel.SetActive(false);
    }

    private void Update()
    {
        if (!_panel.activeSelf)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Close();
        }
    }

    public void Open(ReadableDocument document)
    {
        _currentDocument = document;

        _panel.SetActive(true);

        _titleText.text = document.titulo;
        _contentText.text = document.texto;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void Close()
    {
        panel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        if (currentDocument != null)
        {
            ObjectiveSystem.Instance.AddProgress(currentDocument.objectiveValue);

            LiveSystem.Instance.TriggerEvent(LiveEventType.SECRET_DOCUMENT);

            Destroy(currentDocument.gameObject);

            currentDocument = null;
        }
    }
}*/