using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float _speed;
    private Transform _root;

    public void Initialize(Vector2 direction, float speed, Sprite icon)
    {
        GetComponent<SpriteRenderer>().sprite = icon;
        _root = transform.parent;
        transform.parent = null;
        print(_root);
        transform.SetPositionAndRotation(_root.position, Quaternion.Euler(0, direction.y * Mathf.Rad2Deg, 0));
        _speed = speed;

        Collider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * _speed;
        CheckCollision();
    }

    private void CheckCollision()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, transform.localScale.y, transform.forward, _speed * Time.fixedDeltaTime, ~LayerMask.GetMask("Bullets"));
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
            Destroy(gameObject);
        }
        
    }
}