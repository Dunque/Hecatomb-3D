using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushState : State
{
    public bool isSleeping;
    public float detectionRadius = 6;
    public string sleepAnimation;
    public string wakeAnimation;
    public LayerMask detectionLayer;

    public PursueTargetState pursueTargetState;

    public override State Tick(EnemyManager enemyManager, BaseEnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager) {
        if (isSleeping) {
            //enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
            enemyAnimatorManager.anim.Play("Sleeping");
        }

        #region Handle Target Detection

        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i<colliders.Length; i++) {
            PlayerStats playerStats = colliders[i].transform.GetComponent<PlayerStats>();

            if (playerStats != null) {
                Vector3 targetsDirection = playerStats.transform.position - enemyManager.transform.position;
                float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);

                if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle && isSleeping) {
                    enemyManager.currentTarget = playerStats;
                    isSleeping = false;
                    enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                }
            }
        }

        #endregion

        #region Handle State Chang
        
        if (enemyManager.currentTarget != null) {
            return pursueTargetState;
        } else {
            return this;
        }

        #endregion
    }

}
