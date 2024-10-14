using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private const float Acceleration = .2f;
    private const float Deceleration = .1f;

    private InputActionMap _inputMap;

    private Rigidbody2D _rb;
    private PlayerStats _stats;

    private float _inputDirection;
    private int _currentJumps = 0;

    private Vector3 _gravityDirection = Vector3.down;
    private float _gravityScale = 30;
    private bool _addGravity = true;

    private void Start()
    {
        _inputMap = GameObject.Find("InputHandler").GetComponent<PlayerInput>().actions.FindActionMap("Player");
        _inputMap["Move"].performed += OnMove;
        _inputMap["Jump"].performed += OnJump;
        _inputMap["Dash"].performed += OnDash;

        _rb = GetComponent<Rigidbody2D>();
        _stats = GetComponent<PlayerStats>();
    }

    private void FixedUpdate()
    {
        ResetVelocity();
        ResetJumps();
        AddPlayerForce();
        AddGravityForce();
    }

    #region InputMessages
    private void OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<float>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_currentJumps < _stats.MaxJumps - 1)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(Vector2.up * _stats.JumpHeight, ForceMode2D.Impulse);
            _currentJumps++;
        }
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        _rb.velocity = Vector2.zero;
        _currentJumps--;
    }
    #endregion

    private void ResetVelocity()
    {
        if (_rb.velocity.magnitude <= 0.01f)
        {
            _rb.velocity = Vector2.zero;
        }
    }

    private void ResetJumps()
    {
        if (IsGrounded())
        {
            _currentJumps = 0;
        }
    }

    private void AddPlayerForce()
    {
        float targetVelocty = _inputDirection * _stats.WalkSpeed;
        float velocityDifference = (targetVelocty - _rb.velocity.x) / Time.fixedDeltaTime;
        float coefficient = (velocityDifference != 0) ? Acceleration : Deceleration;
        float movement = coefficient * velocityDifference;
        _rb.AddForce(Vector2.right * movement, ForceMode2D.Force);
    }

    private void AddGravityForce()
    {
        if (!IsGrounded() && _addGravity)
        {
            _rb.AddForce(_gravityDirection * _gravityScale, ForceMode2D.Force);
        }
    }

    private bool IsGrounded()
    {
        if (Physics2D.Raycast(transform.position, _gravityDirection, transform.localScale.x + 0.05f, LayerMask.GetMask("Ground")))
        {
            return true;
        }
        return false;
    }
}