using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private const float Acceleration = .2f;
    private const float Deceleration = .1f;
    private const int DashAmmo = 5;

    private InputActionMap _inputMap;

    private Rigidbody2D _rb;
    private PlayerStats _stats;
    private SpriteRenderer _renderer;
    private Animator _animator;
    private BoxCollider2D _hitBox;

    private float _inputDirection;

    private Timer _startCheckCooldown;
    private bool _checkJumps = true;
    private float _checkJumpCooldown = .1f;
    private int _currentJumps = 0;

    private Vector2 _gravityDirection = Vector2.down;
    private float _gravityScale = 37;
    private bool _addGravity = true;

    private Timer _dashRecharge;
    private float _dashCooldown = 1.5f;
    private bool _canDash = true;
    private bool _dashEnded = false;
    private float _xToAdd = 0;

    private System.Action _currentMovement;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;
        _stats = GetComponent<PlayerStats>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _hitBox = GetComponent<BoxCollider2D>();

        _animator.speed = 0;

        _startCheckCooldown = Timer.CreateTimer(gameObject, () => _checkJumps = true, _checkJumpCooldown);
        _dashRecharge = Timer.CreateTimer(gameObject, () => _canDash = true, _dashCooldown);

        _currentMovement = AddPlayerForce;
    }

    private void OnEnable()
    {
        _inputMap = GameObject.Find("InputHandler").GetComponent<PlayerInput>().actions.FindActionMap("Player");
        _inputMap["Move"].performed += OnMove;
        _inputMap["Jump"].performed += OnJump;
        _inputMap["Dash"].performed += OnDash;
        _inputMap["Pause"].performed += _ => Debug.Break();
    }

    private void OnDisable()
    {
        _inputMap["Move"].performed -= OnMove;
        _inputMap["Jump"].performed -= OnJump;
        _inputMap["Dash"].performed -= OnDash;
        _inputMap["Pause"].performed -= _ => Debug.Break();
    }

    private void Update()
    {
        if (_dashEnded)
        {
            transform.position += new Vector3(_xToAdd, 0, 0);
            _dashEnded = false;
            _xToAdd = 0;
        }
    }

    private void FixedUpdate()
    {
        ResetVelocity();
        ResetJumps();
        _currentMovement();
        AddGravityForce();
    }

    #region InputMessages
    private void OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<float>();
        if (_inputDirection < 0)
        {
            _animator.speed = 1;
            _renderer.flipX = true;
        }
        if (_inputDirection > 0)
        {
            _animator.speed = 1;
            _renderer.flipX = false;
        }
        if (_inputDirection == 0 && _addGravity)
        {
            _animator.speed = 0;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_currentJumps < _stats.MaxJumps)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(Vector2.up * _stats.JumpHeight, ForceMode2D.Impulse);
            _currentJumps++;
            _checkJumps = false;
            _startCheckCooldown.StartTimer();
        }
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (context.control.IsActuated() && _canDash)
        {
            _animator.speed = 1;
            _animator.Play("PlayerDash");
            _addGravity = false;
            _currentMovement = AddDashForce;
            _canDash = false;
            _rb.velocity = Vector2.zero;
            if (_currentJumps > 0)
            {
                _currentJumps--;
            }
        }
    }
    #endregion

    public void OnDashExit()
    {
        _dashRecharge.StartTimer();
        _addGravity = true;
        _currentMovement = AddPlayerForce;
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _stats.CurrentAmmo += DashAmmo;
        
        _animator.Play("PlayerWalk");
        _dashEnded = true;
        _xToAdd = _renderer.sprite.bounds.size.x - _hitBox.bounds.size.x;
        if (_renderer.flipX)
        {
            _xToAdd *= -1;
        }
    }

    private void ResetVelocity()
    {
        if (_rb.velocity.magnitude <= 0.01f)
        {
            _rb.velocity = Vector2.zero;
        }
    }

    private void ResetJumps()
    {
        if (IsGrounded() && _checkJumps)
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

    private void AddDashForce()
    {
        //_rb.velocity = (_dashDirection * _dashSpeed) / _dashTime;
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
        if (Physics2D.Raycast(transform.position, _gravityDirection, _hitBox.bounds.size.y / 2 + 0.2f, LayerMask.GetMask("Ground")))
        {
            return true;
        }
        return false;
    }
}