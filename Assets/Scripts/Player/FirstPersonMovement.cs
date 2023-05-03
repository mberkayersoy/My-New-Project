using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class FirstPersonMovement : MonoBehaviourPun, IPunObservable
{
    PhotonView pw;
    public PlayerManage playerManage;
    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = 1;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]

    [Space(5)]

    public LayerMask GroundLayers;
    public PlayerAttribute player;
    private float _speed;
    //Rigidbody rb;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public CharacterController _controller;
    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    private Animator animator;
    private bool hasAnimator;
    private float animationBlend;

    // timeout deltatime
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;



    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;
    private float _targetRotation = 0.0f;
    private StarterAssetsInputs input;
    private PlayerInput playerInput;
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
    public new Camera camera;
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;
    private Vector3 characterPosition;
    private Quaternion characterRotation;

    private void Start()
    {
        player = GetComponent<PlayerAttribute>();
        _controller = GetComponent<CharacterController>();
        input = GetComponent<StarterAssetsInputs>();
        playerInput = GetComponent<PlayerInput>();
        hasAnimator = TryGetComponent(out animator);
        AssignAnimationIDs();
        pw = GetComponent<PhotonView>();
        playerManage = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<PlayerManage>();


        // reset our timeouts on start
        jumpTimeoutDelta = JumpTimeout;
        fallTimeoutDelta = FallTimeout;
    }
    void FixedUpdate()
    {
        if (!pw.IsMine) 
        {
            return;
        }
        characterPosition = transform.position;
            characterRotation = transform.rotation;
            if (!player.isDead)
            {
                
                JumpAndGravity();
                Move();
            }
            else
            {

                //ResetAnimator();
                //Debug.Log("animationBlend: " + animationBlend);
            }

    }
    private void Update()
    {
        GroundedCheck();
    }

    private void LateUpdate()
    {
        //CameraRotation();
    }
    [PunRPC]
    private void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = input.sprint ? player.sprintSpeed : player.moveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (input.move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              camera.transform.eulerAngles.y;
            //float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref rotationVelocity,
            //    0.125f);

            //rotate to face input direction relative to camera position
            //if (_rotateOnMove)
            //{
            //    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            //}

        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        //Debug.Log("Speed: " + _speed);
        // move the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator if using character
        if (hasAnimator)
        {
            animator.SetFloat(_animIDSpeed, animationBlend);
            animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
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
    [PunRPC]
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
                verticalVelocity = Mathf.Sqrt(player.jumpHeight * -2f * Gravity);

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
    [PunRPC]
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
    [PunRPC]
    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }
    }
    [PunRPC]
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
    [PunRPC]
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ball"))
        {
            GameManagerr.Instance.EditHitPlayers(this.gameObject, hit.gameObject.GetComponent<Ball>().ballTeamID);
        }
    }
    [PunRPC]
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(characterPosition);
            stream.SendNext(characterRotation);
        }
        else
        {
            characterPosition = (Vector3)stream.ReceiveNext();
            characterRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}