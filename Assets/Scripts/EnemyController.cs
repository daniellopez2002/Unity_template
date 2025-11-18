using UnityEngine;
using UnityEngine.AI;

public enum NPCState
{
    Patrol,
    Scream,
    Chase,
    Attack
}

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public Transform Player;
    private NavMeshAgent _agent;
    private Animator _animator;
    private AudioSource _audio;

    [Header("States")]
    public NPCState CurrentState = NPCState.Patrol;

    [Header("Patrol Settings")]
    public Transform[] PatrolPoints;
    private int _patrolIndex;

    [Header("Detection")]
    public float DetectionRange = 10f;
    public float AttackRange = 1.8f;

    [Header("Scream Settings")]
    public float MinScreamInterval = 4f;
    public float MaxScreamInterval = 9f;
    public float MinScreamTime = 1.2f;
    public float MaxScreamTime = 2.1f;
    public AudioClip ScreamSound;
    private float _nextRandomScream;
    private float _screamTimer;

    [Header("Attack Settings")]
    public float AttackCooldown = 1.5f;
    private bool _canAttack = true;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();

        GoToNextPatrolPoint();
        ResetRandomScreamTimer();
    }

    private void Update()
    {
        if (GameManager.Instance.IsPause)
        {
            _agent.isStopped = true;
            return;
        }

        _animator.SetBool("Walk", _agent.velocity.sqrMagnitude > 0.1f);

        switch (CurrentState)
        {
            case NPCState.Patrol: PatrolUpdate(); break;
            case NPCState.Scream: ScreamUpdate(); break;
            case NPCState.Chase: ChaseUpdate(); break;
            case NPCState.Attack: AttackUpdate(); break;
        }

        if (CurrentState == NPCState.Patrol)
        {
            if (Vector3.Distance(transform.position, Player.position) < DetectionRange)
                ChangeState(NPCState.Scream);
        }
    }

    private void ResetRandomScreamTimer()
    {
        _nextRandomScream = Random.Range(MinScreamInterval, MaxScreamInterval);
    }

    private void ChangeState(NPCState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case NPCState.Patrol:
                _agent.isStopped = false;
                GoToNextPatrolPoint();
                break;

            case NPCState.Scream:
                _agent.isStopped = true;
                _screamTimer = Random.Range(MinScreamTime, MaxScreamTime);
                _agent.isStopped = true;
                _agent.velocity = Vector3.zero;
                _agent.ResetPath();
                _animator.SetTrigger("Scream");
                _audio.clip = ScreamSound;
                _audio.Play();
                break;

            case NPCState.Chase:
                _agent.isStopped = false;
                break;

            case NPCState.Attack:
                _agent.isStopped = true;
                _animator.SetTrigger("Attack");
                break;
        }
    }

    private void PatrolUpdate()
    {
        if (!_agent.pathPending && _agent.remainingDistance < 0.2f)
            GoToNextPatrolPoint();

        _nextRandomScream -= Time.deltaTime;
        if (_nextRandomScream <= 0f)
        {
            ChangeState(NPCState.Scream);
            ResetRandomScreamTimer();
        }

        if (Vector3.Distance(transform.position, Player.position) < DetectionRange)
        {
            ChangeState(NPCState.Scream);
            ResetRandomScreamTimer();
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (PatrolPoints.Length == 0) return;
        _agent.destination = PatrolPoints[_patrolIndex].position;
        _patrolIndex = (_patrolIndex + 1) % PatrolPoints.Length;
    }

    public void OnScreamAnimationFinished()
    {
        float dist = Vector3.Distance(transform.position, Player.position);

        if (dist < DetectionRange)
            ChangeState(NPCState.Chase);
        else
            ChangeState(NPCState.Patrol);

        ResetRandomScreamTimer();
    }


    private void ScreamUpdate()
    {
        
    }

    private void ChaseUpdate()
    {
        _agent.destination = Player.position;

        if (Vector3.Distance(transform.position, Player.position) <= AttackRange && _canAttack)
            ChangeState(NPCState.Attack);
    }

    private void AttackUpdate()
    {
        float distance = Vector3.Distance(transform.position, Player.position);

        if (distance <= 2.0f)
            PlayerController.Instance.Kill();
    }

    public void OnAttackAnimationFinished()
    {
        StartCoroutine(AttackCooldownRoutine());
    }

    private System.Collections.IEnumerator AttackCooldownRoutine()
    {
        _canAttack = false;
        yield return new WaitForSeconds(AttackCooldown);
        _canAttack = true;
        ChangeState(NPCState.Chase);
    }
}
