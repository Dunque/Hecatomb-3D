using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocomotionManager : MonoBehaviour {

    EnemyManager enemyManager;
    public EntityStats currentTarget;
    public LayerMask detectionLayer;

    private void Awake() {
        enemyManager = GetComponent<EnemyManager>();
    }

    public void HandleDetection(){
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);
        Debug.Log(colliders);

        for (int i = 0; i < colliders.Length; i++) {
            EntityStats entityStats = colliders[i].transform.GetComponent<EntityStats>();
            if (entityStats != null) {
                Vector3 targetDirection = entityStats.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle) {
                    currentTarget = entityStats;
                }
            }
        }
    }

    void OnDrawGizmosSelected() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, enemyManager.detectionRadius);
    }
}
