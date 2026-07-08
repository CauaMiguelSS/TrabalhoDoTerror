using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class ObjectiveSystem : MonoBehaviour
{
    public static ObjectiveSystem Instance;

    [Header("UI")]
    [SerializeField] private GameObject panelObjective;
    [SerializeField] private TMP_Text objectivesText;

    [Header("Input")]
    [SerializeField] private InputActionReference openObjectivesAction;

    private bool doorOpened;
    private int documentsFound;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        panelObjective.SetActive(false);
        UpdateObjectivesUI();
    }

    private void OnEnable()
    {
        if (openObjectivesAction != null)
        {
            openObjectivesAction.action.Enable();
            openObjectivesAction.action.performed += TogglePanel;
        }
    }

    private void OnDisable()
    {
        if (openObjectivesAction != null)
        {
            openObjectivesAction.action.performed -= TogglePanel;
            openObjectivesAction.action.Disable();
        }
    }

    private void TogglePanel(InputAction.CallbackContext ctx)
    {
        bool open = !panelObjective.activeSelf;

        panelObjective.SetActive(open);

        Cursor.visible = open;
        Cursor.lockState = open
            ? CursorLockMode.None
            : CursorLockMode.Locked;

        FirstPersonController player =
            FindFirstObjectByType<FirstPersonController>();

        if (player != null)
            player.enabled = !open;
    }

    public void DoorOpened()
    {
        doorOpened = true;
        UpdateObjectivesUI();
        CheckVictory();
    }

    public void RegisterDocumentFound()
    {
        documentsFound++;
        UpdateObjectivesUI();
        CheckVictory();
    }

    public void UpdateObjectivesUI()
    {
        if (objectivesText == null)
            return;

        int viewers = 0;

        if (LiveSystem.Instance != null)
            viewers = LiveSystem.Instance.CurrentAudience;

        objectivesText.text =
            (doorOpened ? "☑" : "☐") + " Open Security Door\n\n" +
            (documentsFound >= 3 ? "☑" : "☐") + " Find 3 Documents (" + documentsFound + "/3)\n\n" +
            (viewers >= 1500 ? "☑" : "☐") + " Reach 1500 Viewers (" + viewers + "/1500)";
    }

    public void CheckVictory()
    {
        if (LiveSystem.Instance == null)
            return;

        if (
            doorOpened &&
            documentsFound >= 3 &&
            LiveSystem.Instance.CurrentAudience >= 1500
        )
        {
            LiveSystem.Instance.Victory();
        }
    }
}