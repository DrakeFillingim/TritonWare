using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

public class PickupHandler : MonoBehaviour
{
    private const float PowerupCooldown = 2;

    private GameObject _pickupPrefab;
    private float _xMin;
    private float _xMax;
    private float _yValue = -1;

    private Dictionary<string, PowerupStats> powerupValues;

    private void Start()
    {
        Tilemap map = GameObject.Find("Grid/Tilemap").GetComponent<Tilemap>();
        _xMin = map.transform.TransformPoint(map.localBounds.min).x;
        _xMax = map.transform.TransformPoint(map.localBounds.max).x;

        _pickupPrefab = Resources.Load<GameObject>("Prefabs/Pickup");
        Timer.CreateTimer(gameObject, CreatePickup, PowerupCooldown, true);

        powerupValues = new Dictionary<string, PowerupStats>()
        {
            { "CurrentAmmo", new PowerupStats(10, 0, Resources.Load<Sprite>("Sprites/PlayerBullet")) },
            { "WalkSpeed", new PowerupStats(7.5f, 5, Resources.Load<Sprite>("Sprites/SpeedupBoost")) },
            { "BulletsFired", new PowerupStats(2, 5, Resources.Load<Sprite>("Sprites/Shoot3Bullets")) },
            { "CurrentHealth", new PowerupStats(1, 0, Resources.Load<Sprite>("Sprites/Plus1Heart")) }
        };
    }

    private void CreatePickup()
    {
        GameObject pickup = Instantiate(_pickupPrefab);
        float xSpawn = Random.Range(_xMin, _xMax);
        pickup.transform.position = new Vector3(xSpawn, _yValue, 0);
        Pickup controller = pickup.AddComponent<Pickup>();
        string powerupName = powerupValues.Keys.ToArray()[Random.Range(0, powerupValues.Keys.Count)];
        controller.Initialize(powerupName, powerupValues[powerupName]);
    }
}

public class PowerupStats
{
    public float Amount { get; set; } = 0;
    public float Duration { get; set; } = 0;
    public Sprite Icon { get; set; }

    public PowerupStats(float amount, float duration, Sprite icon)
    {
        Amount = amount;
        Duration = duration;
        Icon = icon;
    }
}