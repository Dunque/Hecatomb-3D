using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{

    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    public bool isPerformingAction;
    public bool isPerformingAttack;

    BaseEnemyStats enemyStats;
    public NavMeshAgent navmeshAgent;
    public Rigidbody enemyRigidBody;

    public EntityStats currentTarget;
    public State currentState;
    
    public float rotationSpeed = 15f;
    public float maximumAttackRange = 2f;
    public float minimumAttackRange = 2f;

    [Header("AI Settings")]
    public float detectionRadius = 6;
    public float maximumDetectionAngle = 50;
    public float minimumDetectionAngle = -50;

    public float currentRecoveryTime = 0;
    

    // Start is called before the first frame update
    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<BaseEnemyStats>();
        enemyRigidBody = GetComponent<Rigidbody>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();
        if(navmeshAgent)
            navmeshAgent.enabled = false;
    }

    private void Start() {
        if(enemyRigidBody)
            enemyRigidBody.isKinematic = false;
    }

    private void Update() {
        HandleRecoveryTimer();
        HandleStateMachine();
    }

    private void LateUpdate() {
        if (navmeshAgent) {
            navmeshAgent.transform.localPosition = Vector3.zero;
            navmeshAgent.transform.localRotation = Quaternion.identity;
        }
    }

    private void HandleStateMachine() {
        if (enemyStats.isDead) {
            currentState = null;
        }else if (currentState != null) {
            State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

            if (nextState != null) {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(State state) {
        currentState = state;
    }

    private void HandleRecoveryTimer() {
        if (currentRecoveryTime > 0) {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction) {
            if(currentRecoveryTime <= 0) {
                isPerformingAction = false;
            }
        }
        if (isPerformingAttack) {
            if (currentRecoveryTime <= 0) {
                isPerformingAttack = false;
            }
        }
    }

}
