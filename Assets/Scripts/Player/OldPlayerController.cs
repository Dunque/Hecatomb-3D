using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class OldPlayerController : MonoBehaviour
{

    [Header("References")]
    public Animator anim;

    [System.Serializable]
    public class MovementSettings
    {
        public float MaxSpeedWalking;
        public float MaxSpeedRunning;
        public float Acceleration;
        public float Deceleration;

        public MovementSettings(float maxSpeedWalking, float maxSpeedRunning, float accel, float decel)
        {
            MaxSpeedWalking = maxSpeedWalking;
            MaxSpeedRunning = maxSpeedRunning;
            Acceleration = accel;
            Deceleration = decel;
        }
    }

    [Header("Aiming")]
    [SerializeField] private Camera m_Camera;
    //[SerializeField] private CameraShake shake;
    //[SerializeField] private WeaponBob wpnBob;
    [SerializeField] private MouseLook m_MouseLook = new MouseLook();

    [Header("Movement")]
    [SerializeField] public bool isRunning;
    [SerializeField] private float m_Friction = 6;
    [SerializeField] private float m_Gravity = 20;
    [SerializeField] public float rayLength = 1.1f;
    [SerializeField] public bool isGrounded;

    [Tooltip("How precise air control is")]
    [SerializeField] private float m_AirControl = 0.3f;
    [SerializeField] public MovementSettings m_GroundSettings = new MovementSettings(7, 12, 14, 10);
    [SerializeField] public MovementSettings m_AirSettings = new MovementSettings(7, 12, 2, 2);
    [SerializeField] public MovementSettings m_StrafeSettings = new MovementSettings(1, 1.71f, 50, 50);

    [Header("Combat")]
    [SerializeField] public bool canAttack;

    [Header("Dodge")]
    [SerializeField] private float maxTime = 0.2f;
    [SerializeField] private float dodgeAmount = 20f;
    [SerializeField] public float dodgeCooldown = 90f;
    [SerializeField] public bool canDodge;
    [SerializeField] public float dodgeTimer = 0f;
    private float lastTimeClickedW;
    private float lastTimeClickedA;
    private float lastTimeClickedS;
    private float lastTimeClickedD;
    

    [Header("Jump")]
    [SerializeField] public float jumpShortSpeed = 3f;   // Velocity for the lowest jump
    [SerializeField] public float jumpSpeed = 6f;          // Velocity for the highest jump
    [SerializeField] public float coyoteTime = 0.2f;
    [SerializeField] public float coyoteTimer = 0f;
    [SerializeField] bool canJump;
    bool jump = false;
    bool jumpCancel = false;

    /// <summary>
    /// Returns player's current speed.
    /// </summary>
    public float Speed { get { return m_Character.velocity.magnitude; } }

    public CharacterController m_Character;
    public Vector3 m_MoveDirectionNorm = Vector3.zero;
    public Vector3 m_PlayerVelocity = Vector3.zero;

    // Used to display real time friction values.
    [SerializeField] private float m_PlayerFriction = 0;

    private Vector3 m_MoveInput;
    private Transform m_Tran;
    private Transform m_CamTran;

    private void CheckGround()
    {
        if (jump)
        {
            isGrounded = m_Character.isGrounded;
        }
        else
        {
            Debug.DrawRay(m_Tran.position, Vector3.down * rayLength, Color.yellow);
            isGrounded = (Physics.Raycast(m_Tran.position, Vector3.down, rayLength, -1) || m_Character.isGrounded);
        }
    }

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
        }
    }

    private void Dodge()
    {
        var wishdir = new Vector3(m_MoveInput.x, 0, m_MoveInput.z);
        wishdir = m_Tran.TransformDirection(wishdir);
        wishdir.Normalize();

        if (canDodge)
        {
            if (Input.GetKeyDown("w"))
            {
                float deltaTimeW = Time.time - lastTimeClickedW;

                if (deltaTimeW < maxTime)
                {
                    Dash(wishdir, dodgeAmount, m_GroundSettings.Acceleration);
                }
                lastTimeClickedW = Time.time;
            }
            else if (Input.GetKeyDown("a"))
            {
                float deltaTimeA = Time.time - lastTimeClickedA;
                if (deltaTimeA < maxTime)
                {
                    Dash(wishdir, dodgeAmount, m_GroundSettings.Acceleration);
                }
                lastTimeClickedA = Time.time;
            }
            else if (Input.GetKeyDown("s"))
            {
                float deltaTimeS = Time.time - lastTimeClickedS;
                if (deltaTimeS < maxTime)
                {
                    Dash(wishdir, dodgeAmount, m_GroundSettings.Acceleration);
                }
                lastTimeClickedS = Time.time;
            }
            else if (Input.GetKeyDown("d"))
            {
                float deltaTimeD = Time.time - lastTimeClickedD;
                if (deltaTimeD < maxTime)
                {
                    Dash(wishdir, dodgeAmount, m_GroundSettings.Acceleration);
                }
                lastTimeClickedD = Time.time;
            }
        }
    }

    private void Start()
    {
        m_Tran = transform;
        m_Character = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        anim.applyRootMotion = true;

        if (!m_Camera)
            m_Camera = Camera.main;

        m_CamTran = m_Camera.transform;
        m_MouseLook.Init(m_Tran, m_CamTran);
    }

    private void SwingBlade()
    {
        if (canAttack)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                anim.applyRootMotion = false;
                anim.Play("SwingV");
                //shake.SetShakeDuration(1f);
                anim.SetBool("holdV", true);
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                anim.applyRootMotion = false;
                anim.Play("Parry");
                anim.SetBool("holdP", true);
            }
            else if (Input.GetButtonDown("Fire3"))
            {
                anim.applyRootMotion = false;
                anim.Play("Thrust");
                anim.SetBool("holdT", true);
            }
            else if (Input.GetButtonDown("Fire5"))
            {
                anim.applyRootMotion = false;
                anim.Play("SwingH");
                anim.SetBool("holdH", true);
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            anim.SetBool("holdV", false);
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            anim.applyRootMotion = true;
            anim.SetBool("holdP", false);
        }
        else if (Input.GetButtonUp("Fire3"))
        {
            anim.SetBool("holdT", false);
        }
        else if (Input.GetButtonUp("Fire5"))
        {
            anim.SetBool("holdH", false);
        }
    }

    private void Jump()
    {
        if (jump)
        {
            m_PlayerVelocity.y = jumpSpeed;
            canJump = false;
            jump = false;
        }
        // Cancel the jump when the button is no longer pressed
        if (jumpCancel)
        {
            if (m_PlayerVelocity.y > jumpShortSpeed)
                m_PlayerVelocity.y = jumpShortSpeed;
            jumpCancel = false;
        }
    }

    private void Update()
    {
        CheckGround();
        m_MoveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        m_MouseLook.UpdateCursorLock();
        QueueJump();

        if (!canDodge)
        {
            DodgeCooldown();
        }

        // Set movement state.
        if (isGrounded)
        {
            GroundMove();

            //Coyote time
            coyoteTimer = coyoteTime;
            canJump = true;

            //Dodge
            Dodge();
        }
        else
        {
            //Start decreasing coyote time
            CoyoteCooldown();

            //Apply air movement physics
            AirMove();
        }

        Jump();

        // Rotate the character and camera.
        m_MouseLook.LookRotation(m_Tran, m_CamTran);

        // Move the character.
        m_Character.Move(m_PlayerVelocity * Time.deltaTime);

        //Swing blade.
        SwingBlade();

        //Change color
        GetComponentInChildren<Renderer>().material.SetColor(
        "_Color", isGrounded ? Color.black : Color.white
);
    }

    // Queues the next jump.
    private void QueueJump()
    {
        if (Input.GetButtonDown("Jump") && (canJump))   // Player starts pressing the button
            jump = true;
        if (Input.GetButtonUp("Jump") && !(isGrounded))     // Player stops pressing the button
            jumpCancel = true;
    }

    // Handle air movement.
    private void AirMove()
    {
        float accel;

        var wishdir = new Vector3(m_MoveInput.x, 0, m_MoveInput.z);
        wishdir = m_Tran.TransformDirection(wishdir);

        float wishspeed = wishdir.magnitude;
        wishspeed *= m_AirSettings.MaxSpeedWalking;

        wishdir.Normalize();
        m_MoveDirectionNorm = wishdir;

        // CPM Air control.
        float wishspeed2 = wishspeed;
        if (Vector3.Dot(m_PlayerVelocity, wishdir) < 0)
        {
            accel = m_AirSettings.Deceleration;
        }
        else
        {
            accel = m_AirSettings.Acceleration;
        }

        // If the player is ONLY strafing left or right
        if (m_MoveInput.z == 0 && m_MoveInput.x != 0)
        {
            if (wishspeed > m_StrafeSettings.MaxSpeedWalking)
            {
                wishspeed = m_StrafeSettings.MaxSpeedWalking;
            }

            accel = m_StrafeSettings.Acceleration;
        }

        Accelerate(wishdir, wishspeed, accel);
        if (m_AirControl > 0)
        {
            AirControl(wishdir, wishspeed2);
        }

        // Apply gravity
        m_PlayerVelocity.y -= m_Gravity * Time.deltaTime;
    }

    // Air control occurs when the player is in the air, it allows players to move side 
    // to side much faster rather than being 'sluggish' when it comes to cornering.
    private void AirControl(Vector3 targetDir, float targetSpeed)
    {
        // Only control air movement when moving forward or backward.
        if (Mathf.Abs(m_MoveInput.z) < 0.001 || Mathf.Abs(targetSpeed) < 0.001)
        {
            return;
        }

        float zSpeed = m_PlayerVelocity.y;
        m_PlayerVelocity.y = 0;
        /* Next two lines are equivalent to idTech's VectorNormalize() */
        float speed = m_PlayerVelocity.magnitude;
        m_PlayerVelocity.Normalize();

        float dot = Vector3.Dot(m_PlayerVelocity, targetDir);
        float k = 32;
        k *= m_AirControl * dot * dot * Time.deltaTime;

        // Change direction while slowing down.
        if (dot > 0)
        {
            m_PlayerVelocity.x *= speed + targetDir.x * k;
            m_PlayerVelocity.y *= speed + targetDir.y * k;
            m_PlayerVelocity.z *= speed + targetDir.z * k;

            m_PlayerVelocity.Normalize();
            m_MoveDirectionNorm = m_PlayerVelocity;
        }

        m_PlayerVelocity.x *= speed;
        m_PlayerVelocity.y = zSpeed; // Note this line
        m_PlayerVelocity.z *= speed;
    }

    // Handle ground movement.
    private void GroundMove()
    {
        ApplyFriction(1.0f);

        var wishdir = new Vector3(m_MoveInput.x, 0, m_MoveInput.z);
        wishdir = m_Tran.TransformDirection(wishdir);
        wishdir.Normalize();
        m_MoveDirectionNorm = wishdir;

        var wishspeed = wishdir.magnitude;

        //Sprinting or not
        if (Input.GetButton("Fire4"))
        {
            wishspeed *= m_GroundSettings.MaxSpeedRunning;
            isRunning = true;
        }
        else
        {
            wishspeed *= m_GroundSettings.MaxSpeedWalking;
            isRunning = false;
        }

        Accelerate(wishdir, wishspeed, m_GroundSettings.Acceleration);

        // Reset the gravity velocity
        m_PlayerVelocity.y = -m_Gravity * Time.deltaTime;

    }

    private void ApplyFriction(float t)
    {
        // Equivalent to VectorCopy();
        Vector3 vec = m_PlayerVelocity;
        vec.y = 0;
        float speed = vec.magnitude;
        float drop = 0;

        // Only apply friction when grounded.
        if (isGrounded)
        {
            float control = speed < m_GroundSettings.Deceleration ? m_GroundSettings.Deceleration : speed;
            drop = control * m_Friction * Time.deltaTime * t;
        }

        float newSpeed = speed - drop;
        m_PlayerFriction = newSpeed;
        if (newSpeed < 0)
        {
            newSpeed = 0;
        }

        if (speed > 0)
        {
            newSpeed /= speed;
        }

        m_PlayerVelocity.x *= newSpeed;
        // playerVelocity.y *= newSpeed;
        m_PlayerVelocity.z *= newSpeed;
    }

    // Calculates acceleration based on desired speed and direction.
    private void Accelerate(Vector3 targetDir, float targetSpeed, float accel)
    {
        float currentspeed = Vector3.Dot(m_PlayerVelocity, targetDir);
        float addspeed = targetSpeed - currentspeed;
        if (addspeed <= 0)
        {
            return;
        }

        float accelspeed = accel * Time.deltaTime * targetSpeed;
        if (accelspeed > addspeed)
        {
            accelspeed = addspeed;
        }

        m_PlayerVelocity.x += accelspeed * targetDir.x;
        m_PlayerVelocity.z += accelspeed * targetDir.z;
    }

    // Calculates acceleration based on desired speed and direction.
    private void Dash(Vector3 targetDir, float targetSpeed, float accel)
    {
        m_PlayerVelocity.x += targetSpeed * targetDir.x;
        m_PlayerVelocity.z += targetSpeed * targetDir.z;
        canDodge = false;
        dodgeTimer = dodgeCooldown;
    }
}