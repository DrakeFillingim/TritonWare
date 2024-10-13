using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;

    public void Initialize(Vector2 direction, float speed)
    {
        _direction = direction;
        _speed = speed;
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(_direction.x * _speed, _direction.y * _speed, 0);
    }
}
