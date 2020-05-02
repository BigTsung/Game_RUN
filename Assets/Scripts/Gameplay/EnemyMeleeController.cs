using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
public class EnemyMeleeController : MonoBehaviour {

    private static string Ani_Follow    = "Follow";
    private static string Ani_Damage    = "Damage";
    private static string Ani_Dead      = "Dead";
    private static string Ani_Idle      = "Idle";
    private static string Ani_Attack    = "Attack";
    private static string Ani_Wander    = "Wander";

    public TargetScanner targetScanner;
    public SphereCollider damageBallCollider;

    [Header("Wander")]
    public float minRandomWanderTime = 3f;
    public float maxRandomWanderTime = 7f;
    public float wanderAgentSpeed = 1f;
    public float wanderAnimationSpeed = 1f;

    [Header("Follow")]
    public float followAgentSpeed = 3f;
    public float followAnimationSpeed = 1f;

    [Header("Attack")]
    public float allowAttackDistance = 1f;

    [Header("Dead")]
    public float disappearTime = 5f;

    [Header("Sound")]
    public AudioClip deadClip;

    [Header("Audio")]
    //public RandomAudioPlayer audioGrout;

    [Header("Debug")]
    public bool drawGizmos = false;

    private NavMeshAgent agent;
    private Animator animator;
    private Collider m_collider;
    private Character character;
    private Vector3 spawnPosition;
    private Behaviour currentBehaviour;

    public enum Behaviour
    {
        Wander,
        Idle,
        Attack,
        Damage,
        Dead,
        Follow
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

    void Awake()
    {
        character = GetComponent<Character>();
        m_collider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Detect();
    }

    private void OnEnable()
    {
        this.transform.localPosition = Vector3.zero;

        character.onDead += OnDead;
        character.onDamage += OnDamage;

        SceneLinkedSMB<EnemyMeleeController>.Initialise(animator, this);

        SetAnimationSpeed("WanderSpeed", wanderAnimationSpeed);
        SetAnimationSpeed("FollowSpeed", followAnimationSpeed);

        SetActiveCollider(true);
        SetActiveDamageBall(false);

        spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void OnDisable()
    {
        character.onDead -= OnDead;
        character.onDamage -= OnDamage;
    }

    private void SetAnimationSpeed(string parameter, float speed)
    {
        if(animator != null)
        {
            animator.SetFloat(parameter, speed);
        }
    }

    private bool TargetInAttackRegion()
    {
        bool fight = false;

        float dis = -1f;
        dis = Vector3.Distance(Target.position, this.transform.position);

        if (dis <= agent.stoppingDistance + allowAttackDistance)
        {
            fight = true;
        }

        return fight;
    }

    private void SetAnimatorTrigger(string triggerName)
    {
        if (animator != null && !animator.IsInTransition(0))
        {
            //Debug.Log(this.name + ": " + triggerName);
            animator.SetTrigger(triggerName);
        }
    }

    private void SetAnimatorInteger(string triggerName, int intVal)
    {
        if (animator != null)
        {
            animator.SetInteger(triggerName, intVal);
        }
    }

    private static Vector3 GetRandomPosition(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    private void CountDownForWander()
    {
        //Debug.Log("CountDownForWander");
        StopWander();
        CancelInvoke("CountDownForWander");
    }

    private void SetCurrentBehaviour(Behaviour behaviour)
    {
        currentBehaviour = behaviour;
    }

    private Behaviour GetCurrentBehaviour()
    {
        return currentBehaviour;
    }

    // ===========================================
    // Function for Behaviour
    // ===========================================


    public void SetAgentDestinition(Vector3 target)
    {
        if (agent != null)
            agent.SetDestination(target);
    }

    public void ProcessDetectionResult()
    {
        Debug.Log(currentBehaviour.ToString());

        if (Target != null)
        {
            switch (currentBehaviour)
            {
                case Behaviour.Wander:
                    SetAnimatorTrigger(Ani_Follow);
                    break;
                case Behaviour.Idle:
                    SetAnimatorTrigger(Ani_Follow);
                    break;
                case Behaviour.Follow:
                    if (TargetInAttackRegion())
                    {
                        SetAnimatorTrigger(Ani_Attack);
                    }
                    break;
                case Behaviour.Attack:
                    if (TargetInAttackRegion())
                    {
                        SetAnimatorTrigger(Ani_Attack);
                    }
                    else
                    {
                        SetAnimatorTrigger(Ani_Follow);
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("Didn't detect any target!!!!");
            if (GetCurrentBehaviour() != Behaviour.Wander)
                SetAnimatorTrigger(Ani_Wander);
        }
    }

    public void StartFacingTarget()
    {
        if (Target != null)
            transform.LookAt(new Vector3(Target.position.x, transform.position.y, Target.position.z));
    }

    // NEED TO FIX
    public void Detect()
    {
        Target = null;

        if (PlayerManager.Instance.ExistPlayer)
        {
            List<Transform> playlist = PlayerManager.Instance.PlayerList;
            for (int i = 0; i < playlist.Count; i++)
            {
                //Debug.Log(Vector3.Distance(playlist[i].position, transform.position));
                if (Vector3.Distance(playlist[i].position, transform.position) <= targetScanner.detectionRadius)
                {
                    Target = playlist[i];
                }
            }
        }
    }

    public void StartDamage()
    {
        Debug.Log("StartDead");
        AgentIsStop = true;
    }

    public void StartDead()
    {
        Debug.Log("StartDead");
        AgentIsStop = true;
    }

    public void StartAttack()
    {
        Debug.Log("StartAttack");
        SetCurrentBehaviour(Behaviour.Attack);
        AgentIsStop = true;
    }

    public void StartFollow()
    {
        SetCurrentBehaviour(Behaviour.Follow);
        SetAgentSpeed(followAgentSpeed);
        AgentIsStop = false;
    }

    public void StartWander()
    {
        SetCurrentBehaviour(Behaviour.Wander);
        SetAgentSpeed(wanderAgentSpeed);
        AgentIsStop = false;

        agent.SetDestination(GetRandomPosition(transform.position, 100f, 1 << NavMesh.GetAreaFromName("Walkable")));

        float randomTime = Random.Range(minRandomWanderTime, maxRandomWanderTime);
        Invoke("CountDownForWander", randomTime);
    }

    public void StopWander()
    {
        if (GetCurrentBehaviour() != Behaviour.Wander)
            return;

        AgentIsStop = true;

        SetAnimatorInteger(Ani_Idle, Random.Range(1, 5));
    }

    public void StartIdle()
    {
        SetCurrentBehaviour(Behaviour.Idle);
        ResetIndleInteger();
        //FacingTarget = false;
        AgentIsStop = true;
    }

    public void ResetIndleInteger()
    {
        SetAnimatorInteger(Ani_Idle, 0);
    }

    public void SetAgentSpeed(float speed)
    {
        agent.speed = speed;
        //Debug.Log("Agent speed: " + agent.speed);
    }

    // NEED TO FIX
    public Transform DetectClosedTarget()
    {
        Target = null;

        //if (PlayerManager.Instance.ExistPlayer)
        //{
        //    List<Transform> playlist = PlayerManager.Instance.PlayerList;
        //    float minDis = float.MaxValue;
        //    float dis = 0f;
        //    for (int i = 0; i < playlist.Count; i++)
        //    {
        //        dis = Vector3.Distance(playlist[i].position, transform.position);
        //        if (dis <= minDis)
        //        {
        //            Target = playlist[i];
        //            minDis = dis;
        //        }
        //    }
        //}

        return Target;
    }

    public void Grout()
    {
        //if (audioGrout != null)
        //    audioGrout.PlayRandomClip();
    }

    public bool ArrivedSpawnPosition()
    {
        bool result = false;
        float dis = -1f;
        dis = Vector3.Distance(spawnPosition, this.transform.position);
        if (dis < 0.1f)
        {
            result = true;
        }

        return result;
    }

    public void Disappear()
    {
        this.transform.parent.gameObject.SetActive(false);
    }

    // ===========================================
    // private Function
    // ===========================================

    private void SetActiveCollider(bool status)
    {
        if (m_collider != null)
            m_collider.enabled = status;
    }

    private void CountDownForDisappear()
    {
        Disappear();
    }
    // ===========================================
    // public Function
    // ===========================================

    public void SetActiveDamageBall(bool status)
    {
        if (damageBallCollider != null)
        {
            damageBallCollider.enabled = status;
        }
    }

    // ===========================================
    // Delegate Function
    // ===========================================

    private void OnDead()
    {
        Debug.Log("Dead: " + this.transform.parent.name);
        SetAnimatorTrigger(Ani_Dead);

        SetActiveCollider(false);
        //AudioPlayer.Instance.PlayOneShot(deadClip);
        Invoke("CountDownForDisappear", disappearTime);
    }

    private void OnDamage(int hurtVal)
    {
        Debug.Log("Damage: " + this.transform.parent.name);
        SetAnimatorTrigger(Ani_Damage);
    }

    // ===========================================
    // Function for Drawing
    // ===========================================
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(drawGizmos)
            targetScanner.EditorGizmo(transform);
    }
#endif
}