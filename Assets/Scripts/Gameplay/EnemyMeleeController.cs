using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
public class EnemyMeleeController : MonoBehaviour {
    private enum Behaviour
    {
        IDLE,
        Attack,
        Damage,
        Dead,
        GO_TO_TARGET
    }
    private enum Animation
    { 
        IDLE,
        WALK,
        RUN,
        ATTACK,
        DAMAGE,
        DEAD
    }

    public Transform Target
    {
        get;
        set;
    }

    private bool AgentIsStop
    {
        get
        { return agent.isStopped; }
        set
        { agent.isStopped = value; }
    }

    [SerializeField] private TargetScanner targetScanner;
    [SerializeField] private SphereCollider damageBallCollider;

    [Header("AGENT SETTING")]
    [SerializeField] private float agentStopDistance = 0.1f;
    [SerializeField] private float agentSpeed = 0.5f;
    [SerializeField] private int agentAngularSpeed = 120;

    //[Header("ATTACK")]
    //[SerializeField] private float allowAttackDistance = 1f;

    [Header("DEAD")]
    [SerializeField] private float dissolveTime = 5f;

    [Header("DEBUG")]
    [SerializeField] private bool drawGizmos = false;

    private NavMeshAgent agent;
    private Animator animator;
    private Collider interactiveCollider;
    private Character character;
    private Vector3 spawnPosition;
    private Behaviour currentBehaviour;

    // ===========================================
    // Function for Monobehaviour
    // ===========================================

    void Awake()
    {
        character = GetComponent<Character>();
        interactiveCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.stoppingDistance = agentStopDistance;
        agent.speed = agentSpeed;
        agent.angularSpeed = agentAngularSpeed;
        //Debug.Log(agent.stoppingDistance);
    }

    private void OnEnable()
    {
        character.onDead += OnDead;
        character.onDamage += OnDamage;

        SceneLinkedSMB<EnemyMeleeController>.Initialise(animator, this);

        SetActiveCollider(true);
        //SetActiveDamageBall(false);

        spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void OnDisable()
    {
        character.onDead -= OnDead;
        character.onDamage -= OnDamage;
    }

    // ===========================================
    // Function for Behaviour
    // ===========================================

    public void BackToIdleMode()
    {
        AgentIsStop = false;
        SetAnimatorTrigger(Animation.IDLE.ToString()) ;
    }
    
    public void StartGoToTarget()
    {
        AgentIsStop = false;
        SetAnimatorTrigger(Animation.WALK.ToString());
    }

    public void GoToDestinition()
    {
        if (agent != null)
        {
            if (Target != null)
                agent.SetDestination(Target.position);
            else
                Debug.LogWarning("Target is null");
        }
        else
        {
            Debug.LogWarning("your agent is null");
        }
    }

    public bool ReadyToAttack()
    {
        if (Target == null)
            return false;
        
        float dis = Vector3.Distance(Target.position, this.transform.position);
        if (dis <= agentStopDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StartToAttack()
    {
        AgentIsStop = true;
        SetAnimatorTrigger(Animation.ATTACK.ToString());
    }

    //public void ProcessDetectionResult()
    //{
    //    Debug.Log(currentBehaviour.ToString());

    //    if (Target != null)
    //    {
    //        switch (currentBehaviour)
    //        {
    //            case Behaviour.Follow:
    //                if (TargetInAttackRegion())
    //                {
    //                    SetAnimatorTrigger(Ani_Attack);
    //                }
    //                break;
    //            case Behaviour.Attack:
    //                if (TargetInAttackRegion())
    //                {
    //                    SetAnimatorTrigger(Ani_Attack);
    //                }
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("Not detect any target!!!!");
    //    }
    //}

    //public void StartFacingTarget()
    //{
    //    if (Target != null)
    //        transform.LookAt(new Vector3(Target.position.x, transform.position.y, Target.position.z));
    //}

    //public void StartDamage()
    //{
    //    Debug.Log("StartDead");
    //    AgentIsStop = true;
    //}

    //public void StartDead()
    //{
    //    Debug.Log("StartDead");
    //    AgentIsStop = true;
    //}

    //public void StartAttack()
    //{
    //    Debug.Log("StartAttack");
    //    SetCurrentBehaviour(Behaviour.Attack);
    //    AgentIsStop = true;
    //}

    //public void StartWander()
    //{
    //    AgentIsStop = false;

    //    agent.SetDestination(GetRandomPosition(transform.position, 100f, 1 << NavMesh.GetAreaFromName("Walkable")));
    //}

    //public void StopWander()
    //{
    //    AgentIsStop = true;

    //    SetAnimatorInteger(Ani_Idle, Random.Range(1, 5));
    //}

    //public void StartIdle()
    //{
    //    SetCurrentBehaviour(Behaviour.IDLE);
    //    ResetIndleInteger();

    //    AgentIsStop = true;
    //}

    //public void ResetIndleInteger()
    //{
    //    SetAnimatorInteger(Ani_Idle, 0);
    //}

    //public void SetAgentSpeed(float speed)
    //{
    //    agent.speed = speed;
    //}

    //public Transform DetectClosedTarget()
    //{
    //    Target = null;

    //    if (PlayerManager.Instance.ExistPlayer)
    //    {
    //        List<Transform> playlist = PlayerManager.Instance.PlayerList;
    //        float minDis = float.MaxValue;
    //        float dis = 0f;
    //        for (int i = 0; i < playlist.Count; i++)
    //        {
    //            dis = Vector3.Distance(playlist[i].position, transform.position);
    //            if (dis <= minDis)
    //            {
    //                Target = playlist[i];
    //                minDis = dis;
    //            }
    //        }
    //    }

    //    return Target;
    //}

    //public bool ArrivedSpawnPosition()
    //{
    //    bool result = false;
    //    float dis = -1f;
    //    dis = Vector3.Distance(spawnPosition, this.transform.position);
    //    if (dis < 0.1f)
    //    {
    //        result = true;
    //    }

    //    return result;
    //}

    public void Disappear()
    {
        this.transform.parent.gameObject.SetActive(false);
    }

    // ===========================================
    // private Function
    // ===========================================

    //private void SetAnimationSpeed(string parameter, float speed)
    //{
    //    if (animator != null)
    //    {
    //        animator.SetFloat(parameter, speed);
    //    }
    //}

    private void SetActiveCollider(bool status)
    {
        if (interactiveCollider != null)
            interactiveCollider.enabled = status;
    }

    //private void CountDownForDisappear()
    //{
    //    Disappear();
    //}

    //private bool TargetInAttackRegion()
    //{
    //    bool fight = false;

    //    float dis = -1f;
    //    dis = Vector3.Distance(Target.position, this.transform.position);

    //    if (dis <= agent.stoppingDistance + allowAttackDistance)
    //    {
    //        fight = true;
    //    }

    //    return fight;
    //}

    private void SetAnimatorTrigger(string triggerName)
    {
        if (animator != null && !animator.IsInTransition(0))
        {
            animator.SetTrigger(triggerName);
        }
    }

    //private void SetAnimatorInteger(string triggerName, int intVal)
    //{
    //    if (animator != null)
    //    {
    //        animator.SetInteger(triggerName, intVal);
    //    }
    //}

    //private static Vector3 GetRandomPosition(Vector3 origin, float distance, int layermask)
    //{
    //    Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

    //    randomDirection += origin;

    //    NavMeshHit navHit;

    //    NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

    //    return navHit.position;
    //}

    //private void CountDownForWander()
    //{
    //    //StopWander();
    //    CancelInvoke("CountDownForWander");
    //}

    private void SetCurrentBehaviour(Behaviour behaviour)
    {
        currentBehaviour = behaviour;
    }

    //private Behaviour GetCurrentBehaviour()
    //{
    //    return currentBehaviour;
    //}

    // ===========================================
    // public Function
    // ===========================================

    public void Detect()
    {
        Target = null;

        if (PlayerManager.Instance.ExistPlayer)
        {
            List<Transform> playlist = PlayerManager.Instance.PlayerList;
            for (int i = 0; i < playlist.Count; i++)
            {
                if (Vector3.Distance(playlist[i].position, transform.position) <= targetScanner.detectionRadius)
                {
                    Target = playlist[i];
                }
            }
        }
    }

    public bool ExistTarget()
    {
        return Target != null ? true : false;
    }

    //public void SetActiveDamageBall(bool status)
    //{
    //    if (damageBallCollider != null)
    //    {
    //        damageBallCollider.enabled = status;
    //    }
    //}

    // ===========================================
    // Delegate Function
    // ===========================================

    private void OnDead()
    {
        Debug.Log("Dead: " + this.transform.parent.name);
        //SetAnimatorTrigger(Ani_Dead);

        SetActiveCollider(false);
        //AudioPlayer.Instance.PlayOneShot(deadClip);
        Invoke("CountDownForDisappear", dissolveTime);
    }

    private void OnDamage(int hurtVal)
    {
        Debug.Log("Damage: " + this.transform.parent.name);
        //SetAnimatorTrigger(Ani_Damage);
    }

    // ===========================================
    // Function for Drawing
    // ===========================================
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (drawGizmos)
            targetScanner.EditorGizmo(transform);
    }
#endif
}