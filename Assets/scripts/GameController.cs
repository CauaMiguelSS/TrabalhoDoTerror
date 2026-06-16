using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }//Singleton
    public UnityEvent OnUseBattery;
    public UnityEvent OnUseFlashlight;
    //public GameObject Player { get => player;}

    //[SerializeField] private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }
}
