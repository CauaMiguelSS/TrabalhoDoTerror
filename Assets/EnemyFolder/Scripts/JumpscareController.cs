using UnityEngine;

public class JumpscareController : MonoBehaviour
{
    [Header("Player")]
    public Transform playerCamera;

    public MonoBehaviour[] playerScripts;

    [Header("Enemy")]
    public GameObject enemy;
    public EnemyAI enemyAI;

    [Header("UI")]
    public GameObject deathScreen;

    [Header("Audio")]
    public AudioSource jumpscareSound;

    [Header("Settings")]
    public float distanceFromCamera = 1.5f;

    bool triggered = false;

    void Start()
    {
        if (deathScreen != null)
            deathScreen.SetActive(false);
    }

    public void TriggerJumpscare()
    {
        Debug.Log("JUMPSCARE EXECUTOU");
        if (triggered)
            return;

        triggered = true;

        // trava player
        foreach (MonoBehaviour script in playerScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        if (enemyAI != null)
        {
            enemyAI.FreezeEnemy();
        }

        PositionEnemy();

        if (jumpscareSound != null)
        {
            jumpscareSound.Play();
        }

        Invoke(nameof(ShowDeathScreen), 0.5f);
    }

    void PositionEnemy()
    {
        if (enemy == null || playerCamera == null)
            return;

        Vector3 pos =
            playerCamera.position +
            playerCamera.forward * distanceFromCamera;

        enemy.transform.position = pos;

        enemy.transform.LookAt(playerCamera);
    }

    void ShowDeathScreen()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}