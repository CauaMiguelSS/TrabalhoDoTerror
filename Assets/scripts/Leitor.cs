using UnityEngine;

public class Leitor : MonoBehaviour 
{
    public Door porta;
    private IInteractable _target;

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
        _target.Interact();
    }
}