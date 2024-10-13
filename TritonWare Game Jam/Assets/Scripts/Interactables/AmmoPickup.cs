using UnityEngine;

public class AmmoPickup : MonoBehaviour, IInteractable
{
    private const int AmmoAmount = 3;

    private float _rechargeTime = 5;
    private float _currentTime = 0;
    private bool _canPickup = true;

    private void Update()
    {
        if (!_canPickup)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= _rechargeTime)
            {
                _canPickup = true;
                _currentTime = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats stats = collision.GetComponent<PlayerStats>();
        if (stats != null && _canPickup)
        {
            print("here");
            OnPickup(stats);
            _canPickup = false;
        }
    }

    public void OnPickup(PlayerStats stats)
    {
        stats.CurrentAmmo += AmmoAmount;
    }
}