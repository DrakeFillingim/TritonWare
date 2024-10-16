using UnityEngine;
using UnityEngine.Tilemaps;
using System.Reflection;

public class PickupHandler : MonoBehaviour
{
    private const float PowerupCooldown = 3;

    private GameObject _pickupPrefab;
    private float _xMin;
    private float _xMax;
    private float _yValue;

    private string[] _powerupNames = new string[8];

    private void Start()
    {
        Tilemap map = GameObject.Find("Grid/Tilemap").GetComponent<Tilemap>();
        _xMin = map.transform.TransformPoint(map.localBounds.min).x;
        _xMax = map.transform.TransformPoint(map.localBounds.max).x;
        _yValue = map.transform.TransformPoint(map.localBounds.max).y - 1;

        _pickupPrefab = Resources.Load<GameObject>("Prefabs/Pickup");
        Timer.CreateTimer(gameObject, CreatePickup, PowerupCooldown, true);

        int i = 0;
        foreach (PropertyInfo p in typeof(PlayerStats).GetProperties()[0..8])
        {
            _powerupNames[i] = p.Name;
            i++;
        }
    }

    private void CreatePickup()
    {
        GameObject pickup = Instantiate(_pickupPrefab);
        float xSpawn = Random.Range(_xMin, _xMax);
        pickup.transform.position = new Vector3(xSpawn, _yValue, 0);
        Pickup controller = pickup.AddComponent<Pickup>();
        controller.Initialize(_powerupNames[Random.Range(0, _powerupNames.Length - 1)], Random.Range(1, 5));
    }
}