using UnityEngine;

public class Cartao : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isOn;
    private Outline _out;

    private void Start()
    {
        _out = GetComponentInChildren<Outline>();
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement jogador = other.GetComponent<PlayerMovement>();

        if (jogador != null)
        {
            jogador.temCartao = true;
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        _isOn = !_isOn;
    }
    public void ShowOutline()
    {
        if (_out != null)
            _out.enabled = true;
    }
    public void HideOutline()
    {
        if (_out != null)
            _out.enabled = false;
    }
}