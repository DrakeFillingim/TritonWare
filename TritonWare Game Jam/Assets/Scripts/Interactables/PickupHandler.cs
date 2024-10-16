using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    private GameObject _pickupPrefab;


    private void Start()
    {
        _pickupPrefab = Resources.Load<GameObject>("Prefabs/Pickup");
        CreatePickup();
    }

    private void CreatePickup()
    {
        GameObject pickup = Instantiate(_pickupPrefab);
        pickup.transform.position = new Vector3(0, 1, 0);
        Pickup controller = pickup.AddComponent<Pickup>();
        controller.Initialize("BulletsFired", 1);
    }
}