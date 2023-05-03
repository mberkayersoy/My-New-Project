// CHANGE LOG
// 
// CHANGES || version VERSION
//
// "Enable/Disable Headbob, Changed look rotations - should result in reduced camera jitters" || version 1.0.1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NewController : MonoBehaviourPunCallbacks
{
    public float MaxFallSpeed;
    public float _airSpeed;
    PlayerManage playerManage;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;
    private float _speed;
    public float SpeedChangeRate = 5.0f;
    private Rigidbody rb;
    PhotonView pw;
    PlayerAttribute playerAttriube;

    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    private Animator animator;
    private bool hasAnimator;
    private float animationBlend;

    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    // timeout deltatime
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    private float verticalVelocity;
    private float terminalVelocity = 53.0f;
    #region Camera Movement Variables

    public Camera playerCamera;

    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    // Internal Variables
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    #region Camera Zoom Variables

    public bool enableZoom = true;
    public bool holdToZoom = false;
    public KeyCode zoomKey = KeyCode.Mouse1;
    public float zoomFOV = 30f;
    public float zoomStepTime = 5f;

    // Internal Variables
    private bool isZoomed = false;

    #endregion
    #endregion

    #region Movement Variables

    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;
    // Internal Variables
    private bool isWalking = false;

    #region Sprint

    public bool enableSprint = true;
    public bool unlimitedSprint = false;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintSpeed = 7f;
    public float sprintDuration = 5f;
    public float sprintCooldown = .5f;
    public float sprintFOV = 80f;
    public float sprintFOVStepTime = 10f;

    // Internal Variables
    private bool isSprinting = false;

    #endregion

    #region Jump

    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;

    #endregion

    #region Crouch

    public bool enableCrouch = true;
    public bool holdToCrouch = true;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float crouchHeight = .75f;
    public float speedReduction = .5f;

    // Internal Variables
    private bool isCrouched = false;
    private Vector3 originalScale;

    #endregion
    #endregion


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pw = GetComponent<PhotonView>();
        playerAttriube = GetComponent<PlayerAttribute>();
        hasAnimator = TryGetComponent(out animator);
        AssignAnimationIDs();
        playerManage = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<PlayerManage>();
        if (!pw.IsMine)
        {
            playerCamera.enabled = false;
        }
        // Set internal variables
        playerCamera.fieldOfView = fov;
        originalScale = transform.localScale;

        // reset our timeouts on start
        jumpTimeoutDelta = JumpTimeout;
        fallTimeoutDelta = FallTimeout;

    }
    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
        if (hasAnimator)
        {
            animator.SetBool(_animIDGrounded, Grounded);
        }
    }

    private void Update()
    {
        // Prevent control is connected to Photon and represent the localPlayer
        if (pw.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        if (pw.IsMine)
        {
            GroundedCheck();
            if (!playerAttriube.isDead)
            {
                // Control camera movement
                if (cameraCanMove)
                {
                    yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

                    if (!invertCamera)
                    {
                        pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
                    }
                    else
                    {
                        // Inverted Y
                        pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
                    }

                    // Clamp pitch between lookAngle
                    pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

                    transform.localEulerAngles = new Vector3(0, yaw, 0);
                    playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
                }

                #region Camera Zoom

                if (enableZoom)
                {
                    // Changes isZoomed when key is pressed
                    // Behavior for toogle zoom
                    if (Input.GetKeyDown(zoomKey) && !holdToZoom && !isSprinting)
                    {
                        if (!isZoomed)
                        {
                            isZoomed = true;
                        }
                        else
                        {
                            isZoomed = false;
                        }
                    }

                    // Changes isZoomed when key is pressed
                    // Behavior for hold to zoom
                    if (holdToZoom && !isSprinting)
                    {
                        if (Input.GetKeyDown(zoomKey))
                        {
                            isZoomed = true;
                        }
                        else if (Input.GetKeyUp(zoomKey))
                        {
                            isZoomed = false;
                        }
                    }

                    // Lerps camera.fieldOfView to allow for a smooth transistion
                    if (isZoomed)
                    {
                        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
                    }
                    else if (!isZoomed && !isSprinting)
                    {
                        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, zoomStepTime * Time.deltaTime);
                    }
                }

                #endregion

                #region Sprint

                if (enableSprint)
                {
                    if (isSprinting)
                    {
                        isZoomed = false;
                        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, sprintFOVStepTime * Time.deltaTime);
                    }
                }

                #endregion

            }
            else
            {
            ResetAnimator();
            }
        }

    }

    void FixedUpdate()
    {           // Prevent control is connected to Photon and represent the localPlayer
        if (pw.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        if (pw.IsMine)
        {
            if (!playerAttriube.isDead)
            {
                Jump();
                Move();
            }
        }
    }

    private void JumpAndGravity()
    {
        if (Grounded)
        {
            // reset the fall timeout timer
            fallTimeoutDelta = FallTimeout;

            // update animator if using character
            if (hasAnimator)
            {
                animator.SetBool(_animIDJump, false);
                animator.SetBool(_animIDFreeFall, false);
            }

            // stop our velocity dropping infinitely when grounded
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            // Jump
            if (Input.GetButtonDown("Jump") && jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                verticalVelocity = Mathf.Sqrt(playerAttriube.jumpHeight * -2f * Gravity);

                // update animator if using character
                if (hasAnimator)
                {
                    animator.SetBool(_animIDJump, true);
                }

                // Set the character's vertical velocity to the calculated jump velocity
                //rb.velocity = new Vector3(rb.velocity.x, verticalVelocity, rb.velocity.z);
            }

            // jump timeout
            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                //update animator if using character
                if (hasAnimator)
                {
                    animator.SetBool(_animIDFreeFall, true);
                }
            }
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += Gravity * Time.deltaTime;
        }
    }
    private void Jump()
    {
        fallTimeoutDelta = FallTimeout;
        if (Grounded)
        {
            if (hasAnimator)
            {
                animator.SetBool(_animIDJump, false);
                animator.SetBool(_animIDFreeFall, false);
            }
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * playerAttriube.jumpHeight ,  ForceMode.Impulse);

                // update animator if using character
                if (hasAnimator)
                {
                    animator.SetBool(_animIDJump, true);
                }
            }
            // jump timeout
            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }

        }
        else
        {
            // reset the jump timeout timer
            jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                //update animator if using character
                if (hasAnimator)
                {
                    animator.SetBool(_animIDFreeFall, true);
                }
            }

        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private void Move()
    {
        float targetSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            targetSpeed = playerAttriube.sprintSpeed;
        }
        else
        {
            targetSpeed = playerAttriube.moveSpeed;
        }
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(hInput, 0.0f, vInput);
        movement = transform.TransformDirection(movement); // convert movement from local to world space

        if (movement == Vector3.zero) targetSpeed = 0;

        float currentHorizontalSpeed = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z).magnitude;
        _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;

        rb.AddForce(movement.normalized * _speed * 1000 * Time.deltaTime); //+ new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
        //rb.velocity = movement.normalized * _speed + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime;

        //else if (!Grounded)
        //{
        //    // Apply gravity to the character
        //    rb.AddForce(new Vector3(0f, -Gravity, 0f) * Time.deltaTime);

        //    // Limit character's fall speed to avoid reaching terminal velocity too quickly
        //    if (rb.velocity.y < -MaxFallSpeed)
        //    {
        //        rb.velocity = new Vector3(rb.velocity.x, -MaxFallSpeed, rb.velocity.z);
        //    }

        //    // Apply character's movement direction and speed while in the air
        //    rb.AddForce(movement.normalized * _airSpeed * 1000 * Time.fixedDeltaTime);
        //}


        // update animator if using character
        if (hasAnimator)
        {
            animator.SetFloat(_animIDSpeed, animationBlend);
            animator.SetFloat(_animIDMotionSpeed, 1);
        }
    }
    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameManagerr.Instance.EditHitPlayers(this.gameObject, collision.gameObject.GetComponent<Ball>().ballTeamID);
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.position, FootstepAudioVolume);
        }
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, FootstepAudioVolume);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);

    }

    public void ResetAnimator()
    {
        animator.SetFloat(_animIDSpeed, 0);
        animator.SetBool(_animIDGrounded, Grounded);
        animator.SetBool(_animIDJump, false);
        animator.SetBool(_animIDFreeFall, false);
        animationBlend = 0;
        jumpTimeoutDelta = JumpTimeout;
        fallTimeoutDelta = FallTimeout;
    }

}