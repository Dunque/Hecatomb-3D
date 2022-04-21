using System;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("References")]
    public PlayerController controller;

    [Header("Bob")]
    public float walkingBobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;

    float defaultPosX;
    float defaultPosY;
    float defaultPosZ;

    float timer = 0;
    float wbs;

    [Header("Shake")]
    public float defaultShakeTime = 0.2f;
    public float angle = 30f;
    public float angleDash = 50f;
    public float xAmount = 1.5f;
    public float yAmount = 2f;
    public float timeP = 1f;
    public float timeR = 1f;
    public float timeDash = 1f;

    private float shakeUntil;
    private Action shakeFunc;

    public void ShakeCamera(Action shakeFunc)
    {
        float sTime = defaultShakeTime;
        shakeUntil = Mathf.Max(shakeUntil, Time.time + sTime);
        this.shakeFunc = shakeFunc;
    }

    public void DoShakeDashR()
    {
        Quaternion actualRot = transform.localRotation;
        Quaternion finalRot = Quaternion.Euler(transform.localEulerAngles.x, 0f, -angleDash);
        transform.localRotation = Quaternion.Slerp(actualRot, finalRot, Time.deltaTime * timeDash);
    }
    public void DoShakeDashL()
    {
        Quaternion actualRot = transform.localRotation;
        Quaternion finalRot = Quaternion.Euler(transform.localEulerAngles.x, 0f, angleDash);
        transform.localRotation = Quaternion.Slerp(actualRot, finalRot, Time.deltaTime * timeDash);
    }

    public void DoShakeV()
    {
        Vector3 actualPos = transform.localPosition;
        Vector3 finalPos = actualPos + Vector3.up * yAmount + Vector3.right * xAmount;
        Quaternion actualRot = transform.localRotation;
        Quaternion finalRot = Quaternion.Euler(transform.localEulerAngles.x, 0f, -angle);
        transform.localPosition = Vector3.Lerp(actualPos, finalPos, Time.deltaTime * timeP);
        transform.localRotation = Quaternion.Slerp(actualRot, finalRot, Time.deltaTime * timeR);
    }

    public void DoShakeV2()
    {
        Vector3 actualPos = transform.localPosition;
        Vector3 finalPos = actualPos + Vector3.up * -yAmount + Vector3.right * -xAmount;
        Quaternion actualRot = transform.localRotation;
        Quaternion finalRot = Quaternion.Euler(transform.localEulerAngles.x, 0f, angle);
        transform.localPosition = Vector3.Lerp(actualPos, finalPos, Time.deltaTime * timeP);
        transform.localRotation = Quaternion.Slerp(actualRot, finalRot, Time.deltaTime * timeR);
    }

    public void DoShakeH()
    {
        Vector3 actualPos = transform.localPosition;
        Vector3 finalPos = actualPos + transform.right * xAmount;
        Quaternion actualRot = transform.localRotation;
        Quaternion finalRot = Quaternion.Euler(transform.localEulerAngles.x, 0f, -angle);
        transform.localPosition = Vector3.Lerp(actualPos, finalPos, Time.deltaTime * timeP);
        transform.localRotation = Quaternion.Slerp(actualRot, finalRot, Time.deltaTime * timeR);
    }
    public void DoShakeH2()
    {
        Vector3 actualPos = transform.localPosition;
        Vector3 finalPos = actualPos + transform.right * -xAmount;
        Quaternion actualRot = transform.localRotation;
        Quaternion finalRot = Quaternion.Euler(transform.localEulerAngles.x, 0f, angle);
        transform.localPosition = Vector3.Lerp(actualPos, finalPos, Time.deltaTime * timeP);
        transform.localRotation = Quaternion.Slerp(actualRot, finalRot, Time.deltaTime * timeR);
    }
    public void DoShakeAir()
    {
        Vector3 actualPos = transform.localPosition;
        Vector3 finalPos = actualPos + transform.up * -yAmount;
        transform.localPosition = Vector3.Lerp(actualPos, finalPos, Time.deltaTime * timeP);
    }


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<PlayerController>();
        defaultPosY = transform.localPosition.y;
        defaultPosX = transform.localPosition.x;
        defaultPosZ = transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        wbs = walkingBobbingSpeed * (controller.body.velocity.magnitude / 7f);

        if (shakeFunc != null && shakeUntil >= Time.time)
        {
            shakeFunc.Invoke();
        }
        else
        {
            if (controller.playerInput.magnitude > 0.1f && controller.OnGround)
            {
                //Player is moving
                timer += Time.deltaTime * wbs;
                transform.localPosition = new Vector3(
                    Mathf.Lerp(transform.localPosition.x, defaultPosX, Time.deltaTime * 7f),
                    defaultPosY + Mathf.Sin(timer) * bobbingAmount,
                    Mathf.Lerp(transform.localPosition.z, defaultPosZ, Time.deltaTime * 7f)
                    );
            }
            else
            {
                //Idle
                timer = 0;
                transform.localPosition = new Vector3(
                    Mathf.Lerp(transform.localPosition.x, defaultPosX, Time.deltaTime * 7f),
                    Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * 7f),
                    Mathf.Lerp(transform.localPosition.z, defaultPosZ, Time.deltaTime * 7f)
                    );
            }
        }
    }
}
