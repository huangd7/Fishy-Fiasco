using UnityEngine;
using UnityEngine.AI;

public class AnglerfishAI : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;

    public float viewDistance = 18f;
    public float viewAngle = 80f;
    public float hearingDistance = 12f;
    public KeyCode sprintKey = KeyCode.LeftShift;

    public float attackDistance = 1.8f;
    public float oxygenDrainPerSecond = 20f;

    public float patrolRadius = 10f;
    public float decisionInterval = 1f;

    enum State { Patrol, Investigate, Chase, Attack }
    State state = State.Patrol;

    Vector3 investigatePoint;
    bool hasInvestigatePoint;

    Vector3 lastSeenPos;
    bool hasLastSeen;

    float decisionTimer;

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
        if (player == null) return;

        Vector3 a = transform.position;
        Vector3 b = player.position;
        a.y = 0f;
        b.y = 0f;
        float dist = Vector3.Distance(a, b);

        bool canSee = CanSeePlayer();
        bool canHearSprint = dist <= hearingDistance && Input.GetKey(sprintKey);

        if (canSee)
        {
            lastSeenPos = player.position;
            hasLastSeen = true;
        }

        if (dist <= attackDistance)
        {
            state = State.Attack;
        }
        else if (canSee)
        {
            state = State.Chase;
        }
        else if (canHearSprint)
        {
            investigatePoint = player.position;
            hasInvestigatePoint = true;
            state = State.Investigate;
        }
        else
        {
            if (state != State.Patrol && (state == State.Investigate || state == State.Chase) && hasLastSeen)
            {
                investigatePoint = lastSeenPos;
                hasInvestigatePoint = true;
                state = State.Investigate;
            }
            else if (state != State.Patrol && !hasInvestigatePoint)
            {
                state = State.Patrol;
            }
        }

        if (state == State.Patrol) Patrol();
        if (state == State.Investigate) Investigate();
        if (state == State.Chase) ChaseWithProbabilities(dist);
        if (state == State.Attack) Attack(dist);
    }

    void Patrol()
    {
        if (!agent.hasPath || agent.remainingDistance <= 0.7f)
        {
            PickPatrolPoint();
        }
    }

    void Investigate()
    {
        if (!hasInvestigatePoint)
        {
            state = State.Patrol;
            return;
        }

        agent.SetDestination(investigatePoint);

        if (!agent.pathPending && agent.remainingDistance <= 1f)
        {
            hasInvestigatePoint = false;
            state = State.Patrol;
        }
    }

    void ChaseWithProbabilities(float dist)
    {
        decisionTimer -= Time.deltaTime;
        if (decisionTimer > 0f)
        {
            agent.SetDestination(player.position);
            return;
        }

        decisionTimer = decisionInterval;

        float chaseDirect = 2f;
        float goLastSeen = hasLastSeen ? 1f : 0f;
        float pause = 0.2f;

        if (dist > 12f) chaseDirect += 1f;
        if (!CanSeePlayer() && hasLastSeen) goLastSeen += 2f;
        if (Input.GetKey(sprintKey)) chaseDirect += 1f;

        float sum = chaseDirect + goLastSeen + pause;
        float r = Random.value * sum;

        if (r < chaseDirect)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else if (r < chaseDirect + goLastSeen)
        {
            agent.isStopped = false;
            agent.SetDestination(lastSeenPos);
        }
        else
        {
            agent.isStopped = true;
        }
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
            state = State.Investigate;
            if (hasLastSeen)
            {
                investigatePoint = lastSeenPos;
                hasInvestigatePoint = true;
            }
        }
    }

    bool CanSeePlayer()
    {
        Vector3 dir = (player.position - transform.position);
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