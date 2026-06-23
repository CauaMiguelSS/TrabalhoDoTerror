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
    private float _originalIntensity;

    [SerializeField] private InputActionReference _flashlightAction;
    [SerializeField] private float _intensityDecreaseRate = 0.5f;
    [SerializeField] private float _batteryDuration = 10f;

    private bool _losingPower;
    private float _batteryTimer;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void Start()
    {
        _originalIntensity = _light.intensity;
        _batteryTimer = _batteryDuration;

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

    private void Update()
    {
        switch (_activeState)
        {
            case ActiveState.OFF:
                break;

            case ActiveState.ON:

                if (_losingPower)
                {
                    if (_light.intensity <= 0f)
                        return;

                    _light.intensity -= Time.deltaTime * _intensityDecreaseRate;
                }
                else
                {
                    _batteryTimer -= Time.deltaTime;

                    if (_batteryTimer <= 0f)
                    {
                        _losingPower = true;
                    }
                }

                break;

            default:
                break;
        }
    }

    private void OnFlashlightPerformed(InputAction.CallbackContext context)
    {
        TurnFlashlight();
    }

    public void Recharge()
    {
        _light.intensity = _originalIntensity;
        _batteryTimer = _batteryDuration;
        _losingPower = false;
    }

    public void TurnFlashlight()
    {
        if (_activeState.Equals(ActiveState.ON))
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
        switch (newState)
        {
            case ActiveState.OFF:
                _light.enabled = false;
                break;

            case ActiveState.ON:
                _light.enabled = true;
                break;

            default:
                break;
        }

        _activeState = newState;
    }
}