using StarterAssets;
using TMPro;
using UnityEngine;

public class NoteUI : MonoBehaviour
{
    public static NoteUI Instance;

    [Header("UI")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private TMP_Text noteText;
    private GameObject currentNote;
    private PlayerMovement player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        notePanel.SetActive(false);
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>();
    }

    public void ShowNote(string text)
    {
        notePanel.SetActive(true);
        noteText.text = text;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        FirstPersonController player = FindFirstObjectByType<FirstPersonController>();

        if (player != null)
            player.DisableControl();

        Time.timeScale = 0f;
    }
    public void ShowNote(string text, GameObject noteObject)
    {
        currentNote = noteObject;

        notePanel.SetActive(true);
        noteText.text = text;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        FirstPersonController player = FindFirstObjectByType<FirstPersonController>();

        if (player != null)
            player.DisableControl();

        Time.timeScale = 0f;
    }

    public void CloseNote()
    {
        notePanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        FirstPersonController player = FindFirstObjectByType<FirstPersonController>();

        if (player != null)
            player.EnableControl();

        Time.timeScale = 1f;

        if (currentNote != null)
        {
            Destroy(currentNote);
            currentNote = null;
        }
    }

    private void Update()
    {
        if (notePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseNote();
        }
    }
}
