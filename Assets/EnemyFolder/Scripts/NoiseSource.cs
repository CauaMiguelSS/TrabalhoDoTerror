using UnityEngine;

public class NoiseSource : MonoBehaviour
{
    public bool callEnemy = true;

    private void OnEnable()
    {
        if (callEnemy)
            MakeNoise();
    }

    public void MakeNoise()
    {
        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
        foreach (EnemyAI enemy in enemies)
        {
            enemy.HearNoise(transform.position);
        }
    }
}
