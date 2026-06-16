using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    public float speed = 2f;
    public float amount = 5f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = startPos +
            new Vector3(0, Mathf.Sin(Time.time * speed) * amount, 0);
    }
}