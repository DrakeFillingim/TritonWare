using UnityEngine;

public class WaveAttack : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;

    public void Initialize(Vector2 direction, Vector2 startPosition, float speed)
    {
        transform.parent = null;
        transform.position = startPosition;
        _direction = direction;
        _speed = speed * Time.fixedDeltaTime;

        if (direction.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void Update()
    {
        transform.position += new Vector3(_direction.x * _speed, _direction.y * _speed, 0);
    }
}
