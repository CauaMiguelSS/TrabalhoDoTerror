using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonEffect : MonoBehaviour,
    ISelectHandler,
    IDeselectHandler
{
    private Vector3 targetScale = Vector3.one;

    public void OnSelect(BaseEventData eventData)
    {
        targetScale = Vector3.one * 1.1f;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        targetScale = Vector3.one;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * 10f
        );
    }
}