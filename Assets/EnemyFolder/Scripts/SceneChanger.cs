using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string enterScene;
    [SerializeField] private string escapeScene;

    void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(enterScene);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(escapeScene);
        }
    }
}