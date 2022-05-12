using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackShootState : AttackState
{
    public ShootingEnemy shootingEnemy;
    public WeaponIK weaponIK;
    public Gun gun;
    public BaseEnemyStats baseEnemyStats;
    public float shootTime = 1;

    public override State Tick(EnemyManager enemyManager, BaseEnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager) {
        enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
        if (enemyManager.isPerformingAttack)
            return combatStanceState;

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

        if (distanceFromTarget < enemyManager.minimumAttackRange || distanceFromTarget > enemyManager.maximumAttackRange || (viewableAngle < enemyManager.minimumDetectionAngle || viewableAngle > enemyManager.maximumDetectionAngle)) {
            return combatStanceState;
        }

        if (gun.currentAmmo > 0 && shootingEnemy.currentTarget && enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAttack == false && baseEnemyStats.isHurt == false) {
            enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            enemyAnimatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);

            enemyAnimatorManager.PlayTargetAnimation("GunFire", true);

            enemyManager.isPerformingAttack = true;
            enemyManager.currentRecoveryTime = shootTime;
            gun.Shoot();
            return combatStanceState;
        }
        return this;
    }

    

    }
