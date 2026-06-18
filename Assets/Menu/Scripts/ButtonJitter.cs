using UnityEngine;

public class ButtonJitter : MonoBehaviour
{
    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = startPos +
            new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0
            );
    }
}