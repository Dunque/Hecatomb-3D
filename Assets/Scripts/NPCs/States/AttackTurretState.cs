using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTurretState : AttackState
{
    public PursueTargetTurretState pursueTargetTurretState;
    public Turret turret;
    public float shootTime = 5;

    public override State Tick(EnemyManager enemyManager, BaseEnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager) {
        if (enemyManager.isPerformingAttack)
            return pursueTargetTurretState;

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        if (distanceFromTarget < enemyManager.minimumAttackRange || distanceFromTarget > enemyManager.maximumAttackRange) {
            return pursueTargetTurretState;
        }

        if (enemyManager.currentTarget && enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAttack == false && enemyStats.isHurt == false) {
            //enemyAnimatorManager.PlayTargetAnimation("GunFire", true);

            enemyManager.isPerformingAttack = true;
            enemyManager.currentRecoveryTime = shootTime;
            turret.Shoot();
            return pursueTargetTurretState;
        }
        return pursueTargetTurretState;
    }

    

    }
