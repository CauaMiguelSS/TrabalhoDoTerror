using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuActions : MonoBehaviour
{
    [SerializeField] private InputActionReference _submitAction;

    private void OnEnable()
    {
        _submitAction.action.Enable();
        _submitAction.action.performed += OnSubmit;
    }

    private void OnDisable()
    {
        _submitAction.action.performed -= OnSubmit;
        _submitAction.action.Disable();
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;

        if (selectedButton == null)
            return;

        Button button = selectedButton.GetComponent<Button>();

        if (button == null)
            return;

        button.onClick.Invoke();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SciFi_Warehouse");
    }

    public void OpenOptions()
    {
        Debug.Log("Options");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}