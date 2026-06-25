using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button[] buttons;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private Vector2 cursorOffset = new Vector2(-120f, 0f);

    private int currentIndex;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SelectButton(0);

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;

            EventTrigger trigger =
                buttons[i].gameObject.GetComponent<EventTrigger>();

            if (trigger == null)
            {
                trigger = buttons[i].gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry hoverEntry =
                new EventTrigger.Entry();

            hoverEntry.eventID = EventTriggerType.PointerEnter;
            hoverEntry.callback.AddListener((data) =>
            {
                SelectButton(index);
            });

            trigger.triggers.Add(hoverEntry);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (buttons == null || buttons.Length == 0)
                return;

            buttons[currentIndex].onClick.Invoke();
        }
    }

    private void SelectButton(int index)
    {
        if (buttons == null || buttons.Length == 0)
            return;

        currentIndex = index;

        GameObject selected = buttons[currentIndex].gameObject;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(selected);
        }

        MoveCursorToButton(
            buttons[currentIndex].GetComponent<RectTransform>()
        );
    }

    private void MoveCursorToButton(RectTransform buttonRect)
    {
        if (cursor == null || buttonRect == null)
            return;

        cursor.position =
            buttonRect.position +
            new Vector3(cursorOffset.x, cursorOffset.y, 0f);
    }
}