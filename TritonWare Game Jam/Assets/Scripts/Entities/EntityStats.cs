public interface IEntityStats
{
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

    public float WalkSpeed { get; set; }
    public int BulletsFired { get; set; }
}