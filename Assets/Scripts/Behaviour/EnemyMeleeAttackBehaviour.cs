using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttackBehaviour : SceneLinkedSMB<EnemyMeleeController>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStatePostEnter(animator, stateInfo, layerIndex);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log(stateInfo.normalizedTime);
        if (stateInfo.normalizedTime >= 1f)
        {
            if (m_MonoBehaviour.ReadyToAttack())
                m_MonoBehaviour.StartToAttack();
            else
                m_MonoBehaviour.BackToIdleMode();
        }
    }
}
