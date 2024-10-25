using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum AiState
{
    Idle,
    Wandering,
    Attacking
}

public class Npc : MonoBehaviour, IDamageable
{
    [Header("Stat")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropItems;

    [Header("AI")]
    private NavMeshAgent agent;
    public float detectDistance;
    private AiState aiState;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    private float playerDistance;

    public float fieldOfView = 120f;

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        SetState(AiState.Wandering);
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

        animator.SetBool("Moving", aiState != AiState.Idle);

        switch(aiState)
        {
            case AiState.Idle:
            case AiState.Wandering:
                PassiveUpdate();
                break;
            case AiState.Attacking:
                AttackingUpdate();
                break;
        }
    }

    public void SetState(AiState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AiState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;

            case AiState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;

            case AiState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        animator.speed = agent.speed / walkSpeed;
    }

    private void PassiveUpdate()
    {
        if(aiState == AiState.Wandering && agent.remainingDistance<0.1f)
        {
            SetState(AiState.Idle);
            Invoke("WanderToNewRocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }

        if(playerDistance < detectDistance)
        {
            SetState(AiState.Attacking);
        }
    }

    private void WanderToNewRocation()
    {
        if (aiState != AiState.Idle) return;

        SetState(AiState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    private Vector3 GetWanderLocation()
    {
        NavMeshHit hit;
        int i = 0;
        do
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i >= 30) break;
        }
        while (Vector3.Distance(transform.position, hit.position) < detectDistance) ;

        return hit.position;
    }

    private void AttackingUpdate()
    {
        if(playerDistance < attackDistance && IsPlayerInFov())
        {
            agent.isStopped = true;

            if(Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                CharacterManager.Instance.Player.controller.GetComponent<IDamageable>().TakeDamage(damage);
                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            if(playerDistance < detectDistance)
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if(agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
                {
                    agent.SetDestination(CharacterManager.Instance.Player.transform.position);
                }
                else
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(AiState.Wandering);
                }
            }
            else
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AiState.Wandering);
            }
        }
    }

    private bool IsPlayerInFov()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        return angle < fieldOfView * 0.5f;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if(health <= 0)
        {
            OnDie();
            return;
        }

        StartCoroutine(DamageFlash());
    }

    private void OnDie()
    {
        for(int i = 0; i < dropItems.Length; i++)
        {
            Instantiate(dropItems[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private IEnumerator DamageFlash()
    {
        for(int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = new Color(1f, 0f, 0f);
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = Color.white;
        }
    }
}
