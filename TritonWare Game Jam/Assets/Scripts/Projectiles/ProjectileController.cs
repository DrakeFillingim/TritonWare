using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;
    private Transform _root;

    private static GameObject _bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");

    public static void ShootBullet(int bulletsToShoot, float startingAngle, float angleChange, Transform parent, Sprite icon)
    {
        float currentAngle = startingAngle;
        for (int i = 0; i < bulletsToShoot; i++)
        {
            float addBy = angleChange * i;
            if (i % 2 == 0)
            {
                addBy *= -1;
            }
            currentAngle += addBy;
            GameObject firedBullet = Instantiate(_bulletPrefab, parent);
            ProjectileController controller = firedBullet.AddComponent<ProjectileController>();
            controller.Initialize(new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad)).normalized, 20 * Time.fixedDeltaTime, icon);
        }
    }

    public void Initialize(Vector2 direction, float speed, Sprite icon)
    {
        GetComponent<SpriteRenderer>().sprite = icon;
        _root = transform.parent;
        transform.parent = null;
        _direction = direction;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg);
        _speed = speed;

        Collider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(_direction.x * _speed, _direction.y * _speed, 0);
        CheckCollision();
    }

    private void CheckCollision()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, transform.localScale.y, _direction, _speed, ~LayerMask.GetMask("Bullets", "Powerups"));
        if (hit.collider != null)
        {
            OnCollision(hit.collider);
        }
    }

    private void OnCollision(Collider2D collision)
    {
        if (collision.transform != _root)
        {
            //print("collision: " + collision + "root: " + _root);
            IEntityStats stats = collision.GetComponent<IEntityStats>();
            if (stats != null)
            {
                stats.CurrentHealth--;
            }
            Destroy(gameObject);
        }
        
    }
}