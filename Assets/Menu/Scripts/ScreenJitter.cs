using UnityEngine;

public class ScreenJitter : MonoBehaviour
{
    Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition =
            originalPos + Random.insideUnitSphere * 2f;
    }
}