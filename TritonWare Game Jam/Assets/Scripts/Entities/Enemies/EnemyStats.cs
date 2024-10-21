using UnityEngine;

public class EnemyStats : MonoBehaviour, IEntityStats
{
    private const int StartingHealth = 25;

    public int MaxHealth { get; set; } = StartingHealth;

    private int _currentHealth = StartingHealth;
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            if (value > MaxHealth)
            {
                value = MaxHealth;
            }
            _currentHealth = value;
            if (_currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public float WalkSpeed { get; set; } = 5;
    public int BulletsFired { get; set; } = 5;
}
