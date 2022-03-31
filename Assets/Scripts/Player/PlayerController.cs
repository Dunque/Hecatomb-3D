using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerInputSpace = default;

    [Header("State Machine")]
    [SerializeField] public PlayerState state;
    [SerializeField] public PlayerState State { get => state; set => state = value; }
    public GroundedState groundedState;
    public DashingState dashingState;
    public AirborneState airborneState;

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
    public Vector3 velocity, desiredVelocity;
    public Vector3 contactNormal, steepNormal;

    [Header("Jump")]
    [SerializeField, Range(0f, 10f)] public float jumpHeightShort = 3f;   // Velocity for the lowest jump
    [SerializeField, Range(0f, 10f)] public float jumpHeight = 5f;          // Velocity for the highest jump
    [SerializeField] public float coyoteTime = 0.15f;
    [SerializeField] public float coyoteTimer = 0f;
    [SerializeField] public bool canJump;
    public bool jump = false;
    public bool jumpCancel = false;

    [Header("Ground Stuff")]
    [SerializeField, Range(0, 90)] public float maxGroundAngle = 40f, maxStairsAngle = 50f;
    [SerializeField, Range(0f, 100f)] public float maxSnapSpeed = 100f;
    [SerializeField, Min(0f)] public float probeDistance = 1.5f;
    [SerializeField] public LayerMask probeMask = -1, stairsMask = -1;
    public int groundContactCount, steepContactCount;
    [SerializeField] public bool OnGround => groundContactCount > 0;
    public float minGroundDotProduct, minStairsDotProduct;
    public int stepsSinceLastGrounded, stepsSinceLastJump;

    [Header("Dodge")]
    [SerializeField] public float dodgeAmount = 10f;
    [SerializeField] public float dodgeCooldown = 0.5f;
    [SerializeField] public bool isDashing;
    [SerializeField] public bool canDodge;
    [SerializeField] public float dodgeTimer = 0f;
    [SerializeField] public float isDashingTimer = 0f;
    public bool dashed = false;
    [SerializeField] public float airDashAmount = 5f;

    [Header("Combat")]
    [SerializeField] public bool canAttack;
    [SerializeField] public bool canAirAttack;
    [SerializeField] public bool canCombo;
    [SerializeField] public bool canCombo2;
    [SerializeField] public float knockback;
    [SerializeField] public float damage;

    [Header("Aiming")]
    [SerializeField] private Camera m_Camera;
    [SerializeField] private MouseLook mouseLook = new MouseLook();

    [Header("Sound")]
    [SerializeField] public AudioSource audio_PKCharge;
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
        groundedState = new GroundedState(this);
        dashingState = new DashingState(this);
        airborneState = new AirborneState(this);

        State = groundedState;
        State.ToState(this, groundedState);

        body = GetComponent<Rigidbody>();
        renderer = GetComponentInChildren<Renderer>();
        shake = GetComponentInChildren<HeadBob>();
        OnValidate();

        trans = this.transform;
        anim = GetComponentInChildren<Animator>();

        anim.applyRootMotion = true;

        if (!m_Camera)
        {
            m_Camera = Camera.main;
        }
            
        camTrans = m_Camera.transform;
        playerInputSpace = camTrans;
        mouseLook.Init(trans, camTrans);
    }

    public void HandleInput(Input input)
    {
        State.HandleInput(this);
    }

    void Update()
    {
        State.Update(this);
        mouseLook.UpdateCursorLock();

        // Rotate the character and camera.
        mouseLook.LookRotation(trans, camTrans);

        //Getting the move input
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        //Moving
        Vector3 forward = playerInputSpace.forward;
        forward.y = 0f;
        forward.Normalize();
        Vector3 right = playerInputSpace.right;
        right.y = 0f;
        right.Normalize();
        desiredVelocity = (forward * playerInput.y + right * playerInput.x) * maxSpeed;

        //Activating jumps
        //QueueJump();

        if (!canDodge || isDashing)
        {
            DodgeCooldown();
        }

        if (OnGround)
        {
            //Coyote time
            coyoteTimer = coyoteTime;
            canJump = true;

            //Dodge();
        }
        else
        {
            //Start decreasing coyote time
            CoyoteCooldown();
        }

        //Swing blade.
        //SwingBlade();
        SwingRelease();

        //Change color
        renderer.material.SetColor(
        "_BaseColor", OnGround ? Color.black : Color.white);
    }

    void FixedUpdate()
    {
        UpdateState();
        AdjustVelocity();

        State.FixedUpdate(this);

        body.velocity = velocity;

        ClearState();
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

    private void CoyoteCooldown()
    {
        if (coyoteTimer > 0)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else
        {
            canJump = false;
            coyoteTimer = 0;
            //State.ToState(this, airborneState);
        }
    }

    void SwingRelease()
    {
        //Stop charging attacks upon button release
        if (!Input.GetButton("Fire1"))
        {
            anim.SetBool("holdV", false);
            anim.SetBool("2holdV", false);
            anim.SetBool("holdH", false);
            anim.SetBool("holdAir", false);
        }

        //Parry ground & air
        if (Input.GetButtonDown("Fire2"))
        {
            anim.Play("Shoot");
        }
    }

    private void SwingBlade()
    {
        //Stop charging attacks upon button release
        if (!Input.GetButton("Fire1"))
        {
            anim.SetBool("holdV", false);
            anim.SetBool("2holdV", false);
            anim.SetBool("holdH", false);
            anim.SetBool("holdAir", false);
        }

        //Parry ground & air
        if (Input.GetButtonDown("Fire2"))
            {
                anim.Play("Shoot");
            }

        //Dashing attacks
        if (isDashing)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (playerInput.y >= 0f)
                {
                    anim.Play("Stinger");
                    knockback = 5f;
                    damage = 10f;
                }
                else
                {
                    anim.Play("HTime");
                    knockback = 5f;
                    damage = 10f;
                }

            }
        }

        //Aerial attack
        if (!OnGround)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                anim.Play("AirSwing");
                anim.SetBool("holdAir", true);
                knockback = 5f;
                damage = 10f;
            }
        }
        //Grounded 3 hit combo
        else if (!isDashing && OnGround)
        {
            if (canAttack)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    anim.Play("SwingV");
                    anim.SetBool("holdV", true);
                    knockback = 5f;
                    damage = 10f;
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    anim.Play("Parry");
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    anim.Play("Stinger");
                    knockback = 5f;
                    damage = 10f;
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    anim.Play("HTime");
                    knockback = 5f;
                    damage = 10f;
                }
            }

            if (canCombo)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    canCombo = false;
                    anim.applyRootMotion = false;
                    anim.CrossFade("2SwingV", 0.2f);
                    anim.SetBool("2holdV", true);
                    knockback = 6.5f;
                    damage = 12f;
                }
            }

            if (canCombo2)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    canCombo2 = false;
                    anim.applyRootMotion = false;
                    anim.CrossFade("SwingH", 0.2f);
                    anim.SetBool("holdH", true);
                    knockback = 9f;
                    damage = 20f;
                }
            }
        }
    }

    public void SwingGroundCombos()
    {
        if (canAttack)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                anim.Play("SwingV");
                anim.SetBool("holdV", true);
                knockback = 5f;
                damage = 10f;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                anim.Play("Parry");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                anim.Play("Stinger");
                knockback = 5f;
                damage = 10f;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                anim.Play("HTime");
                knockback = 5f;
                damage = 10f;
            }
        }

        if (canCombo)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                canCombo = false;
                anim.applyRootMotion = false;
                anim.CrossFade("2SwingV", 0.2f);
                anim.SetBool("2holdV", true);
                knockback = 6.5f;
                damage = 12f;
            }
        }

        if (canCombo2)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                canCombo2 = false;
                anim.applyRootMotion = false;
                anim.CrossFade("SwingH", 0.2f);
                anim.SetBool("holdH", true);
                knockback = 9f;
                damage = 20f;
            }
        }
    }

    public void SwingAirborne()
    {
        if (canAirAttack)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                anim.Play("AirSwing");
                anim.SetBool("holdAir", true);
                knockback = 5f;
                damage = 10f;
            }
        }
    }

    public void SwingDashing()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (playerInput.y >= 0f)
            {
                anim.Play("Stinger");
                knockback = 5f;
                damage = 10f;
            }
            else
            {
                anim.Play("HTime");
                knockback = 5f;
                damage = 10f;
            }
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
            }
            else if (normal.y > -0.01f)
            {
                steepContactCount += 1;
                steepNormal += normal;
            }
        }
    }

    //----------------------------------------------------------------------------------------------------------

    void ClearState()
    {
        groundContactCount = steepContactCount = 0;
        contactNormal = steepNormal = Vector3.zero;
    }

    void UpdateState()
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
            //contactNormal = Vector3.up;
        }
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

        float currentX = Vector3.Dot(velocity, xAxis);
        float currentZ = Vector3.Dot(velocity, zAxis);

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