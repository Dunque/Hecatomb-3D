using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrolState : State
{
    public LayerMask detectionLayer;
    public float detectionRadius = 6;

    public LayerMask whatIsGround;
    public Vector3 walkPoint;
    bool walkpointSet;
    public float walkpointRange;

    public PursueTargetState pursueTargetState;

    public override State Tick(EnemyManager enemyManager, BaseEnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager) {
        #region Handle Target Detection

        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++) {
            PlayerStats playerStats = colliders[i].transform.GetComponent<PlayerStats>();

            if (playerStats != null) {
                Vector3 targetsDirection = playerStats.transform.position - enemyManager.transform.position;
                float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);

                if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle) {
                    enemyManager.currentTarget = playerStats;
                }
            }
        }
        #endregion

        #region Handle State Chang
        if (enemyManager.currentTarget != null) {
            return pursueTargetState;
        }
        enemyManager.navmeshAgent.enabled = true;
        if (!walkpointSet) SearchWalkPoint();
        if (walkpointSet) {
            enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            Vector3 targetVelocity = enemyManager.enemyRigidBody.velocity;
            enemyManager.navmeshAgent.SetDestination(walkPoint);
            enemyManager.enemyRigidBody.velocity = targetVelocity;
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1) {
            enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            walkpointSet = false;
        }
            

        return this;
        #endregion
    }

    private void SearchWalkPoint() {
        float randomZ = UnityEngine.Random.Range(-walkpointRange, walkpointRange);
        float randomX = UnityEngine.Random.Range(-walkpointRange, walkpointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y , transform.position.z+ randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) {
           walkpointSet = true;
        }
    }
}
