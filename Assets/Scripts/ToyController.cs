using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class ToyController : MonoBehaviour
{
    [SerializeField] private Transform movementFrameOfReference;
    [SerializeField] private float jumpSpeed = 4;
    [SerializeField] private float keepPressedJumpAcceleration = 1;

    [SerializeField] private OVRInput.Button jumpButton;
    
    [SerializeField] private Transform respawnTransform;

    [SerializeField] private float maximumLinearSpeed = 0.9f;
    [SerializeField] private float gravity = -9.8f;

    private Animator _animator;
    private CharacterController _characterController;

    private Vector3 _moveVelocity;
    private Quaternion _rotation;
    private Vector2 _motionInput;
    private bool _jumpRequested;
    private JumpingState _jumpingState = JumpingState.Grounded;

    private const float JumpDelay = 0.16f;

    private UniversalAdditionalCameraData centerEyeAnchor;

    private enum JumpingState
    {
        Grounded,
        JumpStarted,
        JumpedAndAirborne
    }

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        

        centerEyeAnchor = FindObjectOfType<UniversalAdditionalCameraData>();
    }

    private void Start()
    {
        movementFrameOfReference = centerEyeAnchor.transform;
        respawnTransform = centerEyeAnchor.transform;
    }

    void Update()
    {
        GetLocomotionInput();
        HandleLocomotion();
        HandleJumping();
        ApplyMotion();
    }

    public void Respawn()
    {
        _characterController.enabled = false;
        transform.position = respawnTransform.position + respawnTransform.forward * 0.3f;
        _characterController.enabled = true;
    }

    private void GetLocomotionInput()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        Vector2 thumbstickAxis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        _motionInput = new Vector2(hInput + thumbstickAxis.x, vInput + thumbstickAxis.y);
    }

    private void ApplyMotion()
    {
        _moveVelocity.y += gravity * Time.deltaTime;
        _characterController.Move(_moveVelocity * Time.deltaTime);
        if (Mathf.Abs(_motionInput.y) > 0 || Mathf.Abs(_motionInput.x) > 0)
        {
            transform.rotation = _rotation;
        }
    }

    private void HandleLocomotion()
    {
        bool noMovementInput = Mathf.Abs(_motionInput.y) == 0 && Mathf.Abs(_motionInput.x) == 0;
        _animator.SetBool("Running", !noMovementInput && _characterController.isGrounded);

        Vector3 motionForwardDirection =
            Vector3.ProjectOnPlane(movementFrameOfReference.forward, Vector3.up).normalized;
        Vector3 motionRightDirection = Vector3.ProjectOnPlane(movementFrameOfReference.right, Vector3.up).normalized;
        Vector3 motionDirection = (motionForwardDirection * _motionInput.y + motionRightDirection * _motionInput.x)
            .normalized;
        _rotation = transform.rotation;

        if (_characterController.isGrounded)
        {
            _moveVelocity = motionDirection * maximumLinearSpeed;
            Vector3 lerpedMoveDirection = Vector3.Lerp(transform.forward, motionDirection, 0.6f);
            _rotation = Quaternion.LookRotation(lerpedMoveDirection);
        }
    }

    private void HandleJumping()
    {
        bool jumpButtonDown = OVRInput.GetDown(jumpButton) || Input.GetButtonDown("Jump");
        bool jumpButtonPressed = OVRInput.Get(jumpButton) || Input.GetButton("Jump");

        if (_jumpRequested)
        {
            _moveVelocity.y = jumpSpeed;
            _jumpRequested = false;
        }

        if (_jumpingState == JumpingState.JumpStarted && !_characterController.isGrounded)
        {
            _jumpingState = JumpingState.JumpedAndAirborne;
        }

        if (_jumpingState != JumpingState.Grounded && jumpButtonPressed)
        {
            _moveVelocity.y += keepPressedJumpAcceleration * Time.deltaTime;
        }

        if (_jumpingState == JumpingState.Grounded && _characterController.isGrounded && jumpButtonDown)
        {
            _jumpingState = JumpingState.JumpStarted;
            StartCoroutine(RequestJumpAfterSeconds(JumpDelay));
            _animator.SetTrigger("Jumping");
        }
        else if (_characterController.isGrounded && _jumpingState == JumpingState.JumpedAndAirborne)
        {
            _animator.SetTrigger("Landed");
            _jumpingState = JumpingState.Grounded;
        }
    }

    private IEnumerator RequestJumpAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        _jumpRequested = true;
    }
}
