using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetShootingState : PursueTargetState
{
    public override State Tick(EnemyManager enemyManager, BaseEnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager) {
        if (enemyManager.isPerformingAttack || enemyManager.isPerformingAction) {
            enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

        if (distanceFromTarget > enemyManager.maximumAttackRange || distanceFromTarget < enemyManager.minimumAttackRange) {
            enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }

        HandleRotateTowardsTarget(enemyManager);

        if (distanceFromTarget <= enemyManager.maximumAttackRange && distanceFromTarget > enemyManager.minimumAttackRange) {
            return combatStanceState;
        } else {
            return this;
        }
    }
}
