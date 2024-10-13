using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private const int StartingHealth = 3;
    private const int StartingAmmo = 15;

    public int MaxHealth { get; set; } = StartingHealth;
    public int CurrentHealth { get; set; } = StartingHealth;

    public float WalkSpeed { get; set; } = 5;
    public float MaxJumps { get; set; } = 2;
    public float JumpHeight { get; set; } = 15;

    public int MaxAmmo { get; set; } = StartingAmmo;
    public int CurrentAmmo { get; set; } = StartingAmmo;
}
