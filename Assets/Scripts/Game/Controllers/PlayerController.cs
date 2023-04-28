using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerController : GravityObject
{

    // Exposed variables
    [Header("Movement settings")] public float walkSpeed = 8;
    public float runSpeed = 14;
    public float jumpForce = 20;
    public float vSmoothTime = 0.1f;
    public float airSmoothTime = 0.5f;
    public float stickToGroundForce = 8;
    public float upVelocity = 0;
    public float downVelocity = 0;
    public float maxVelocity = 10;

    [Header("Energy settings")]
    public float energyDrainRate = -0.005f;
    public float energyRechargeRate = 0.05f;
    public float maxEnergy = 100;
    public float minEnergy = 0;
    public float energyRechargeDelay = 1;
    public float energyRechargeDelayTimer = 0;
    public float energyRechargeDelayTimerMax = 1;
    public float energyRechargeDelayTimerMin = 0;
    public float energyRechargeDelayTimerReset = 0;

    [Header("Energy System")]
    public GameObject sun;
    public float energy;
    private Vignette _vignette;


    public bool isFlying = false;
    public bool isWalking = false;

    public AtmosphereTrigger atmosphereTrigger;
    public CelestialBody celestialBody;

    public float flyForce = .01f;

    public bool isWithinPlanetRange = true;

    [Header("Mouse settings")] public float mouseSensitivityMultiplier = 1;
    public float maxMouseSmoothTime = 0.3f;
    public Vector2 pitchMinMax = new(-40, 85);
    public InputSettings inputSettings;

    [Header("Other")] public float mass = 70;
    public LayerMask walkableMask;
    public Transform feet;

    [Header("Crystal Collider")]
    public CrystalCollider crystalCollider;
    private bool hasCrystal = false;

    // [Header("Puzzle 1 Settings")]
    // public bool isInPuzzleMode = false;
    // public Shape shape; 
    // public Camera puzzle1Camera;


    // Private
    Rigidbody rb;

    Ship spaceship;

    float yaw;
    float pitch;
    float smoothYaw;
    float smoothPitch;

    float yawSmoothV;
    float pitchSmoothV;

    Vector3 targetVelocity;
    Vector3 cameraLocalPos;
    Vector3 smoothVelocity;
    Vector3 smoothVRef;

    CelestialBody referenceBody;

    Camera cam;
    bool readyToFlyShip;
    bool debug_playerFrozen;
    Animator animator;

    //declare GameControl script
    public GameController gameController;

    void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        cameraLocalPos = cam.transform.localPosition;
        spaceship = FindObjectOfType<Ship>();
        InitRigidbody();

        animator = GetComponentInChildren<Animator>();
        inputSettings.Begin();
    }

    void InitRigidbody()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.mass = mass;
    }
    void HandleMovement()
    {
        HandleEditorInput();
        if (Time.timeScale == 0)
        {
            return;
        }

        // Look input
        yaw += Input.GetAxisRaw("Mouse X") * inputSettings.mouseSensitivity / 10 * mouseSensitivityMultiplier;
        pitch -= Input.GetAxisRaw("Mouse Y") * inputSettings.mouseSensitivity / 10 * mouseSensitivityMultiplier;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        float mouseSmoothTime = Mathf.Lerp(0.01f, maxMouseSmoothTime, inputSettings.mouseSmoothing);
        smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchSmoothV, mouseSmoothTime);
        float smoothYawOld = smoothYaw;
        smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawSmoothV, mouseSmoothTime);

        if (!debug_playerFrozen && Time.timeScale > 0)
        {
            cam.transform.localEulerAngles = Vector3.right * smoothPitch;
            transform.Rotate(Vector3.up * Mathf.DeltaAngle(smoothYawOld, smoothYaw), Space.Self);
        }

        // Movement
        bool isGrounded = IsGrounded();
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        bool running = Input.GetKey(KeyCode.LeftShift);
        targetVelocity = transform.TransformDirection(input.normalized) * ((running) ? runSpeed : walkSpeed);
        smoothVelocity = Vector3.SmoothDamp(smoothVelocity, targetVelocity, ref smoothVRef, (isGrounded) ? vSmoothTime : airSmoothTime);


        // Flying mode
        if (Input.GetKey(KeyCode.Space) && energy > 0)
        {
            energy -= 0.1f;
            rb.AddForce(transform.up * flyForce * 0.07f, ForceMode.VelocityChange);
            Debug.Log("Flying");
            isFlying = true;
        }
        else
        {
            // Apply small downward force to prevent player from bouncing when going down slopes
            rb.AddForce(-transform.up * stickToGroundForce * 0.2f, ForceMode.VelocityChange);
            isFlying = false;
        }
    }

    // // Add this method inside the class
    // public void SwitchPuzzleMode()
    // {
    //     isInPuzzleMode = !isInPuzzleMode;

    //     // Enable/Disable the appropriate UI and controls
    //     // You'll need to implement these methods in the UIController script
    //     if (isInPuzzleMode)
    //     {

    //     // Show the 2D puzzle UI
    //     // UIController.ShowPuzzleUI();

    //     // Enable Puzzle1Camera and disable the main player camera
    //     puzzle1Camera.enabled = true;
    //     cam.enabled = false;
    //     }
    //     else
    //     {
    //     // Hide the 2D puzzle UI
    //     // UIController.HidePuzzleUI();

    //     // Disable Puzzle1Camera and enable the main player camera
    //     puzzle1Camera.enabled = false;
    //     cam.enabled = true;
    //     }
    // }


    private void UpdateHasCrystal(bool value)
    {
        hasCrystal = value;
    }

    private void Start()
    {
        if (_vignette){
        var volume = GetComponentInChildren<PostProcessVolume>();
        }
        // volume.profile.TryGetSettings(out _vignette);
       if (crystalCollider){
        crystalCollider.onCrystalCollision.AddListener(UpdateHasCrystal);
       }
    }


    void Update()
    {
        if (isFlying && Input.GetAxisRaw("Vertical") != 0 || isFlying && Input.GetAxisRaw("Horizontal") != 0)
        {
            animator.SetBool("isFlying", true);
            animator.SetBool("isWalking", false);
        } else if (!isFlying && Input.GetAxisRaw("Vertical") != 0 || !isFlying && Input.GetAxisRaw("Horizontal") != 0)
        {
            animator.SetBool("isFlying", false);
            animator.SetBool("isWalking", true);
        } else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isFlying", false);
        }



        //if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        //{
        //    Debug.Log("walking!");
        //    //animator.Play("walking");
        //    //animator.SetTrigger("isWalking");
        //    if (isFlying)
        //    {
        //        animator.SetBool("isFlying", true);
        //    }
        //    else
        //    {
        //        animator.SetBool("isWalking", true);
        //    }
        //}
        //else
        //{
        //    animator.SetBool("isWalking", false);
        //    animator.SetBool("isFlying", false);
        //}
        if(gameController){
        if (!gameController.gameActive)
        {
            HandleMovement();
        }
        else
        {

        }
        }
        
        if (sun){


        UpdateEnergy();
        }
         


    }



    bool IsGrounded()
    {
        // Sphere must not overlay terrain at origin otherwise no collision will be detected
        // so rayRadius should not be larger than controller's capsule collider radius
        const float rayRadius = .3f;
        const float groundedRayDst = .2f;
        bool grounded = false;

        if (referenceBody)
        {
            var relativeVelocity = rb.velocity - referenceBody.velocity;
            // Don't cast ray down if player is jumping up from surface
            if (relativeVelocity.y <= jumpForce * .5f)
            {
                RaycastHit hit;
                Vector3 offsetToFeet = (feet.position - transform.position);
                Vector3 rayOrigin = rb.position + offsetToFeet + transform.up * rayRadius;
                Vector3 rayDir = -transform.up;

                grounded = Physics.SphereCast(rayOrigin, rayRadius, rayDir, out hit, groundedRayDst, walkableMask);
            }
        }

        return grounded;
    }

    void UpdateEnergy()
    {
        RaycastHit hit;
        Vector3 directionToSun = (sun.transform.position - transform.position).normalized;
        float angleToSun = Vector3.Angle(directionToSun, transform.up);

        if (Physics.Linecast(transform.position, sun.transform.position, out hit))
        {
            if (hit.collider.name != "Terrain Mesh" && angleToSun < 100)
            {
                energy += isFlying ? energyDrainRate : energyRechargeRate; // Adjust these values to modify energy consumption and replenishment rates
            }
            else
            {
                energy -= 0.05f;
            }
            energy = Mathf.Clamp(energy, minEnergy, maxEnergy);

            if (_vignette != null)
            {
                _vignette.intensity.value = Math.Min(energy / maxEnergy, 0.68f);
            }
        }
    }
    private void CalculateUpDownVelocity()
    {
        // Calculate the projection of the player's velocity onto the up direction
        Vector3 upProjection = Vector3.Dot(rb.velocity, transform.up) * transform.up;

        // If the projection has a positive y-component, it's an upward velocity
        if (upProjection.y > 0)
        {
            upVelocity = upProjection.magnitude;
            downVelocity = 0;
        }
        // If the projection has a negative y-component, it's a downward velocity
        else if (upProjection.y < 0)
        {
            upVelocity = 0;
            downVelocity = -upProjection.magnitude;
        }
        // If the projection has a y-component equal to 0, there's no up or down velocity
        else
        {
            upVelocity = 0;
            downVelocity = 0;
        }
    }




    void FixedUpdate()
    {
        CelestialBody[] bodies = NBodySimulation.Bodies;
        Vector3 gravityOfNearestBody = Vector3.zero;
        float nearestSurfaceDst = float.MaxValue;


        // Gravity
        foreach (CelestialBody body in bodies)
        {
            float sqrDst = (body.Position - rb.position).sqrMagnitude;
            Vector3 forceDir = (body.Position - rb.position).normalized;
            Vector3 acceleration = forceDir * Universe.gravitationalConstant * body.mass / sqrDst;
            rb.AddForce(acceleration, ForceMode.Acceleration);

            float dstToSurface = Mathf.Sqrt(sqrDst) - body.radius;

            // Find body with strongest gravitational pull 
            if (dstToSurface < nearestSurfaceDst)
            {
                nearestSurfaceDst = dstToSurface;
                gravityOfNearestBody = acceleration;
                referenceBody = body;
            }
        }

        // Rotate to align with gravity up
        Vector3 gravityUp = -gravityOfNearestBody.normalized;
        rb.rotation = Quaternion.FromToRotation(transform.up, gravityUp) * rb.rotation;

        // Move
        rb.MovePosition(rb.position + smoothVelocity * Time.fixedDeltaTime);

        //CalculateUpDownVelocity();
        CalculateUpDownVelocity();
    }

    void HandleEditorInput()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log("Debug mode: Toggle freeze player");
                debug_playerFrozen = !debug_playerFrozen;
            }
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    public void ExitFromSpaceship()
    {
        cam.transform.parent = transform;
        cam.transform.localPosition = cameraLocalPos;
        smoothYaw = 0;
        yaw = 0;
        smoothPitch = cam.transform.localEulerAngles.x;
        pitch = smoothPitch;
    }

    public Camera Camera
    {
        get { return cam; }
    }

    public Rigidbody Rigidbody
    {
        get { return rb; }
    }


}