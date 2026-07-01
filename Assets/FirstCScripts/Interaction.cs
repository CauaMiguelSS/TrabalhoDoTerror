using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] private float _interactionRange = 3f;

    private Camera _mainCam;
    private IInteractable _target;

    void Start()
    {
        _mainCam = Camera.main;
    }

    void Update()
    {
        if (Physics.Raycast(_mainCam.transform.position, _mainCam.transform.forward, out RaycastHit hit, _interactionRange))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                if (_target != interactable)
                {
                    _target?.HideOutline();
                    _target = interactable;
                    _target.ShowOutline();
                }

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    _target.Interact();
                    LiveSystem.Instance.TriggerEvent(LiveEventType.SECRET_DOCUMENT);
                }
            }
            else
            {
                _target?.HideOutline();
                _target = null;
            }
        }
        else
        {
            _target?.HideOutline();
            _target = null;
        }
    }
}