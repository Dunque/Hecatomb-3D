using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PursueTargetTurretState : PursueTargetState
{
    public GameObject turret;
    public Turret turretGun;

    public AttackTurretState attackState;
    public IdleState idleState;

    Vector3 targetPosition;
    System.Random random = new System.Random();

    public override State Tick(EnemyManager enemyManager, BaseEnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager) {

        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        targetPosition = new Vector3(
            enemyManager.currentTarget.transform.position.x + (float)(random.NextDouble() * turretGun.spread),
            enemyManager.currentTarget.transform.position.y + (float)(random.NextDouble() * turretGun.spread),
            enemyManager.currentTarget.transform.position.z + (float)(random.NextDouble() * turretGun.spread));
        turret.transform.LookAt(targetPosition, transform.up);
        turret.transform.Rotate(-90, 0, -5);

        if (enemyManager.currentRecoveryTime <= 2) {
            turretGun.warningLight.enabled = true;
        } else {
            turretGun.warningLight.enabled = false;
        }


        if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange && distanceFromTarget > enemyManager.minimumAttackRange) {
            return attackState;
        } else if (distanceFromTarget < enemyManager.maximumAttackRange) {
            return this;
        } else {
            return idleState;
        }
    }
}
