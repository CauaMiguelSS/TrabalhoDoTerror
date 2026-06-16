using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuCursor : MonoBehaviour
{
    [SerializeField] private RectTransform cursor;
    [SerializeField] private float offsetX = -120f;

    private void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null)
            return;

        cursor.position =
            selected.transform.position +
            new Vector3(offsetX, 0f, 0f);
    }
}