using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class ObjectiveSystem : MonoBehaviour
{
    public static ObjectiveSystem Instance;

    [Header("UI")]
    [SerializeField] private GameObject _panelObjective;
    [SerializeField] private TMP_Text _objectivesText;

    [Header("Input")]
    [SerializeField] private InputActionReference _openObjectivesAction;

    private bool _doorOpened;
    private int _documentsFound;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _panelObjective.SetActive(false);
        UpdateObjectivesUI();
    }

    private void OnEnable()
    {
        if (_openObjectivesAction != null)
        {
            _openObjectivesAction.action.Enable();
            _openObjectivesAction.action.performed += TogglePanel;
        }
    }

    private void OnDisable()
    {
        if (_openObjectivesAction != null)
        {
            _openObjectivesAction.action.performed -= TogglePanel;
            _openObjectivesAction.action.Disable();
        }
    }

    private void TogglePanel(InputAction.CallbackContext ctx)
    {
        bool open = !_panelObjective.activeSelf;

        _panelObjective.SetActive(open);

        Cursor.visible = open;
        Cursor.lockState = open
            ? CursorLockMode.None
            : CursorLockMode.Locked;

        FirstPersonController player = FindFirstObjectByType<FirstPersonController>();

        if (player != null)
            player.enabled = !open;
    }

    public void DoorOpened()
    {
        _doorOpened = true;
        UpdateObjectivesUI();
        CheckVictory();
    }

    public void RegisterDocumentFound()
    {
        _documentsFound++;

        Debug.Log("Documentos encontrados: " + _documentsFound);

        UpdateObjectivesUI();
        CheckVictory();
    }

    public void UpdateObjectivesUI()
    {
        if (_objectivesText == null)
            return;

        int viewers = 0;

        if (LiveSystem.Instance != null)
            viewers = LiveSystem.Instance.CurrentAudience;

        _objectivesText.text =
            (_doorOpened ? "☑" : "☐") + " Open Security Door\n\n" +
            (_documentsFound >= 3 ? "☑" : "☐") + " Find 3 Documents (" + _documentsFound + "/3)\n\n" +
            (viewers >= 1500 ? "☑" : "☐") + " Reach 1500 Viewers (" + viewers + "/1500)";
    }

    public void CheckVictory()
    {
        if (LiveSystem.Instance == null)
            return;

        if (_documentsFound >= 3 &&
            LiveSystem.Instance.CurrentAudience >= 1500)
        {
            Debug.Log("VITÓRIA!");

            LiveSystem.Instance.Victory();
        }
    }
}