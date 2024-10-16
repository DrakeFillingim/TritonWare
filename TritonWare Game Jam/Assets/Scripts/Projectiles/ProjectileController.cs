using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float _speed;

    public void Initialize(Vector2 direction, float speed)
    {
        transform.rotation = Quaternion.Euler(direction.x * Mathf.Rad2Deg, direction.y * Mathf.Rad2Deg, 0);
        _speed = speed;
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * _speed;
    }
}