using UnityEngine;
using System;
using System.Reflection;

public class Pickup : MonoBehaviour
{
    private string _statToChange;
    private float _changeBy;

    public void Initialize(string statToChange, float changeBy)
    {
        _statToChange = statToChange;
        _changeBy = changeBy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats stats = collision.GetComponent<PlayerStats>();
        if (stats != null)
        {
            PropertyInfo info = stats.GetType().GetProperty(_statToChange);
            if (info != null)
            {
                Type propertyType = info.GetValue(stats).GetType();
                float value = (float)Convert.ChangeType(info.GetValue(stats), typeof(float));
                info.SetValue(stats, Convert.ChangeType(value + _changeBy, propertyType));
            }
            else
            {
                print("error finding stat " + _statToChange);
            }
        }
        Destroy(gameObject);
    }
}