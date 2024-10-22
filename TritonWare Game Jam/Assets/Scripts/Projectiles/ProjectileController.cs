using UnityEngine;
using System.Collections.Generic;

public class ProjectileController : MonoBehaviour
{
    public event System.Action projectileDestroyed;

    private Vector2 _direction;
    private float _speed;
    private Transform _root;
    private bool _destroyOnCollide = true;

    private static GameObject _bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");

    public static GameObject[] ShootBullet(int bulletsToShoot, float startingAngle, float angleChange, Transform parent, Sprite icon, Vector2 startPosition, float speed, bool destroyOnCollide)
    {
        List<GameObject> bulletsFired = new List<GameObject>();
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
            controller.Initialize(new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad)).normalized, speed * Time.fixedDeltaTime, icon, destroyOnCollide);
            firedBullet.transform.parent = null;
            firedBullet.transform.position = startPosition;
            bulletsFired.Add(firedBullet);
        }
        return bulletsFired.ToArray();
    }

    public void Initialize(Vector2 direction, float speed, Sprite icon, bool destroyOnCollide)
    {
        GetComponent<SpriteRenderer>().sprite = icon;
        _root = transform.parent;
        _direction = direction;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg);
        _speed = speed;
        _destroyOnCollide = destroyOnCollide;

        Collider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }

    private void FixedUpdate()
    {
        CheckCollision();
        transform.position += new Vector3(_direction.x * _speed, _direction.y * _speed, 0);
    }

    private void CheckCollision()
    {
        Vector2 colliderSize = GetComponent<BoxCollider2D>().bounds.size;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, colliderSize.y / 2, _direction, _speed, ~LayerMask.GetMask("Bullets", "Powerups"));
        if (hit.collider != null)
        {
            OnCollision(hit.collider);
        }
    }

    private void OnCollision(Collider2D collision)
    {
        if (collision.transform != _root)
        {
            IEntityStats stats = collision.GetComponent<IEntityStats>();
            if (stats != null)
            {
                stats.CurrentHealth--;
                if (_destroyOnCollide)
                {
                    projectileDestroyed?.Invoke();
                    Destroy(gameObject);
                }
            }
            else
            {
                projectileDestroyed?.Invoke();
                Destroy(gameObject);
            }
        }
        
    }
}