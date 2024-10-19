using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject _bulletPrefab;
    private Sprite _bulletSprite;

    private Transform _player;

    private Node _rootNode;

    private Timer _attackTimer;
    private float _attackCooldown = .25f;
    private bool _canAttack = true;

    private void Start()
    {
        _bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
        _bulletSprite = Resources.Load<Sprite>("Sprites/KingTritonProjectile");
        _player = GameObject.Find("Player").transform;
        _rootNode = new SequencerNode(new Node[] {
            new LeafNode(CheckCanAttack), new LeafNode(CheckPlayerLOS), new LeafNode(FireBullet)
        });

        _attackTimer = Timer.CreateTimer(gameObject, () => _canAttack = true, _attackCooldown);
    }

    private void Update()
    {
        _rootNode.Evaluate();
    }

    private Node.NodeStates CheckCanAttack()
    {
        if (_canAttack)
        {
            return Node.NodeStates.Success;
        }
        return Node.NodeStates.Failure;
    }

    private Node.NodeStates CheckPlayerLOS()
    {
        Vector2 toPlayer = _player.position - transform.position;
        //print(Physics2D.Raycast(transform.position, toPlayer, toPlayer.magnitude, ~LayerMask.GetMask("Player", "Powerups")).transform.name);
        if (!Physics2D.Raycast(transform.position, toPlayer, toPlayer.magnitude, ~LayerMask.GetMask("Player", "Powerups")))
        {
            return Node.NodeStates.Success;
        }
        return Node.NodeStates.Failure;
    }

    private Node.NodeStates FireBullet()
    {
        GameObject firedBullet = Instantiate(_bulletPrefab, transform);
        //firedBullet.GetComponent<SpriteRenderer>().color = Color.red;
        ProjectileController controller = firedBullet.AddComponent<ProjectileController>();
        Vector3 toPlayer = _player.position - transform.position;
        print("angle: " + Vector2.Angle(Vector2.right, toPlayer.normalized));
        print("tan: " + Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg);
        controller.Initialize(toPlayer.normalized, 20 * Time.fixedDeltaTime, _bulletSprite);
        _canAttack = false;
        _attackTimer.Start();
        return Node.NodeStates.Success;
    }
}
