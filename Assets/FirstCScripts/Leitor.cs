using UnityEngine;

public class Leitor : MonoBehaviour, IInteractable
{
    public Door porta;
    private IInteractable _target;
    private Outline _out;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement jogador = other.GetComponent<PlayerMovement>();

        if (jogador != null && jogador.temCartao)
        {
            porta.open = true;
        }
    }
    public void Interact()
    {
        PlayerMovement jogador = FindFirstObjectByType<PlayerMovement>();

        if (jogador != null && jogador.temCartao)
        {
            porta.open = true;
        }
        GetComponent<NoiseSource>().MakeNoise();
    }

    private void Start()
    {
        _out = GetComponentInChildren<Outline>();
        if (_out != null)
            _out.enabled = false;
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