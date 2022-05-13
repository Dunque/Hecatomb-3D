using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBone {
    public HumanBodyBones bone;
    public float weight = 1.0f; 
}

public class WeaponIK : MonoBehaviour
{
    public Transform targetTransform;
    public Transform aimTransform;
    public Vector3 targetOffset;
    
    public int iterations = 10;
    [Range(0, 1)]
    public float weight = 1.0f;

    public float angleLimit = 180.0f;
    public float distanceLimit = 1.5f;

    public HumanBone[] humanBones;
    Transform[] boneTransforms;

    private void Start() {
        Animator animator = GetComponent<Animator>();
        boneTransforms = new Transform[humanBones.Length];
        for(int i = 0; i < boneTransforms.Length; i++) {
            boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
        }
    }

    Vector3 GetTargetPosition() {
        Vector3 targetDirection = (targetTransform.position + targetOffset) - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if (targetAngle > angleLimit) {
            blendOut += (targetAngle - angleLimit) / 50.0f;
        }

        float targetDistance = targetDirection.magnitude;
        if (targetDistance < distanceLimit) {
            blendOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;
    }

    void LateUpdate()
    {
        if (aimTransform == null || targetTransform == null) {
            return;
        }

        Vector3 targetPosition = GetTargetPosition();
        for(int i = 0; i < iterations; i++) {
            for(int b = 0; b < boneTransforms.Length; b++) {
                Transform bone = boneTransforms[b];
                float boneWeight = humanBones[b].weight * weight;
                if (bone != null) {
                    AimAtTarget(bone, targetPosition, boneWeight);
                }
            }
        }
        
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight) {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }
    
    public void SetTargetTransform(Transform target) {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim) {
        aimTransform = aim;
    }
}
