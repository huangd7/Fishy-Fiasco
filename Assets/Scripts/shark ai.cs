using UnityEngine;
using UnityEngine.AI;

public class sharkai : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;

    public float viewDistance = 30f;
    public float viewAngle = 140f;

    public float attackDistance = 2.2f;
    public float oxygenDrainPerSecond = 28f;

    public float patrolRadius = 18f;

    enum State { Patrol, Chase, Attack }
    State state = State.Patrol;

    OxygenSystem oxygen;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (player != null) oxygen = player.GetComponent<OxygenSystem>();

        PickPatrolPoint();
    }

    void Update()
    {
        if (player == null || agent == null) return;

        Vector3 a = transform.position;
        Vector3 b = player.position;
        a.y = 0f;
        b.y = 0f;
        float dist = Vector3.Distance(a, b);

        if (dist <= attackDistance) state = State.Attack;
        else if (CanSeePlayer()) state = State.Chase;
        else state = State.Patrol;

        if (state == State.Patrol) Patrol();
        if (state == State.Chase) Chase();
        if (state == State.Attack) Attack(dist);
    }

    void Patrol()
    {
        agent.isStopped = false;

        if (!agent.hasPath || agent.remainingDistance <= 1f)
        {
            PickPatrolPoint();
        }
    }

    void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    void Attack(float dist)
    {
        agent.isStopped = true;

        if (oxygen != null)
        {
            oxygen.AddOxygen(-oxygenDrainPerSecond * Time.deltaTime);
        }

        if (dist > attackDistance + 0.6f)
        {
            agent.isStopped = false;
            state = State.Chase;
        }
    }

    bool CanSeePlayer()
    {
        Vector3 dir = player.position - transform.position;
        float dist = dir.magnitude;
        if (dist > viewDistance) return false;

        dir.Normalize();
        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > viewAngle * 0.5f) return false;

        Ray ray = new Ray(transform.position + Vector3.up * 0.8f, dir);
        if (Physics.Raycast(ray, out RaycastHit hit, viewDistance))
        {
            return hit.transform == player;
        }
        return false;
    }

    void PickPatrolPoint()
    {
        Vector3 random = transform.position + Random.insideUnitSphere * patrolRadius;
        random.y = transform.position.y;

        if (NavMesh.SamplePosition(random, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}