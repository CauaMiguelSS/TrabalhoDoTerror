using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class EnemyAI : MonoBehaviour
{
    public enum State {Patrol, Chase, Investigate}

    [Header("Audio")]

    public AudioSource audioSource;

    public AudioClip patrolSound;
    public AudioClip investigateSound;
    public AudioClip chaseSound;
    public AudioClip frozenSound;

    [Header("Sound Distance")]

    public float maxSoundDistance = 20f;
    public float minVolume = 0.05f;
    public float maxVolume = 1f;

    private AudioClip currentClip;

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

    private Vector3 lastSeenPosition;
    private float investigateTimer;

    [Header("Investigation")]
    public float investigateTime = 5f;

    [Header("Proximity")]
    public float proximityRange = 4f;

    [Header("Hearing")]
    public float hearingRange = 8f;

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
        FirstPersonController controller = player.GetComponent<FirstPersonController>();

        if (controller != null)
        {
            float dist =
                Vector3.Distance(transform.position, player.position);

            if (controller.GetComponent<CharacterController>().velocity.magnitude > 5.5f && dist <= hearingRange)
            {
                lastSeenPosition = player.position;
                state = State.Chase;
            }
        }

        DetectPlayer();

        switch (state)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase();
                break;

            case State.Investigate:
                Investigate();
                break;
        }
    }

    void DetectPlayer()
    {
        UpdateAudio();

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
            lastSeenPosition = player.position;
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
                agent.SetDestination(patrolPoints[patrolIndex].position);
            }
        }
    }

    void Chase()
    {
        if (jumpscareTriggered)
            return;

        float dist = Vector3.Distance(transform.position, player.position);

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

            Vector3 lookDir = player.position - transform.position;

            lookDir.y = 0f;

            if (lookDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDir);
            }

            return;
        }

        agent.speed = chaseSpeed;
        agent.isStopped = false;

        agent.SetDestination(player.position);

        lastSeenPosition = player.position;

        Vector3 eye = transform.position + Vector3.up * 1.6f;
        Vector3 dir = (player.position - eye).normalized;

        float distanceToPlayer =
            Vector3.Distance(transform.position, player.position);

        float angle =
            Vector3.Angle(transform.forward, dir);

        bool canSeePlayer = false;

        if (distanceToPlayer <= viewDistance &&
            angle <= viewAngle * 0.5f)
        {
            if (!Physics.Raycast(
                eye,
                dir,
                distanceToPlayer,
                obstacleMask))
            {
                canSeePlayer = true;
            }
        }

        if (!canSeePlayer)
        {
            investigateTimer = investigateTime;

            agent.isStopped = false;

            state = State.Investigate;
        }
    }
    void Investigate()
    {
        agent.isStopped = false;

        agent.speed = patrolSpeed;

        agent.SetDestination(lastSeenPosition);

        if (!agent.pathPending &&
            agent.remainingDistance <= 1f)
        {
            investigateTimer -= Time.deltaTime;

            if (investigateTimer <= 0)
            {
                state = State.Patrol;

                if (patrolPoints.Length > 0)
                {
                    patrolIndex =
                        Random.Range(
                            0,
                            patrolPoints.Length);

                    agent.SetDestination(
                        patrolPoints[patrolIndex].position);
                }
            }
        }
    }
    bool IsBeingLookedAt()
    {
        Vector3 eyePos =
            playerCamera.position;

        Vector3 dirToEnemy =
            (transform.position - eyePos).normalized;

        float angle =
            Vector3.Angle(playerCamera.forward, dirToEnemy);

        if (angle > 45f)
            return false;

        float distance =
            Vector3.Distance(eyePos, transform.position);

        if (Physics.Raycast(eyePos, dirToEnemy, out RaycastHit hit, distance))
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
    void UpdateAudio()
    {
        if (audioSource == null || player == null)
            return;

        AudioClip targetClip = null;

        if (state == State.Chase && IsBeingLookedAt())
        {
            targetClip = frozenSound;
            audioSource.pitch = 0.6f;
        }
        else
        {
            switch (state)
            {
                case State.Patrol:
                    targetClip = patrolSound;
                    break;

                case State.Investigate:
                    targetClip = investigateSound;
                    break;

                case State.Chase:
                    targetClip = chaseSound;
                    break;
            }
        }
        switch (state)
        {
            case State.Patrol:
                audioSource.pitch = 0.9f;
                break;

            case State.Investigate:
                audioSource.pitch = 0.75f;
                break;

            case State.Chase:
                audioSource.pitch = 1.15f;
                break;
        }

        if (targetClip != currentClip)
        {
            currentClip = targetClip;

            audioSource.clip = currentClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        float distance =
            Vector3.Distance(
                transform.position,
                player.position);

        float t =
            Mathf.Clamp01(
                1f - distance / maxSoundDistance);

        audioSource.volume =
            Mathf.Lerp(
                minVolume,
                maxVolume,
                t);
    }
}