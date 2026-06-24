using UnityEngine;

public class JumpscareController : MonoBehaviour
{
    [Header("Player")]
    public Transform playerCamera;

    public MonoBehaviour[] playerScripts;

    [Header("Enemy")]
    public GameObject enemy;
    public EnemyAI enemyAI;
    public Transform lookTarget;

    [Header("UI")]
    public GameObject deathScreen;

    [Header("Audio")]
    public AudioSource jumpscareSound;

    bool triggered = false;
    bool lockCamera;

    void Start()
    {
        if (deathScreen != null)
            deathScreen.SetActive(false);
    }
    void LateUpdate()
    {
        if (!lockCamera)
            return;

        if (playerCamera == null || lookTarget == null)
            return;

        Vector3 dir =
            (lookTarget.position - playerCamera.position).normalized;

        Quaternion targetRot =
            Quaternion.LookRotation(dir);

        playerCamera.rotation =
            Quaternion.Slerp(playerCamera.rotation, targetRot, Time.deltaTime * 12f);
    }
    public void TriggerJumpscare()
    {
        Debug.Log("JUMPSCARE EXECUTOU");
        if (triggered)
            return;

        triggered = true;

        foreach (MonoBehaviour script in playerScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        if (enemyAI != null)
        {
            enemyAI.FreezeEnemy();
        }

        lockCamera = true;

        if (jumpscareSound != null)
        {
            jumpscareSound.Play();
        }

        Invoke(nameof(ShowDeathScreen), 0.5f);
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