using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsPress : MonoBehaviour
{
    [SerializeField] private string _nomeDaCena;
    [SerializeField] private GameObject _painelCredits;
    public void Play()
    {
        SceneManager.LoadScene(_nomeDaCena);
    }

    public void OpenCredits()
    {
        _painelCredits.SetActive(true);
    }

    public void CloseCredits()
    {
        _painelCredits.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
