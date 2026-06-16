using UnityEngine;
using UnityEngine.UI;

public class VHSNoise : MonoBehaviour
{
    public RawImage noiseImage;

    void Update()
    {
        noiseImage.uvRect = new Rect(
            Random.value,
            Random.value,
            1,
            1
        );
    }
}