using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerCharacter))]
public class PlayerBehaviour : MonoBehaviour
{
    public static class AnimationState
    {
        public static string IDLE   { get { return "Idle";  } }
        public static string RUN    { get { return "Run";   } }
        public static string WALK   { get { return "Walk";  } }
        public static string BACK   { get { return "Back";  } }
        public static string SHOOT  { get { return "Shoot"; } }
        public static string DEAD   { get { return "Dead";  } }
        public static string DAMAGE { get { return "Damage";} }
    }

    private PlayerCharacter playerCharacter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerCharacter = GetComponent<PlayerCharacter>();
    }

    private void OnEnable()
    {
        SceneLinkedSMB<PlayerBehaviour>.Initialise(animator, this);
        if (playerCharacter != null)
            playerCharacter.onDamage += OnDamage;

        if (playerCharacter != null)
            playerCharacter.onDead += OnDead;
    }

    private void OnDestroy()
    {
        if (playerCharacter != null)
            playerCharacter.onDamage -= OnDamage;

        if (playerCharacter != null)
            playerCharacter.onDead += OnDead;
    }

    //=================================
    // private function
    //=================================

    private void OnDamage(int val)
    {
        if (IsDead())
            return;

        Debug.Log("OnDamage: " + val);

        SetAnimation(PlayerBehaviour.AnimationState.DAMAGE);
        UIManager.Instance.RefreshHealthBar(val);
    }

    private void OnDead()
    {
        Debug.Log("OnDead");
       
        SetAnimation(PlayerBehaviour.AnimationState.DEAD);

        UIManager.Instance.RefreshHealthBar(0);
        UIManager.Instance.SetGameStatus(GameStatusUIManager.STATUS.GAMEOVER);
    }

    private bool IsDead()
    {
        if (playerCharacter != null)
            return playerCharacter.Dead;

        return false;
    }

    //=================================
    // public function
    //=================================

    public void SetAnimation(string aniName)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(aniName) && !animator.IsInTransition(0))
        {
            //Debug.LogWarning("Animaiton name: " + aniName);
            animator.SetTrigger(aniName);
        }
    }

    public void CrossFade(string state, float durationTime)
    { 
        if(animator != null)
        {
             animator.CrossFadeInFixedTime(state, durationTime);
        }
    }

    public Animator CurrentAnimator()
    {
        if (animator != null)
            return animator;
        else
            return null;
    }

    public void SetAnimatorSpeed(float speed)
    {
        if(animator != null)
        {
            animator.speed = speed;
        }
    }

    public void SetShootingSpeed(float speed)
    {
        if (animator != null)
        {
            animator.SetFloat("shootSpeed", speed);
        }
        else
        {
            Debug.LogWarning("the animaror is null!");
        }
    }
}
