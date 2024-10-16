using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerMovement : MonoBehaviour
{
    private const float Acceleration = .2f;
    private const float Deceleration = .1f;

    private InputActionMap _inputMap;

    private Rigidbody2D _rb;
    private PlayerStats _stats;

    private float _inputDirection;

    private Vector3 _gravityDirection = Vector3.down;
    private float _gravityScale = 30;
    private bool _addGravity;

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
       
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(Vector2.up * _stats.JumpHeight, ForceMode2D.Impulse);

    }

    private void OnDash(InputAction.CallbackContext context)
    {

    }
    #endregion

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
        if (!IsGrounded())
        {
            _rb.AddForce(_gravityDirection * _gravityScale, ForceMode2D.Force);
        }
    }

    private bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, _gravityDirection, transform.localScale.x / 2, LayerMask.GetMask("Ground")))
        {
            return true;
        }
        return false;
    }
}