using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BackgroundMusic : MonoBehaviour
{

    public const int maxHealth = 100;
    public int currentHealth;
    public float PhaseTwoTriggerHealth = (float)maxHealth / 2;
    public bool PhaseTwo = false;
    public GameObject music1;
    public GameObject music2;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        music1 = GameObject.Find("PhaseOneMusic");
        music2 = GameObject.Find("PhaseTwoMusic");
        music1.GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }
    void Update()
    {
        if(currentHealth <= PhaseTwoTriggerHealth && PhaseTwo == false)
        {
            music1.GetComponent<FMODUnity.StudioEventEmitter>().Stop();
            music2.GetComponent<FMODUnity.StudioEventEmitter>().Play();
            PhaseTwo = true;
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
    }
}
