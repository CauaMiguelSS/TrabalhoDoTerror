using UnityEngine;

public class Door : MonoBehaviour
{
    public bool open = false;
    public float altura = 3f;
    public float velocity = 2f;

    private Vector3 destination;
    private AudioSource audioSource;
    private bool tocouSom = false;

    void Start()
    {
        destination = transform.position + Vector3.up * altura;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (open)
        {
            if (!tocouSom)
            {
                audioSource.Play();
                tocouSom = true;
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                destination,
                velocity * Time.deltaTime
            );
        }
        else if(!abrir)
        { 
        
        }
    }


}