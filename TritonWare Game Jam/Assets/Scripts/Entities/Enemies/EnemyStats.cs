using UnityEngine;

public class EnemyStats : MonoBehaviour, IEntityStats
{
    private int Phase = 0;
    private GameObject music1;
    private GameObject music2;
    private GameObject music3;
    private GameObject Player;
    private const int StartingHealth = 25;
    public int MaxHealth { get; set; } = StartingHealth;
    private int _currentHealth = StartingHealth;
    private void Start()
    {
        music1 = GameObject.Find("PhaseOneMusic");
        music2 = GameObject.Find("PhaseTwoMusic");
        music3 = GameObject.Find("TritonDefeated");
        music1.GetComponent<FMODUnity.StudioEventEmitter>().Play();
        Phase = 1;
    }
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
            if (_currentHealth <= StartingHealth / 2 && Phase == 1)
            {
                Phase = 2;
                music1.GetComponent<FMODUnity.StudioEventEmitter>().Stop();
                music2.GetComponent<FMODUnity.StudioEventEmitter>().Play();
            }
            if (_currentHealth <= 0)
            {
                Destroy(gameObject);
                Phase = 0;
                music1.GetComponent<FMODUnity.StudioEventEmitter>().Stop();
                music2.GetComponent<FMODUnity.StudioEventEmitter>().Stop();
                music3.GetComponent<FMODUnity.StudioEventEmitter>().Play();
            }
        }
    }

    public float WalkSpeed { get; set; } = 5;
    public int BulletsFired { get; set; } = 4;
}
