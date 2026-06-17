using UnityEngine;
using TMPro;

public class CameraTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        int hours = Mathf.FloorToInt(timer / 3600);
        int minutes = Mathf.FloorToInt((timer % 3600) / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        timerText.text = $"{hours:00}:{minutes:00}:{seconds:00}";
    }
}