using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingTriton : MonoBehaviour
{

    public const int maxHealth = 100;
    public int currentHealth;
    public float PhaseTwoTriggerHealth = (float)maxHealth / 2;
    public bool PhaseTwo = false;
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= PhaseTwoTriggerHealth)
        {
            PhaseTwo = true;
        }
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        healthBar.SetHealth(currentHealth);
    }
}
