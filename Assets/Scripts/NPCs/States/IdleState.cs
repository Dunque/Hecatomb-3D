using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public PursueTargetState pursueTargetState;
    public LayerMask detectionLayer;

    public override State Tick(EnemyManager enemyManager, BaseEnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager) {
        #region Handle Enemy Target Detection
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++) {
            EntityStats entityStats = colliders[i].transform.GetComponent<EntityStats>();
            if (entityStats != null) {
                Vector3 targetDirection = entityStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
                
                if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle) {
                    enemyManager.currentTarget = entityStats;
                }
            }
        }
        #endregion

        #region Handle Switching To Next State
        if (enemyManager.currentTarget != null) {
            return pursueTargetState;
        } else {
            return this;
        }
        #endregion
    }

}
