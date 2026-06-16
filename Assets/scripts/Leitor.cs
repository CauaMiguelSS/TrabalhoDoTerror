using UnityEngine;

public class Leitor : MonoBehaviour
{
    public Door porta;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement jogador = other.GetComponent<PlayerMovement>();

        if (jogador != null && jogador.temCartao)
        {
            porta.abrir = true;
        }
    }
}