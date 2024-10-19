using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private const float AttackAngle = 10;
    private const int EvenStartAngle = -5;

    private InputActionMap _inputMap;
    private GameObject _bulletPrefab;
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

        _bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
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
            float currentAngle = 0;
            if (_stats.BulletsFired % 2 == 0)
            {
                currentAngle = EvenStartAngle;
            }
            for (int i = 0; i < _stats.BulletsFired; i++)
            {
                if (_stats.CurrentAmmo > 0)
                {
                    float addBy = AttackAngle * i;
                    if (i % 2 == 0)
                    {
                        addBy *= -1;
                    }
                    currentAngle += addBy;
                    GameObject firedBullet = Instantiate(_bulletPrefab, transform);
                    ProjectileController controller = firedBullet.AddComponent<ProjectileController>();
                    print("angel: " + currentAngle + "\n: " + Mathf.Sign(_previousVelocity.x) * new Vector2(Mathf.Sin(currentAngle * Mathf.Deg2Rad), Mathf.Cos(currentAngle * Mathf.Deg2Rad)).normalized);
                    controller.Initialize(Mathf.Sign(_previousVelocity.x) * new Vector2(Mathf.Sin(currentAngle * Mathf.Deg2Rad), Mathf.Cos(currentAngle * Mathf.Deg2Rad)).normalized, 20 * Time.fixedDeltaTime, _bulletSprite);
                    _stats.CurrentAmmo--;
                }
            }
        }
    }
}
