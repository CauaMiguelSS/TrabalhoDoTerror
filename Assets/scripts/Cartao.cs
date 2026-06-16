using UnityEngine;

public class Cartao : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement jogador = other.GetComponent<PlayerMovement>();

        if (jogador != null)
        {
            jogador.temCartao = true;
            Destroy(gameObject);
        }
    }
}