using UnityEngine;
using System;
using System.Reflection;

public class Pickup : MonoBehaviour
{
    private string _statToChange;

    private float _changeBy;
    private float _duration;

    private PlayerStats _affectedPlayer;

    private float _startY;
    private float _startTime;

    public void Initialize(string statToChange, PowerupStats infoStats)
    {
        _statToChange = statToChange;
        _changeBy = infoStats.Amount;
        _duration = infoStats.Duration;

        gameObject.layer = 9;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = infoStats.Icon;
        transform.localScale = new Vector3(.4f, .4f, .4f);
        Collider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        _startY = transform.position.y;
        _startTime = Time.time;
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, _startY + Mathf.Sin((Time.time - _startTime) * 2.5f) / 4, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _affectedPlayer = collision.GetComponent<PlayerStats>();
        if (_affectedPlayer != null)
        {
            PropertyInfo info = _affectedPlayer.GetType().GetProperty(_statToChange);
            if (info != null)
            {
                Type propertyType = info.GetValue(_affectedPlayer).GetType();
                float value = (float)Convert.ChangeType(info.GetValue(_affectedPlayer), typeof(float));
                info.SetValue(_affectedPlayer, Convert.ChangeType(value + _changeBy, propertyType));
                if (_duration == 0)
                {
                    Destroy(gameObject);
                }
                Timer.CreateTimer(gameObject, OnBuffTimeout, _duration, true, false);
                gameObject.GetComponent<Collider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                print("error finding stat " + _statToChange);
            }
        }
    }

    private void OnBuffTimeout()
    {
        PropertyInfo info = _affectedPlayer.GetType().GetProperty(_statToChange);
        if (info != null)
        {
            Type propertyType = info.GetValue(_affectedPlayer).GetType();
            float value = (float)Convert.ChangeType(info.GetValue(_affectedPlayer), typeof(float));
            info.SetValue(_affectedPlayer, Convert.ChangeType(value - _changeBy, propertyType));
        }
        Destroy(gameObject);
    }
}