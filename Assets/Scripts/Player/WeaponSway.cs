using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Position")]
    public float amount = 0.02f;
    public float maxAmount = 0.06f;
    public float smoothAmount = 6f;

    [Header("Rotation")]
    public float rotationAmount = 4f;
    public float maxRotationAmount = 5f;
    public float smoothRotation = 12f;

    [Space]
    public bool rotationX = true;
    public bool rotationY = true;
    public bool rotationZ = true;

    private Vector3 initPos;
    private Quaternion initRot;

    private float InputX;
    private float InputY;



    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.localPosition;
        initRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateSway();

        MoveSway();
        TiltSway();
    }

    private void CalculateSway()
    {
        InputX = -Input.GetAxis("Mouse X");
        InputY = -Input.GetAxis("Mouse Y");
    }

    private void MoveSway()
    {
        float moveX = Mathf.Clamp(InputX * amount, -maxAmount, maxAmount);
        float moveY = Mathf.Clamp(InputY * amount, -maxAmount, maxAmount);

        Vector3 finalPos = new Vector3(moveX, moveY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + initPos, Time.deltaTime * smoothAmount);
    }

    private void TiltSway()
    {
        float tiltY = Mathf.Clamp(InputX * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(InputY * rotationAmount, -maxRotationAmount, maxRotationAmount);

        Quaternion finalRot = Quaternion.Euler(new Vector3(
            rotationX ? -tiltX : 0f,
            rotationY ? tiltY : 0f,
            rotationZ ? tiltY : 0f
            ));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRot * initRot, Time.deltaTime * smoothRotation);
    }




}
