using UnityEngine;
using UnityEngine.InputSystem;

public enum ActiveState
{
    OFF,
    ON
}

public class Flashlight : MonoBehaviour
{
    private ActiveState _activeState = ActiveState.OFF;

    private Light _light;

    [SerializeField] private InputActionReference _flashlightAction;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void Start()
    {
        SetState(ActiveState.OFF);
    }

    private void OnEnable()
    {
        _flashlightAction.action.Enable();
        _flashlightAction.action.performed += OnFlashlightPerformed;
    }

    private void OnDisable()
    {
        _flashlightAction.action.performed -= OnFlashlightPerformed;
        _flashlightAction.action.Disable();
    }

    private void OnFlashlightPerformed(InputAction.CallbackContext context)
    {
        TurnFlashlight();
    }

    public void TurnFlashlight()
    {
        if (_activeState == ActiveState.ON)
        {
            SetState(ActiveState.OFF);
        }
        else
        {
            SetState(ActiveState.ON);
        }
    }

    public void SetState(ActiveState newState)
    {
        _light.enabled = (newState == ActiveState.ON);
        _activeState = newState;
    }
}