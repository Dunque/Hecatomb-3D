﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerInputSpace = default;

    [Header("State Machine")]
    [SerializeField] public PlayerState state;
    [SerializeField] public PlayerState State { get => state; set => state = value; }
    [SerializeField] public GroundedState groundedState;
    [SerializeField] public AirborneState airborneState;

    [Header("References")]
    public Rigidbody body;
    public Transform trans;
    public Transform camTrans;
    public Animator anim;
    public new Renderer renderer;
    public HeadBob shake;
    public Transform leftHand;
    public Transform rightHand;

    [Header("Movement")]
    [SerializeField, Range(0f, 100f)] public float maxSpeed = 7f;
    [SerializeField, Range(0f, 100f)] public float maxAcceleration = 30f, maxAirAcceleration = 5f;
    [SerializeField] public Vector2 playerInput;
    public Vector3 velocity, desiredVelocity, connectionVelocity;
    Vector3 contactNormal, steepNormal;
    Rigidbody connectedBody;
    Rigidbody previousConnectedBody;

    [Header("Jump")]
    [SerializeField, Range(0f, 10f)] public float jumpHeight = 5f;
    public bool jump = false;

    [Header("Ground Checks")]
    [SerializeField, Range(0, 90)] public float maxGroundAngle = 40f, maxStairsAngle = 50f;
    [SerializeField, Range(0f, 100f)] public float maxSnapSpeed = 100f;
    [SerializeField, Min(0f)] public float probeDistance = 1.5f;
    [SerializeField] public LayerMask probeMask = -1, stairsMask = -1;
    int groundContactCount, steepContactCount;
    [SerializeField] public bool OnGround => groundContactCount > 0;
    float minGroundDotProduct, minStairsDotProduct;
    public int stepsSinceLastGrounded, stepsSinceLastJump;
    Vector3 connectionWorldPosition, connectionLocalPosition;

    [Header("Dodge")]
    [SerializeField] public float dodgeAmount = 10f;
    [SerializeField] public float dodgeCooldown = 0.5f;
    [SerializeField] public float airDashAmount = 5f;
    public bool isDashing;
    public bool canDodge;
    public float dodgeTimer = 0f;
    public float isDashingTimer = 0f;
    public bool dashed = false;

    public struct GroundAttackData
    {
        public string[] animName;
        public float[] knockback;
        public Vector3[] knockbackDir;
        public float[] damage;
    };

    [Header("Combat")]
    [SerializeField] public GroundAttackData groundAttackData;
    [SerializeField] public HitboxStats weaponHitbox;
    [SerializeField] public int currentCombo;
    [SerializeField] public bool canAirAttack;
    [SerializeField] public bool canAttack;
    [SerializeField] public GameObject wpnNone;
    [SerializeField] public GameObject wpnShotgun;
    [SerializeField] public GameObject wpnRevolver;
    [SerializeField] public GameObject[] gunList;
    [SerializeField] public int currentGun = 0;

    [Header("Aiming")]
    [SerializeField] public Camera m_Camera;
    [SerializeField] public MouseLook mouseLook = new MouseLook();
    public Vector3 playerForward;
    public Vector3 playerRight;

    [Header("Sound")]
    [SerializeField] public AudioSource audio_Shotgun1;
    [SerializeField] public AudioSource audio_Shotgun2;
    [SerializeField] public AudioSource audio_Revolver1;
    [SerializeField] public AudioSource audio_SV;
    [SerializeField] public AudioSource audio_SV2;
    [SerializeField] public AudioSource audio_SV3;
    [SerializeField] public AudioSource audio_SH;
    [SerializeField] public AudioSource audio_Dash1;
    [SerializeField] public AudioSource audio_Dash2;
    [SerializeField] public AudioSource audio_Stinger1;
    [SerializeField] public AudioSource audio_Stinger2;

    void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
    }

    void Awake()
    {
        //initialize attack data
        groundAttackData.animName = new string[]{ "SwingV", "2SwingV", "SwingH"};
        groundAttackData.knockback = new float[]{ 5f, 6.5f, 10f};
        groundAttackData.knockbackDir = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero };
        groundAttackData.damage = new float[]{ 10f, 12f, 20f};

        //initialize states
        groundedState = new GroundedState(this);
        airborneState = new AirborneState(this);

        State = groundedState;
        State.ToState(this, groundedState);

        body = GetComponent<Rigidbody>();
        renderer = GetComponentInChildren<Renderer>();
        shake = GetComponentInChildren<HeadBob>();
        anim = GetComponentInChildren<Animator>();
        OnValidate();

        trans = transform;

        if (!m_Camera)
        {
            m_Camera = Camera.main;
        }
            
        camTrans = m_Camera.transform;
        playerInputSpace = camTrans;
        mouseLook.Init(trans, camTrans);

        //Adding weapons to the list
        //TODO temporal thing, remove this
        gunList = new GameObject[] { wpnNone, wpnShotgun, wpnRevolver };
    }

    public void ChangeGun(int newGun)
    {
        gunList[currentGun].SetActive(false);
        currentGun = newGun;
        gunList[currentGun].SetActive(true);
    }

    public void HandleInput()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            State.HandleInput(this);
    }

    void Update()
    {
        HandleInput();
        State.Update(this);
        mouseLook.UpdateCursorLock();

        // Rotate the character and camera.
        mouseLook.LookRotation(trans, camTrans);

        //Getting the move input
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        //Moving
        playerForward = playerInputSpace.forward;
        playerForward.y = 0f;
        playerForward.Normalize();
        playerRight = playerInputSpace.right;
        playerRight.y = 0f;
        playerRight.Normalize();
        desiredVelocity = (playerForward * playerInput.y + playerRight * playerInput.x) * maxSpeed;

        groundAttackData.knockbackDir = new Vector3[] { playerForward, playerForward, playerForward };

        //Dodging timer
        if (!canDodge || isDashing)
        {
            DodgeCooldown();
        }

        //Button release.
        SwingRelease();

        //Change color
        renderer.material.SetColor(
        "_BaseColor", OnGround ? Color.black : Color.white);
    }

    void FixedUpdate()
    {
        UpdateGroundCheck();
        AdjustVelocity();

        State.FixedUpdate(this);

        body.velocity = velocity;

        ClearGroundCheck();
    }

    //------------------------------------------------------------------------------------------------

    private void DodgeCooldown()
    {
        if (dodgeTimer > 0)
        {
            dodgeTimer -= Time.deltaTime;
        }
        else
        {
            canDodge = true;
            dodgeTimer = 0;
        }

        if (isDashingTimer > 0) 
        {
            isDashingTimer -= Time.deltaTime;
        }
        else
        {
            isDashing = false;
            isDashingTimer = 0;
        }
    }

    void SwingRelease()
    {
        //Stop charging attacks upon button release
        if (!Input.GetButton("Fire1"))
        {
            anim.SetBool("holding", false);
        }
    }

    //------------------------------------------------------------------------------------------------

    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision collision)
    {
        float minDot = GetMinDot(collision.gameObject.layer);
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            if (normal.y >= minDot)
            {
                groundContactCount += 1;
                contactNormal += normal;
                connectedBody = collision.rigidbody;
            }
            else if (normal.y > -0.01f)
            {
                steepContactCount += 1;
                steepNormal += normal;
                if (groundContactCount == 0)
                {
                    connectedBody = collision.rigidbody;
                }
            }
        }
    }

    //----------------------------------------------------------------------------------------------------------

    void ClearGroundCheck()
    {
        groundContactCount = steepContactCount = 0;
        contactNormal = steepNormal = connectionVelocity = Vector3.zero;
        previousConnectedBody = connectedBody;
        connectedBody = null;
    }

    void UpdateGroundCheck()
    {
        stepsSinceLastGrounded += 1;
        stepsSinceLastJump += 1;
        velocity = body.velocity;

        if (OnGround || SnapToGround() || CheckSteepContacts())
        {
            if (State == airborneState)
                State.ToState(this, groundedState);
            stepsSinceLastGrounded = 0;
        }
        else 
        {
            if (State == groundedState)
                State.ToState(this, airborneState);
        }

        //Connected to a moving platform
        if (connectedBody)
        {
            //Check to prevent player from sticking to light bodies
            if (connectedBody.isKinematic || connectedBody.mass >= body.mass)
            {
                UpdateConnectionState();
            }
        }
    }

    void UpdateConnectionState()
    {
        if (connectedBody == previousConnectedBody)
        {
            Vector3 connectionMovement = connectedBody.transform.TransformPoint(connectionLocalPosition) - connectionWorldPosition;
            connectionVelocity = connectionMovement / Time.deltaTime;
        }
        connectionWorldPosition = body.position;
        connectionLocalPosition = connectedBody.transform.InverseTransformPoint(connectionWorldPosition);
    }

    bool SnapToGround()
    {
        if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2)
        {
            return false;
        }
        float speed = velocity.magnitude;
        if (speed > maxSnapSpeed)
        {
            return false;
        }
        if (!Physics.Raycast(
            body.position, Vector3.down, out RaycastHit hit,
            probeDistance, probeMask))
        {
            return false;
        }
        if (hit.normal.y < GetMinDot(hit.collider.gameObject.layer))
        {
            return false;
        }

        groundContactCount = 1;
        contactNormal = hit.normal;
        float dot = Vector3.Dot(velocity, hit.normal);
        if (dot > 0f)
        {
            velocity = (velocity - hit.normal * dot).normalized * speed;
        }
        connectedBody = hit.rigidbody;
        return true;
    }

    bool CheckSteepContacts()
    {
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();
            if (steepNormal.y >= minGroundDotProduct)
            {
                steepContactCount = 0;
                groundContactCount = 1;
                contactNormal = steepNormal;
                return true;
            }
        }
        return false;
    }

    void AdjustVelocity()
    {
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        //This relative velocity allows for seamless movement when on moving platforms
        Vector3 relativeVelocity = velocity - connectionVelocity;
        float currentX = Vector3.Dot(relativeVelocity, xAxis);
        float currentZ = Vector3.Dot(relativeVelocity, zAxis);

        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX =
            Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ =
            Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }

    Vector3 ProjectOnContactPlane(Vector3 vector)
    {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }

    //-----------------------------------------------------------------------------------------------------------------

    public float GetMinDot(int layer)
    {
        return (stairsMask & (1 << layer)) == 0 ?
            minGroundDotProduct : minStairsDotProduct;
    }
}