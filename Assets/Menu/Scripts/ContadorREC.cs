using UnityEngine;

public class ContadorREC : MonoBehaviour
{
    public GameObject recText;
    public GameObject redDot;

    public float blinkSpeed = 0.5f;

    void Start()
    {
        InvokeRepeating(nameof(Blink), 0f, blinkSpeed);
    }

    void Blink()
    {
        recText.SetActive(!recText.activeSelf);
        redDot.SetActive(!redDot.activeSelf);
    }
}