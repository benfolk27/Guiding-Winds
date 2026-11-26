using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum EnemyState
{
    Patrolling,
    Following,
    Attacking
}

public class EnemyController : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform player;

    [Header("Settings")] 
    [SerializeField] private float patrolWaitTime = 2f;
    [SerializeField] private float stopAtDistance = 1f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private float losePlayerTime = 3f;
    [SerializeField] private float attackRange = 1.2f;

    private NavMeshAgent _agent;
    private Animator _animator;
    private int _currentPatrolIndex;
    private bool _isWaiting;
    private EnemyState _state = EnemyState.Patrolling;
    private float _timeSinceLostPlayer;
    private bool _isAttacking;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        GoToNextPatrolPoint();
    }
    
    private void Update()
    {
        var distanceToPlayer = Vector3.Distance(player.position, transform.position);
        switch (_state)
        {
            case EnemyState.Patrolling:
                Patrol();
                if (distanceToPlayer <= detectionRange && CanSeePlayer())
                {
                    _state = EnemyState.Following;
                }

                break;
            case EnemyState.Following:
                FollowPlayer();
                if (distanceToPlayer <= attackRange)
                {
                    _state = EnemyState.Attacking;
                    StartAttack();
                }
                if (!CanSeePlayer())
                {
                    _timeSinceLostPlayer += Time.deltaTime;
                    if (_timeSinceLostPlayer >= losePlayerTime)
                    {
                        _state = EnemyState.Patrolling;
                        GoToClosestPatrolPoint();
                    }
                }
                else
                {
                    _timeSinceLostPlayer = 0f;
                }

                break;
            case EnemyState.Attacking:
                Attack();
                if (!_isAttacking && distanceToPlayer > attackRange)
                {
                    _state = EnemyState.Following;
                    _agent.isStopped = false;
                }

                break;
        }
        UpdateAnimations();
    }

    private void Attack()
    {
        _agent.isStopped = true;
        var direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    
    private IEnumerator TempAttackEnd()
    {
        yield return new WaitForSeconds(0.3f);

        _isAttacking = false;
    }

    private void OnAttackAnimationEnd()
    {
        _isAttacking = false;
    }

    private void StartAttack()
    {
        _agent.isStopped = true;
        Debug.Log("ATTACKING!");
        _isAttacking = true;
        _animator.SetTrigger("Attack");
        StartCoroutine(TempAttackEnd());
    }

    private void GoToClosestPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        var closestIndex = 0;
        var closestDistance = float.MaxValue;

        for (var i = 0; i < patrolPoints.Length; i++)
        {
            var distance = Vector3.Distance(transform.position, patrolPoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        
        _currentPatrolIndex = closestIndex;
        _agent.SetDestination(patrolPoints[_currentPatrolIndex].position);
    }

    private void FollowPlayer()
    {
        _agent.SetDestination(player.position);
    }

    private void Patrol()
    {
        if (_isWaiting) return;
        if (!_agent.pathPending && _agent.remainingDistance <= stopAtDistance)
        {
            StartCoroutine(WaitAtPatrolPoint());
        }
    }
    
    private IEnumerator WaitAtPatrolPoint()
    {
        _isWaiting = true;
        _agent.isStopped = true;
        
        yield return new WaitForSeconds(patrolWaitTime);

        _agent.isStopped = false;
        GoToNextPatrolPoint();
        _isWaiting = false;
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        _agent.SetDestination(patrolPoints[_currentPatrolIndex].position);
        _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void UpdateAnimations()
    {
        var isWalking = _agent.velocity.sqrMagnitude > 0.01f;
        _animator.SetBool("isWalking", isWalking);
    }

    private bool CanSeePlayer()
    {
        return IsFacingPlayer() && HasClearPathToPlayer();
    }

    private bool IsFacingPlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        dirToPlayer.y = 0f;
        var angle = Vector3.Angle(transform.forward, dirToPlayer);
        return angle <= viewAngle / 2f;
    }

    private bool HasClearPathToPlayer()
    {
        var dirToPlayer = player.position - transform.position;
        Vector3 origin = transform.position + dirToPlayer.normalized * 0.6f;
        if (Physics.Raycast(origin, dirToPlayer.normalized, out RaycastHit hit, dirToPlayer.magnitude - 0.6f))
        {
            bool seePlayer = player || hit.transform.IsChildOf(player);
            Debug.DrawLine(origin, hit.point, seePlayer ? Color.green : Color.red);
            return seePlayer;
        }
        return true;
    }
}
