using UnityEngine;

public class PlayerStats : MonoBehaviour, IEntityStats
{
    private const int StartingHealth = 3;
    private const int StartingAmmo = 15;

    public int MaxHealth { get; set; } = StartingHealth;

    public int _currentHealth = StartingHealth;
    public int CurrentHealth {
        get => _currentHealth;
        set
        {
            if (value > MaxHealth)
            {
                value = MaxHealth;
            }
            _currentHealth = value;
        }
    }

    public int MaxAmmo { get; set; } = StartingAmmo;

    private int _currentAmmo = StartingAmmo;
    public int CurrentAmmo {
        get => _currentAmmo;
        set 
        {
            if (value > MaxAmmo)
            {
                value = MaxAmmo;
            }
            _currentAmmo = value;
        } 
    }

    public float WalkSpeed { get; set; } = 5;
    public int MaxJumps { get; set; } = 2;
    public float JumpHeight { get; set; } = 15;
    public int BulletsFired { get; set; } = 1;
}
