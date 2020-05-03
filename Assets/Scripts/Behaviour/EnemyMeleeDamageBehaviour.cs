using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeDamageBehaviour : SceneLinkedSMB<EnemyMeleeController>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStatePostEnter(animator, stateInfo, layerIndex);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
