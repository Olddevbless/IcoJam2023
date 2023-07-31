using CommonComponents;
using CommonComponents.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyFSM;

public class EnemyFSM : Damagable, IDamageDealer
{
    [SerializeField] private EnemyStates enemyCurrentState;
    [SerializeField] private GameObject[] patrolPoints;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;

    private NavMeshAgent agent;
    private FieldOfView fieldOfView;
    private Animator animator;
    private BreakApart breakApart;
    private bool hasCheckedState = false;
    

    [Header("FSM")]
    [SerializeField] float checkStateTimer;
    [SerializeField] float checkStateTime;

    public int Damage { get => 1; set => throw new System.NotImplementedException(); }

    public enum EnemyStates
    {
        Patrol,
        Charge,
        Attack,
        Dodge,
        Dead
    }

    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponentInChildren<Animator>();
        fieldOfView = GetComponent<FieldOfView>();
        agent = GetComponent<NavMeshAgent>();
        enemyCurrentState = EnemyStates.Patrol;
        breakApart = GetComponent<BreakApart>();
    }
    private void Update()
    {
        checkStateTimer -= Time.deltaTime;
        if (currentHP<=0)
        {
            enemyCurrentState = EnemyStates.Dead;
        }
        if(checkStateTimer<=0)
        {
            CheckState();
            checkStateTimer = checkStateTime;
        }

        
    }
    public void CheckState()
    {
        switch (enemyCurrentState)
        {
            case EnemyStates.Patrol:
                enemyCurrentState=EnemyStates.Charge;
                break;
            case EnemyStates.Charge:
                Charge();
                break;
            case EnemyStates.Attack:
                Attack();
                break;
            case EnemyStates.Dead:
                Dead();
                break;
        }
    }
    private float CheckDistanceFromTarget(GameObject target)
    {
        target = fieldOfView.playerRef;
        var distance = target.transform.position - transform.position;
        return distance.magnitude;
    }
    #region StateMethods
    private void Attack()
    {
        
        RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, attackRange, Vector3.forward, attackRange);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                
                if (hit.collider.gameObject.GetComponent<PlayerController>().shieldIsRaised)
                {
                    hit.collider.gameObject.GetComponent<PlayerController>().BreakShield();
                }
                else
                {
                    hit.collider.gameObject.GetComponent<PlayerController>().TakeDamage(Damage);
                    hit.collider.attachedRigidbody.AddForce(this.transform.forward * 5, ForceMode.Impulse);
                }
                
            }
        }
        if (CheckDistanceFromTarget(fieldOfView.playerRef) > attackDistance)
        {
            
            enemyCurrentState = EnemyStates.Charge;
        }
    }
    private void Dead()
    {
        breakApart.isBreaking = true;
        this.GetComponent<CapsuleCollider>().enabled = false;
        Destroy(this.gameObject, 3f);
    }
    private void Charge()
    {
        this.GetComponent<NavMeshAgent>().destination = fieldOfView.playerRef.transform.position;
        if (CheckDistanceFromTarget(fieldOfView.playerRef) <= attackDistance)
        {
            enemyCurrentState = EnemyStates.Attack;
        }
        else
        {
            agent.destination = fieldOfView.playerRef.transform.position;
        }
    }
    private void Patrol()
    {
        if (fieldOfView.canSeePlayer == false)
        {
            animator.SetBool("isWalking", true);
            agent.destination = patrolPoints[Random.Range(0, patrolPoints.Length)].transform.position;
            Vector3 currentNavTarget = agent.destination;
            if (Vector3.Distance(transform.position, currentNavTarget) < 0.1f)
            {
                agent.destination = patrolPoints[Random.Range(0, patrolPoints.Length)].transform.position;
            }
        }
        else
        {
            enemyCurrentState = EnemyStates.Charge;
        }
    }
    #endregion
}

