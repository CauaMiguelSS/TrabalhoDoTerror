using UnityEngine;

public class Cartao : MonoBehaviour, IInteractable
{
    private Outline _out;

    private void Start()
    {
        _out = GetComponentInChildren<Outline>();

        if (_out != null)
            _out.enabled = false;
    }

    public void Interact()
    {
        PlayerMovement jogador = FindFirstObjectByType<PlayerMovement>();

        if (jogador != null)
        {
            jogador.temCartao = true;
            Destroy(gameObject);
        }
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