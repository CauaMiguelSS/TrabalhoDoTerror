using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase }

    [Header("References")]
    public Transform player;
    public Transform playerCamera;
    public NavMeshAgent agent;
    public Transform[] patrolPoints;

    public JumpscareController jumpscareManager;

    [Header("Vision")]
    public float viewDistance = 15f;
    public float viewAngle = 120f;
    public LayerMask obstacleMask;

    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 8f;

    [Header("Attack")]
    public float stopDistance = 2f;

    [Header("Patrol")]
    public float waitTime = 2f;

    private State state = State.Patrol;
    private int patrolIndex;
    private float waitTimer;
    private bool jumpscareTriggered;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (patrolPoints.Length > 0)
        {
            patrolIndex = Random.Range(0, patrolPoints.Length);
            agent.SetDestination(patrolPoints[patrolIndex].position);
        }
    }

    void Update()
    {
        if (player == null || playerCamera == null)
            return;

        DetectPlayer();

        switch (state)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase();
                break;
        }
    }

    void DetectPlayer()
    {
        Vector3 eye = transform.position + Vector3.up * 1.6f;
        Vector3 dir = (player.position - eye).normalized;

        float dist = Vector3.Distance(transform.position, player.position);
        float angle = Vector3.Angle(transform.forward, dir);

        if (dist > viewDistance)
            return;

        if (angle > viewAngle * 0.5f)
            return;

        if (!Physics.Raycast(eye, dir, dist, obstacleMask))
        {
            state = State.Chase;
        }
    }

    void Patrol()
    {
        agent.speed = patrolSpeed;

        if (patrolPoints.Length == 0)
            return;

        if (agent.remainingDistance < 0.3f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                waitTimer = 0f;

                patrolIndex = Random.Range(0, patrolPoints.Length);

                agent.SetDestination(
                    patrolPoints[patrolIndex].position
                );
            }
        }
    }

    void Chase()
    {
        if (jumpscareTriggered)
            return;

        float dist = Vector3.Distance(
            transform.position,
            player.position
        );

        if (dist <= stopDistance)
        {
            jumpscareTriggered = true;

            if (jumpscareManager != null)
            {
                jumpscareManager.TriggerJumpscare();
            }

            return;
        }

        if (IsBeingLookedAt())
        {
            agent.isStopped = true;

            Vector3 lookDir =
                player.position - transform.position;

            lookDir.y = 0f;

            if (lookDir != Vector3.zero)
            {
                transform.rotation =
                    Quaternion.LookRotation(lookDir);
            }

            return;
        }

        agent.speed = chaseSpeed;
        agent.isStopped = false;

        agent.SetDestination(player.position);
    }

    bool IsBeingLookedAt()
    {
        Vector3 eyePos =
            playerCamera.position;

        Vector3 dirToEnemy =
            (transform.position - eyePos).normalized;

        float angle =
            Vector3.Angle(
                playerCamera.forward,
                dirToEnemy
            );

        if (angle > 45f)
            return false;

        float distance =
            Vector3.Distance(
                eyePos,
                transform.position
            );

        if (Physics.Raycast(
            eyePos,
            dirToEnemy,
            out RaycastHit hit,
            distance))
        {
            if (hit.transform == transform)
                return true;
        }

        return false;
    }

    public void FreezeEnemy()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        enabled = false;
    }
}