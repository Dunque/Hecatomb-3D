using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeDamage : StateMachineBehaviour
{
    PlayerController pc;
    float initialdmg;
    float initialkb;

    float timeElapsed;
    float lerpDuration = 1.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pc = animator.GetComponentInParent<PlayerController>();
        initialdmg = pc.damage;
        initialkb = pc.knockback;
        timeElapsed = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timeElapsed < lerpDuration)
        {
            pc.damage = Mathf.Lerp(initialdmg, initialdmg * 2f, timeElapsed / lerpDuration);
            pc.knockback = Mathf.Lerp(initialkb, initialkb * 1.5f, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            pc.damage = initialdmg * 2f;
            pc.knockback = initialkb * 1.5f;
        }
    }
}
