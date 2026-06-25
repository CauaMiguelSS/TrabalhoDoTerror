using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public GameObject dialogueText;

    // Marque apenas nos diálogos que dependem do cartão
    public bool needsCard = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();

        // Se esse diálogo depende do cartão e o jogador já pegou o cartão,
        // ele não aparece mais.
        if (needsCard && player.temCartao)
            return;

        dialogueText.SetActive(true);
        StartCoroutine(HideDialogue());
    }

    IEnumerator HideDialogue()
    {
        yield return new WaitForSeconds(3f);
        dialogueText.SetActive(false);
    }
}