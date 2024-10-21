using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private const float AttackAngle = 15;

    private Sprite _waveSprite;
    private Sprite _bulletSprite;
    private SpriteRenderer _renderer;
    private Animator _animator;
    private Transform _player;

    private Node _rootNode;

    private Timer _attackTimer;
    private float _attackCooldown = 1f;
    private bool _canAttack = true;

    private Timer _movementTimer;
    private bool _startMoveTimer = true;
    private float _moveDuration = 1;
    private float _moveDistance = -20;
    private float _startX;

    private bool _canWave = true;
    private Timer _waveRecharge;
    private float _waveCooldown = 5;
    private bool _isWaving = false;
    private bool _checkWave = true;

    private EnemyStats _stats;

    private void Start()
    {
        _waveSprite = Resources.Load<Sprite>("Sprites/WaveAttack");
        _bulletSprite = Resources.Load<Sprite>("Sprites/KingTritonProjectile");
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _animator.speed = 0;
        _player = GameObject.Find("Player").transform;

        _rootNode = new SelectorNode(new Node[] {
            new SequencerNode(new Node[] {
                new LeafNode(CheckPlayerLOS), 
                new SuccessNode(new SequencerNode(new Node[] {
                    new LeafNode(CheckCanAttack), new LeafNode(FireBullet)
                }))
            }),
            new SequencerNode(new Node[] {
                new LeafNode(CanWaveAttack),
                new LeafNode(WaveAttack)
            }),
            new LeafNode(Move)
        });

        _attackTimer = Timer.CreateTimer(gameObject, () => _canAttack = true, _attackCooldown);
        _movementTimer = Timer.CreateTimer(gameObject, () => { }, _moveDuration);
        _waveRecharge = Timer.CreateTimer(gameObject, () => _canWave = true, _waveCooldown);
        _startX = transform.position.x;

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
        _animator.speed = 1;
        Vector3 toPlayer = _player.position - transform.position;
        ProjectileController.ShootBullet(_stats.BulletsFired, Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg, AttackAngle, transform, _bulletSprite, transform.position, 20, true);
        _canAttack = false;
        _attackTimer.StartTimer();
        return Node.NodeStates.Success;
    }

    private Node.NodeStates CanWaveAttack()
    {
        if (_canWave)
        {
            return Node.NodeStates.Success;
        }
        return Node.NodeStates.Failure;
    }

    private Node.NodeStates WaveAttack()
    {
        if (!_isWaving && _checkWave)
        {
            _isWaving = true;
            _canWave = false;
            _checkWave = false;
            _animator.speed = 0;
            int angle = 0;
            float startX = -14;
            if (_player.position.x < transform.position.x)
            {
                angle = 180;
                startX = 14;
            }
            GameObject waveAttack = ProjectileController.ShootBullet(1, angle, 0, transform, _waveSprite, new Vector2(startX, -7.5f), 8, false)[0];
            waveAttack.GetComponent<ProjectileController>().projectileDestroyed += OnWaveDestroyed;
            if (_player.position.x < transform.position.x)
            {
                waveAttack.GetComponent<SpriteRenderer>().flipY = true;
            }
            waveAttack.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (_isWaving)
        {
            return Node.NodeStates.Running;
        }
        _checkWave = true;
        _waveRecharge.StartTimer();
        return Node.NodeStates.Success;
    }

    private Node.NodeStates Move()
    {
        if (_startMoveTimer)
        {
            _movementTimer.StartTimer();
            _startX = transform.position.x;
            _moveDistance *= -1;
            _animator.speed = 0;
        }
        if (_movementTimer.enabled)
        {
            float currentPos = Mathf.Lerp(_startX, _startX + _moveDistance, _movementTimer.CurrentTime / _moveDuration);
            transform.position = new Vector3(currentPos, transform.position.y, transform.position.z);
            _startMoveTimer = false;
            return Node.NodeStates.Running;
        }
        _startMoveTimer = true;
        _renderer.flipX = !_renderer.flipX;
        return Node.NodeStates.Success;
    }

    private void OnWaveDestroyed()
    {
        _isWaving = false;
    }

    public void OnAttackFinish()
    {
        _animator.speed = 0;
    }
}
