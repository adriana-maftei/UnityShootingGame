using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    NavMeshAgent agent;
    Animator anim;
    SoundManager soundMNG;
    [SerializeField] Transform player;
    [SerializeField] GameObject bloodParticle;

    [Header("Enemy Configurations")]
    [SerializeField] List<GameObject> patrolPoints = new List<GameObject>(); 
    float health = 100f;
    int damage = 10;
    int curPatrolIndex = -1;
    float visionDistance = 30f;
    float visionAngle = 90f;
    float waitTimer = 0;
    float timeBetweenAttacks = 1.5f; 
    float meleeDistance = 2f;

    enum STATE { PATROL, CHASE, ATTACK }
    STATE currentState = STATE.PATROL;

    void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
        soundMNG = this.GetComponent<SoundManager>();
        if (patrolPoints.Count != 0) ChangeState(STATE.PATROL);
    }

    void Update()
    {
        switch (currentState)
        {
            case STATE.PATROL:
                if (CanSeePlayer()) ChangeState(STATE.CHASE);
                else if (Random.Range(0, 100) < 10) ChangeState(STATE.PATROL);

                if (agent.remainingDistance < 1)
                {
                    if (curPatrolIndex >= patrolPoints.Count - 1) curPatrolIndex = 0;
                    else curPatrolIndex++;
                    agent.SetDestination(patrolPoints[curPatrolIndex].transform.position);
                }

                if (CanSeePlayer()) ChangeState(STATE.CHASE);
                break;

            case STATE.CHASE:
                agent.SetDestination(player.position);
                if (agent.hasPath)
                {
                    if (CanAttackPlayer()) ChangeState(STATE.ATTACK);
                    else if (CanStopChase()) ChangeState(STATE.PATROL);
                }
                break;

            case STATE.ATTACK:
                LookPlayer();
                waitTimer += Time.deltaTime;
                if (waitTimer >= timeBetweenAttacks)
                {
                    if (CanAttackPlayer()) ChangeState(STATE.CHASE);
                    else ChangeState(STATE.PATROL);
                }
                break;
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        if (direction.magnitude < visionDistance && angle < visionAngle) return true;

        return false;
    }

    private void LookPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private bool CanAttackPlayer()
    {
        Vector3 direction = player.position - transform.position;
        if (direction.magnitude < meleeDistance) return true;
        
        return false;
    }

    private bool CanStopChase() 
    {
        Vector3 direction = player.position - transform.position;
        if (direction.magnitude > visionDistance) return true;
        
        return false;
    }

    private void ChangeState(STATE newState)
    {
        switch (currentState)
        {
            case STATE.PATROL:
                anim.ResetTrigger("isWalking");
                break;
            case STATE.CHASE:
                anim.ResetTrigger("isWalking");
                break;
            case STATE.ATTACK:
                anim.ResetTrigger("isAttacking");
                break;
        }
        switch (newState)
        {
            case STATE.PATROL:
                agent.speed = 2;
                agent.isStopped = false;

                float lastDist = Mathf.Infinity;
                for (int i = 0; i < patrolPoints.Count; i++)
                {
                    GameObject thisPatrolPoints = patrolPoints[i];
                    float distance = Vector3.Distance(transform.position, thisPatrolPoints.transform.position);
                    if (distance < lastDist)
                    {
                        curPatrolIndex = i - 1; 
                        lastDist = distance;
                    }
                }
                anim.SetTrigger("isWalking");
                break;
            case STATE.CHASE:
                agent.speed = 3;
                agent.isStopped = false;
                anim.SetTrigger("isWalking");
                break;
            case STATE.ATTACK:
                anim.SetTrigger("isAttacking");
                soundMNG.PlaySound("zombie");
                agent.isStopped = true;
                waitTimer = 0;
                break;
        }
        currentState = newState;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStats ps = other.GetComponent<PlayerStats>();
        if (ps != null) ps.takeDamagePlayer(damage);
    }

    public void takeDamageEnemy(float damage)
    {
        health -= damage;
        if (health <= 0f) DieEnemy();
    }
    private void DieEnemy()
    {
        anim.Play("die");
        soundMNG.enabled = false;
        this.enabled = false;
        agent.isStopped = true;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject, 1f);
    }
}
