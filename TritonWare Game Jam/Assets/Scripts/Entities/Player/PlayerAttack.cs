using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private const float AttackAngle = 10;
    private const int EvenStartAngle = -5;

    private InputActionMap _inputMap;
    private Sprite _bulletSprite;

    private Rigidbody2D _rb;
    private Vector2 _previousVelocity = Vector2.right;

    private PlayerStats _stats;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _stats = GetComponent<PlayerStats>();
        _inputMap = GameObject.Find("InputHandler").GetComponent<PlayerInput>().actions.FindActionMap("Player");
        _inputMap["Attack"].performed += OnAttack;

        _bulletSprite = Resources.Load<Sprite>("Sprites/PlayerBullet");
    }

    private void FixedUpdate()
    {
        if (_rb.velocity.magnitude >= 0.01f)
        {
            _previousVelocity = _rb.velocity;
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (context.control.IsActuated())
        {
            int bulletsToFire = _stats.BulletsFired;
            if (bulletsToFire > _stats.CurrentAmmo)
            {
                bulletsToFire = _stats.CurrentAmmo;
            }

            float startingAngle = 0;
            if (bulletsToFire % 2 == 0)
            {
                startingAngle = EvenStartAngle;
            }
            if (_previousVelocity.x < 0)
            {
                startingAngle += 180;
            }
            ProjectileController.ShootBullet(bulletsToFire, startingAngle, AttackAngle, transform, _bulletSprite);
            _stats.CurrentAmmo -= bulletsToFire;
        }
    }
}