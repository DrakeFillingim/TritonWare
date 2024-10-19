using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private const float AttackAngle = 5;

    private Sprite _bulletSprite;
    private Transform _player;

    private Node _rootNode;

    private Timer _attackTimer;
    private float _attackCooldown = .5f;
    private bool _canAttack = true;

    private Timer _spinTimer;
    private float _spinDuration = .5f;
    private bool _canSpin = true;
    private float _currentSpinAngle = 0;

    private EnemyStats _stats;

    private void Start()
    {
        _bulletSprite = Resources.Load<Sprite>("Sprites/KingTritonProjectile");
        _player = GameObject.Find("Player").transform;
        _rootNode = new SelectorNode(new Node[] {
            new SequencerNode(new Node[] {
                new LeafNode(CheckCanAttack), new LeafNode(CheckPlayerLOS), new LeafNode(FireBullet)
            }),
            new SequencerNode(new Node[] {
                new InverterNode(new LeafNode(CheckPlayerLOS)), new LeafNode(CheckCanSpin), new LeafNode(SpinAttack)
            })
        });

        _attackTimer = Timer.CreateTimer(gameObject, () => _canAttack = true, _attackCooldown);
        _spinTimer = Timer.CreateTimer(gameObject, () => _canSpin = true, _spinDuration / (360.0f / AttackAngle));

        _stats = GetComponent<EnemyStats>();
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
        if (!Physics2D.Raycast(transform.position, toPlayer, toPlayer.magnitude, ~LayerMask.GetMask("Player", "Powerups", "Bullets")))
        {
            return Node.NodeStates.Success;
        }
        return Node.NodeStates.Failure;
    }

    private Node.NodeStates FireBullet()
    {
        Vector3 toPlayer = _player.position - transform.position;
        ProjectileController.ShootBullet(_stats.BulletsFired, Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg, AttackAngle, transform, _bulletSprite);

        _canAttack = false;
        _attackTimer.Start();
        return Node.NodeStates.Success;
    }

    private Node.NodeStates CheckCanSpin()
    {
        if (_canSpin)
        {
            return Node.NodeStates.Success;
        }
        return Node.NodeStates.Failure;
    }

    private Node.NodeStates SpinAttack()
    {
        ProjectileController.ShootBullet(1, _currentSpinAngle, AttackAngle, transform, _bulletSprite);
        _currentSpinAngle += AttackAngle;
        _canSpin = false;
        _spinTimer.Start();
        return Node.NodeStates.Success;
    }
}
