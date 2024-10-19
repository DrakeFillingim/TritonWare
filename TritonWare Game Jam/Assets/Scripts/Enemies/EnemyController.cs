using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject _bulletPrefab;
    private Sprite _bulletSprite;

    private Transform _player;

    private Node _rootNode;

    private void Start()
    {
        _bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
        _bulletPrefab.GetComponent<SpriteRenderer>().color = Color.red;
        _bulletSprite = Resources.Load<Sprite>("Sprites/PlayerBullet");
        _player = GameObject.Find("Player").transform;
        _rootNode = new SequencerNode(new Node[] {
            new LeafNode(IsPlayerLOS), new LeafNode(FireBullet)
        });
    }

    private void Update()
    {
        _rootNode.Evaluate();
    }

    private Node.NodeStates IsPlayerLOS()
    {
        if (Physics2D.Raycast(transform.position, _player.position - transform.position))
        {
            return Node.NodeStates.Success;
        }
        return Node.NodeStates.Failure;
    }

    private Node.NodeStates FireBullet()
    {
        GameObject firedBullet = Instantiate(_bulletPrefab, transform);
        ProjectileController controller = firedBullet.AddComponent<ProjectileController>();
        Vector3 toPlayer = _player.position - transform.position;
        float angleToPlayer = Mathf.Atan2(toPlayer.y, toPlayer.x);
        //Debug.DrawRay(transform.position, new Vector3(Mathf.Cos(angleToPlayer), Mathf.Sin(angleToPlayer), 0).normalized, Color.yellow, 5);
        //print(angleToPlayer);
        controller.Initialize(new Vector2(Mathf.Sin(angleToPlayer), Mathf.Cos(angleToPlayer)).normalized, 20 * Time.fixedDeltaTime, _bulletSprite);
        return Node.NodeStates.Success;
    }
}
