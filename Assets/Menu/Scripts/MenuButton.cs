using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuButton : MonoBehaviour, IMenuAction
{
    [SerializeField] protected Button button;
    [SerializeField] protected float clickScale = 0.9f;
    [SerializeField] protected float clickDuration = 0.15f;

    public void Execute()
    {
        StartCoroutine(ClickRoutine());
    }

    private IEnumerator ClickRoutine()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 pressedScale = originalScale * clickScale;

        transform.localScale = pressedScale;

        yield return new WaitForSeconds(clickDuration);

        transform.localScale = originalScale;

        OnExecute();
    }

    protected abstract void OnExecute();
}