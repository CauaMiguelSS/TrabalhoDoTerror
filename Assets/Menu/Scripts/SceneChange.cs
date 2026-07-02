using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string _gameSceneName = "SciFi_Warehouse";
    [SerializeField] private GameObject _painelOptions;

    [Header("Transition")]
    [SerializeField] private RectTransform _topBar;
    [SerializeField] private RectTransform _bottomBar;
    [SerializeField] private float _transitionSpeed = 800f;

    private bool _isLoading;

    public void StartGame()
    {
        if (_isLoading)
            return;

        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        _isLoading = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Vector2 topTarget = Vector2.zero;
        Vector2 bottomTarget = Vector2.zero;

        while (
            Vector2.Distance(_topBar.anchoredPosition, topTarget) > 1f ||
            Vector2.Distance(_bottomBar.anchoredPosition, bottomTarget) > 1f
        )
        {
            _topBar.anchoredPosition = Vector2.MoveTowards(
                _topBar.anchoredPosition,
                topTarget,
                _transitionSpeed * Time.deltaTime
            );

            _bottomBar.anchoredPosition = Vector2.MoveTowards(
                _bottomBar.anchoredPosition,
                bottomTarget,
                _transitionSpeed * Time.deltaTime
            );

            yield return null;
        }

        SceneManager.LoadScene(_gameSceneName);
    }

    public void OpenOptions()
    {
        _painelOptions.SetActive(true);
    }

    public void CloseOptions()
    {
        _painelOptions.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}