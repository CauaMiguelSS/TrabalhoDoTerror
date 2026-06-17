using UnityEngine;

public class ScanlineScroll : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float distance = 10f;

    private RectTransform rectTransform;
    private Vector2 startPos;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * distance;

        rectTransform.anchoredPosition =
            startPos + new Vector2(0, offset);
    }
}