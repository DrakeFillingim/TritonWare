using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;
    private Transform _root;



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
            print("collision: " + collision + "root: " + _root);
            IEntityStats stats = collision.GetComponent<IEntityStats>();
            if (stats != null)
            {
                stats.CurrentHealth--;
            }
            Destroy(gameObject);
        }
        
    }
}