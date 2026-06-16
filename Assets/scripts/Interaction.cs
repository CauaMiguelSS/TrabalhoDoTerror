using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] private float _interactionRange = 3f;
    private Camera _mainCam;
    private IInteractable _target;//Objeto alvo do raycast
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(_mainCam.transform.position, _mainCam.transform.forward, out RaycastHit hit, _interactionRange))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                if (_target == interactable)//Se for o mesmo objeto, n?o fa?a nada
                    return;
                _target?.HideOutline();//Desativa o ultimo objeto, caso exista
                _target = interactable;//Sendo outro objeto, ele se torna o novo alvo
                _target.ShowOutline();
            }
            else
            {//Caso o raycast acerte algo que n?o seja interag?vel, ele desativa o ultimo objeto
                _target?.HideOutline();
                _target = null;
            }
        }
        else
        {//Caso o player n?o esteja encostando em nada, ele desativa do ultimo objeto
            _target?.HideOutline();
            _target = null;
        }
    }
    public void OnInteract(InputValue value)
    {
        if (_target == null)//Nullcheck
            return;

        _target.Interact();
    }
}
