using UnityEngine;

public class Door : MonoBehaviour
{
    public bool abrir = false;
    public float altura = 3f;
    public float velocidade = 2f;

    private Vector3 destino;

    void Start()
    {
        destino = transform.position + Vector3.up * altura;
    }

    void Update()
    {
        if (abrir)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destino,
                velocidade * Time.deltaTime
            );
        }
    }

}