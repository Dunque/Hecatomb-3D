using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    EnemyLocomotionManager enemyLocomotionManager;
    bool isPerformingAction;
    
    [Header("AI Settings")]
    public float detectionRadius = 2;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;

    // Start is called before the first frame update
    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleCurrentAction();
    }

    private void HandleCurrentAction() {
        if (enemyLocomotionManager.currentTarget == null) {
            enemyLocomotionManager.HandleDetection();
        }
    }
}
