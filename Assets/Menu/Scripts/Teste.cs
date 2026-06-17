using UnityEngine;
using UnityEngine.EventSystems;

public class Teste : MonoBehaviour
{
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        }
    }
}