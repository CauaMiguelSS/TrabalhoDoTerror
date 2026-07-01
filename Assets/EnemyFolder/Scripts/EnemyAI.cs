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
    public Animator animator;

    [Header("Attack")]
    public float stopDistance = 2f;

    [Header("Patrol")]
    public float waitTime = 2f;

    private State state = State.Patrol;
    private int patrolIndex;
    private float waitTimer;
    private bool jumpscareTriggered;
    private bool freezePlayed = false;
    private bool isFrozen = false;

    private Vector3 lastSeenPosition;
    private float investigateTimer;

    [Header("Investigation")]
    public float investigateTime = 5f;

    [Header("Proximity")]
    public float proximityRange = 4f;

    [Header("Hearing")]
    public float hearingRange = 8f;

    private bool chaseEventTriggered;
    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponent<Animator>();

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
        if (animator != null)
        {
            float speed = agent.velocity.magnitude;

            if (!agent.pathPending && agent.remainingDistance < 0.3f)
            {
                speed = 0f;
            }

            animator.SetFloat("Speed", speed);
        }

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

            if (!chaseEventTriggered)
            {
                chaseEventTriggered = true;

                LiveSystem.Instance.TriggerEvent(LiveEventType.ROBOT_CHASE);
            }
        }
    }

    void Patrol()
    {
        Debug.Log("ESTOU PATRULHANDO");
        agent.speed = patrolSpeed;

        if (animator != null)
        {
            animator.SetBool("Patrol", true);
            animator.SetBool("Chase", false);

            Debug.Log("PATROL TRUE");
        }

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
        float heightDifference = Mathf.Abs(transform.position.y - lastSeenPosition.y);
        if (animator != null)
        {
            animator.SetBool("Patrol", false);
            animator.SetBool("Chase", true);
        }

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
            agent.velocity = Vector3.zero;

            if (!freezePlayed)
            {
                freezePlayed = true;
                isFrozen = true;

                animator.speed = 1f;
                animator.SetTrigger("Freeze");
            }

            return;
        }
        if (isFrozen)
        {
            isFrozen = false;
            freezePlayed = false;

            animator.speed = 1f;
        }

        agent.speed = chaseSpeed;
        agent.isStopped = false;

        if (animator != null)
        {
            animator.SetBool("Chase", true);
        }

        agent.speed = chaseSpeed;
        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;

        agent.SetDestination(player.position);

        lastSeenPosition = player.position;

        Vector3 eye = transform.position + Vector3.up * 1.6f;
        Vector3 dir = (player.position - eye).normalized;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        float angle = Vector3.Angle(transform.forward, dir);

        bool canSeePlayer = false;

        if (distanceToPlayer <= viewDistance && angle <= viewAngle * 0.5f)
        {
            if (!Physics.Raycast(eye, dir, distanceToPlayer, obstacleMask))
            {
                canSeePlayer = true;
            }
        }

        if (!canSeePlayer)
        {
            investigateTimer = investigateTime;

            agent.isStopped = false;

            chaseEventTriggered = false;

            state = State.Investigate;
        }

        if (heightDifference > 2f)
        {
            state = State.Patrol;
            return;
        }
    }
    void Investigate()
    {
        if (animator != null)
        {
            animator.SetBool("Patrol", false);
            animator.SetBool("Chase", false);
        }
        agent.isStopped = false;

        agent.speed = patrolSpeed;

        agent.SetDestination(lastSeenPosition);

        if (!agent.pathPending && agent.remainingDistance <= 1f)
        {
            investigateTimer -= Time.deltaTime;

            if (investigateTimer <= 0)
            {
                state = State.Patrol;

                if (patrolPoints.Length > 0)
                {
                    patrolIndex = Random.Range(0,patrolPoints.Length);
                    agent.SetDestination(patrolPoints[patrolIndex].position);
                }
            }
        }
    }
    bool IsBeingLookedAt()
    {
        Vector3 eyePos = playerCamera.position;
        Vector3 dirToEnemy = (transform.position - eyePos).normalized;
        float angle = Vector3.Angle(playerCamera.forward, dirToEnemy);

        if (angle > 45f)
            return false;

        float distance = Vector3.Distance(eyePos, transform.position);

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
            agent.velocity = Vector3.zero;
            agent.ResetPath();
        }

        if (animator != null)
        {
            animator.SetTrigger("Freeze");
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

        float distance = Vector3.Distance(transform.position, player.position);

        float t = Mathf.Clamp01(1f - distance / maxSoundDistance);

        audioSource.volume = Mathf.Lerp(minVolume, maxVolume, t);
    }
    public void HearNoise(Vector3 noisePosition)
    {
        float dist =
            Vector3.Distance(transform.position, noisePosition);

        if (dist > hearingRange)
            return;

        lastSeenPosition = noisePosition;

        investigateTimer = investigateTime;

        state = State.Investigate;
    }
    public void StopFreezeAnimation()
    {
        animator.speed = 0f;
    }
}