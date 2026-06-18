using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneTransition : MonoBehaviour
{
    [SerializeField] private RectTransform transitionPanel;
    [SerializeField] private float transitionSpeed = 4f;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(string sceneName)
    {
        while (transitionPanel.localScale.x < 1f)
        {
            transitionPanel.localScale = Vector3.Lerp(
                transitionPanel.localScale,
                Vector3.one,
                Time.deltaTime * transitionSpeed
            );

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}