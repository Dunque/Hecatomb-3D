using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    public AttackState attackState;
    public PursueTargetState pursueTargetState;
    public override State Tick(EnemyManager enemyManager, BaseEnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager) {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

        enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);

        if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange && viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle) {
            return attackState;
        } else if(distanceFromTarget > enemyManager.maximumAttackRange || (viewableAngle < enemyManager.minimumDetectionAngle || viewableAngle > enemyManager.maximumDetectionAngle)) {
            return pursueTargetState;
        } else {
            return this;
        }
    }
}
