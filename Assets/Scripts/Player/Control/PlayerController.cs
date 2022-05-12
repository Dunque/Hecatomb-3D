using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerInputSpace = default;

    [Header("State Machine")]
    public PlayerState state;
    public PlayerState State { get => state; set => state = value; }
    public GroundedState groundedState;
    public AirborneState airborneState;

    [Header("References")]
    public Rigidbody body;
    public Transform trans;
    public Transform camTrans;
    public Animator anim;
    public new Renderer renderer;
    public HeadBob shake;
    public WeaponBob wpnBob;
    public WeaponSway wpnSway;
    public Transform leftHand;
    public Transform rightHand;
    public PlayerStats stats;
    public WeaponManager wpnManager;

    [Header("Movement")]
    [SerializeField, Range(0f, 100f)] public float maxSpeed = 7f;
    [SerializeField, Range(0f, 100f)] public float maxAcceleration = 30f, maxAirAcceleration = 5f;
    public Vector2 playerInput;
    public Vector3 velocity, desiredVelocity, connectionVelocity;
    Vector3 contactNormal, steepNormal;
    Rigidbody connectedBody;
    Rigidbody previousConnectedBody;

    [Header("Jump")]
    [SerializeField, Range(0f, 10f)] public float jumpHeight = 5f;
    public float coyoteTime = 0.15f;
    public float coyoteTimer = 0f;

    [Header("Ground Checks")]
    [SerializeField, Range(0, 90)] public float maxGroundAngle = 45f;
    [SerializeField, Range(0f, 100f)] public float maxSnapSpeed = 100f;
    [SerializeField, Min(0f)] public float probeDistance = 1.5f;
    public LayerMask probeMask = -1, stairsMask = -1;
    int groundContactCount, steepContactCount;
    public bool OnGround => groundContactCount > 0;
    float minGroundDotProduct;
    public int stepsSinceLastGrounded, stepsSinceLastJump;
    Vector3 connectionWorldPosition, connectionLocalPosition;

    [Header("Dodge")]
    public float dodgeAmount = 10f;
    public float dodgeCooldown = 0.5f;
    public float airDashAmount = 5f;
    public bool isDashing;
    public bool canDodge;
    public float dodgeTimer = 0f;
    public float isDashingTimer = 0f;
    public bool dashed = false;

    //Struct used to set up the damage, knockback direction and amount of
    //the basic combo attacks.
    public struct GroundAttackData
    {
        public string[] animName;
        public float[] knockback;
        public Vector3[] knockbackDir;
        public float[] damage;
    };

    [Header("Combat")]
    public GroundAttackData groundAttackData;
    public HitboxStats weaponHitbox;
    public int currentCombo;
    public bool canAirAttack;
    public bool canAttack;

    [Header("Aiming")]
    public Camera m_Camera;
    public MouseLook mouseLook = new MouseLook();
    public Vector3 playerForward;
    public Vector3 playerRight;

    [Header("Sound")]
    public AudioSource playerAudioSource;
    public AudioClip[] footstepClips;
    public AudioClip landingClip;
    public AudioClip[] lightSwingClips;
    public AudioClip[] heavySwingClips;
    public AudioClip[] thrustClips;
    public AudioClip[] dashClips;

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

        //Setting up the components, in case we don't want to add them manually through the inspector
        body = GetComponent<Rigidbody>();
        renderer = GetComponentInChildren<Renderer>();
        shake = GetComponentInChildren<HeadBob>();
        wpnBob = GetComponentInChildren<WeaponBob>();
        wpnSway = GetComponentInChildren<WeaponSway>();
        anim = GetComponentInChildren<Animator>();
        wpnManager = GetComponent<WeaponManager>();
        playerAudioSource = GetComponent<AudioSource>();
        stats = GetComponent<PlayerStats>();

        trans = transform;

        if (!m_Camera)
        {
            m_Camera = Camera.main;
        }
            
        camTrans = m_Camera.transform;
        playerInputSpace = camTrans;
        mouseLook.Init(trans, camTrans);

        //Establish the minGroundDotProduct for later ground collision detections
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    //This function is in charge of handling the input of the player
    public void HandleInput()
    {
        //If the player is dead, there is no need to handle the input
        if (!stats.isDead)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                State.HandleInput(this);

            //Getting the move input
            playerInput.x = Input.GetAxis("Horizontal");
            playerInput.y = Input.GetAxis("Vertical");
            playerInput = Vector2.ClampMagnitude(playerInput, 1f);

            // Rotate the character and camera.
            mouseLook.LookRotation(trans, camTrans);

            //Open the weaponWheel
            if (Input.GetButton("WeaponSelect"))
                wpnManager.OpenWeaponWheel();
            else
                wpnManager.CloseWeaponWheel();

            //Quick select weapon by pressin the numbers 1-3
            if (Input.GetKeyDown(KeyCode.Alpha1))
                wpnManager.ChangeGun(1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                wpnManager.ChangeGun(2);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                wpnManager.ChangeGun(3);

            // Pause screen by pressin 'Esc' key
            if (Input.GetButtonDown("Cancel"))
            {
                Debug.Log("Opening pause menu...");
                Director.OpenPauseMenu();
            }



            // Shortcut to kill player (mainly for debugging)
            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("Killing player...");
                stats.ReceiveDamage(1000);
            }

            // Shortcuts to go to next scene (mainly for debugging)
            if (Input.GetKeyDown(KeyCode.N))
            {
                Debug.Log("Going to next scene...");
                Director.NextScene();
            }
            
        }
    }

    void Update()
    {
        HandleInput();

        State.Update(this);

        mouseLook.UpdateCursorLock();

        //Moving
        playerForward = playerInputSpace.forward;
        playerForward.y = 0f;
        playerForward.Normalize();
        playerRight = playerInputSpace.right;
        playerRight.y = 0f;
        playerRight.Normalize();
        desiredVelocity = (playerForward * playerInput.y + playerRight * playerInput.x) * maxSpeed;

        //Update the forward vector of the player in the hitbox, to properly push back enemies when hit
        groundAttackData.knockbackDir = new Vector3[] { playerForward, playerForward, playerForward };

        //The dodging timer is activated independently if the player is grounded or in the air,
        //even thought the player may only dash in the ground.
        if (!canDodge || isDashing)
        {
            DodgeCooldown();
        }

        //Button release.
        SwingRelease();
    }

    //In the fixed update we perform the ground checks and we apply the players movement, due to it
    //being based on forces and a rigidbody.
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
        //Dodging timer, that controls when the player can input the next dodge
        if (dodgeTimer > 0)
        {
            dodgeTimer -= Time.deltaTime;
        }
        else
        {
            canDodge = true;
            dodgeTimer = 0;
        }

        //Dashing timer, that controls the time the dash boost is applied to the player
        if (isDashingTimer > 0) 
        {
            //Player is invulnerable while dashing, to evade damage
            StartCoroutine(stats.IframesTimer(0.5f));
            isDashingTimer -= Time.deltaTime;
        }
        else
        {
            isDashing = false;
            isDashingTimer = 0;
        }
    }

    //the coyote time is used to transition from grounded state to air state a bit later, in order
    //to let the player jump when they are falling a bit from a platform, without noticing it
    private void CoyoteCooldown()
    {
        if (coyoteTimer > 0)
            coyoteTimer -= Time.deltaTime;
        else
            coyoteTimer = 0;
    }

    //This function is in charge of releasing the charged attacks of the player, and it's run independently of
    //the player state
    void SwingRelease()
    {
        //Stop charging attacks upon button release
        if (!Input.GetButton("Attack1"))
        {
            anim.SetBool("holding", false);
        }
    }

    //------------------------------------------------------------------------------------------------

    //This function is called whenever the attached rigidbody colides with a collider
    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    //This function is called whenever the attached rigidbody is already colliding with a collider
    void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    //Function responsible to check if the player is grounded or not, also it sets up the connectedBody rigigbody,
    //used to keep the player attached to moving platforms
    void EvaluateCollision(Collision collision)
    {
        //We check each collision
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            //We compare the normal vector of the contact collision with the one we calculated
            //as a maximum for the player
            if (normal.y >= minGroundDotProduct)
            {
                //We count it as being touching ground
                groundContactCount += 1;
                contactNormal += normal;
                connectedBody = collision.rigidbody;
            }
            //This is the case of a steep contact, but we use -0.01 to give a bit of leniency
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

    //this function is in charge of changing the state of the player based on being on the ground or airborne.
    //To transition to airborne state, first the function starts the Coyote time and when it finishes, it changes
    //the player's state. This allows to jump slightly off platform, to provide a wider timeframe for the jumps.
    void UpdateGroundCheck()
    {
        stepsSinceLastGrounded += 1;
        stepsSinceLastJump += 1;
        velocity = body.velocity;

        //Grounded, transition to ground state
        if (OnGround || SnapToGround() || CheckSteepContacts())
        {
            coyoteTimer = coyoteTime;
            if (State == airborneState)
                State.ToState(this, groundedState);
            stepsSinceLastGrounded = 0;
        }
        //Airborne, call the coyote cooldown and then transition to airborne state
        else
        {
            CoyoteCooldown();
            if (State == groundedState && coyoteTimer == 0)
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

    //This function is used to properly get a reference to the rigidbody that the player is mounting (for example, a moving platform)
    //in order to then apply the movement of said rigidbody to the player.
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

    //This function checks if the player should be brought down to the ground. It's main use is to mitigate
    //the effect of the player going off a slope when they realisticaly should be sticking to the ground.
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
        if (hit.normal.y < minGroundDotProduct)
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

    //This function is in charge of checking if the player is stuck in a small crevase on the ground, 
    //and we use it to be like another grounded check. If we are stuck in a crevase, we count it as being
    //grounded. Otherwise we would have the character stuck there.
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

    //This function is in charge of changing the speed of the player accodring to the slope they are
    //standing in
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

    //This auxiliary function projects a vector in the contact normal plane
    Vector3 ProjectOnContactPlane(Vector3 vector)
    {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }

    //-----------------------------------------------------------------------------------------------------------------
}