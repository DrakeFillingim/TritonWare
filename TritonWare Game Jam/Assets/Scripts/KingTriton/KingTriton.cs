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
    public GameObject music1;
    public GameObject music2;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        music1 = GameObject.Find("PhaseOneMusic");
        music2 = GameObject.Find("PhaseTwoMusic");
        music1.GetComponent<FMODUnity.StudioEventEmitter>().Play();
        if(currentHealth <= PhaseTwoTriggerHealth)
        {
            PhaseTwo = true;
            music1.GetComponent<FMODUnity.StudioEventEmitter>().Stop();
            music2.GetComponent<FMODUnity.StudioEventEmitter>().Play();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
            music2.GetComponent<FMODUnity.StudioEventEmitter>().Stop();
        }

        healthBar.SetHealth(currentHealth);
    }
}
