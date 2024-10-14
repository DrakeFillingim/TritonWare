using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private InputActionMap _inputMap;
    private GameObject _bulletPrefab;

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
        if (context.control.IsActuated() && _stats.CurrentAmmo > 0)
        {
            GameObject firedBullet = Instantiate(_bulletPrefab);
            firedBullet.transform.parent = null;
            firedBullet.transform.position = transform.position;
            ProjectileController controller = firedBullet.AddComponent<ProjectileController>();
            controller.Initialize(new Vector2(Mathf.Sign(_previousVelocity.x), 0), 20 * Time.fixedDeltaTime);
            _stats.CurrentAmmo--;
        }
    }
}
