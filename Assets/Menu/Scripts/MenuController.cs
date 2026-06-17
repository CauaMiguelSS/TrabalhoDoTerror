using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference navigateAction;
    [SerializeField] private InputActionReference submitAction;

    [Header("UI")]
    [SerializeField] private Button[] buttons;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private Vector2 cursorOffset = new Vector2(-120f, 0f);

    [Header("Settings")]
    [SerializeField] private float moveCooldown = 0.15f;

    private int currentIndex;
    private bool canMove = true;

    private void OnEnable()
    {
        if (navigateAction != null)
        {
            navigateAction.action.Enable();
            navigateAction.action.performed += OnNavigate;
        }

        if (submitAction != null)
        {
            submitAction.action.Enable();
            submitAction.action.performed += OnSubmit;
        }
    }

    private void OnDisable()
    {
        if (navigateAction != null)
        {
            navigateAction.action.performed -= OnNavigate;
            navigateAction.action.Disable();
        }

        if (submitAction != null)
        {
            submitAction.action.performed -= OnSubmit;
            submitAction.action.Disable();
        }
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SelectButton(0);
    }

    private void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (!canMove || buttons == null || buttons.Length == 0)
            return;

        Vector2 value = ctx.ReadValue<Vector2>();

        if (value.y > 0.5f)
        {
            SelectButton(currentIndex - 1);
            StartCoroutine(Cooldown());
        }
        else if (value.y < -0.5f)
        {
            SelectButton(currentIndex + 1);
            StartCoroutine(Cooldown());
        }
    }

    private void OnSubmit(InputAction.CallbackContext ctx)
    {
        if (buttons == null || buttons.Length == 0)
            return;

        buttons[currentIndex].onClick.Invoke();
    }

    private IEnumerator Cooldown()
    {
        canMove = false;
        yield return new WaitForSeconds(moveCooldown);
        canMove = true;
    }

    private void SelectButton(int index)
    {
        if (buttons == null || buttons.Length == 0)
            return;

        if (index < 0)
            index = buttons.Length - 1;

        if (index >= buttons.Length)
            index = 0;

        currentIndex = index;

        GameObject selected = buttons[currentIndex].gameObject;
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(selected);

        MoveCursorToButton(buttons[currentIndex].GetComponent<RectTransform>());
    }

    private void MoveCursorToButton(RectTransform buttonRect)
    {
        if (cursor == null || buttonRect == null)
            return;

        cursor.position = buttonRect.position + new Vector3(cursorOffset.x, cursorOffset.y, 0f);
    }
}