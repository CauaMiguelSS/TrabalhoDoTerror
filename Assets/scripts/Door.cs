using UnityEngine;

public class Door : MonoBehaviour
{
    public bool abrir = false;

    void Update()
    {
        if (abrir)
        {
            transform.position += Vector3.up * 2f * Time.deltaTime;
        }
        else if(!abrir)
        { 
        
        }
    }
}