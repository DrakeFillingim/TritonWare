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


    public int damage = 5;

    float moveSpeed = 3f; 
    Rigidbody2D rb; 
    Transform target;
    Vector2 moveDirection;

    private bool isDead;

    //public GameManagerScript gameManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        if(target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            moveDirection = direction;
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            TakeDamage(10);
        }
    }
    


    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
    }

    public void TakeDamage (int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
           // gameManager.youWin();
            Destroy(gameObject);
            music2.GetComponent<FMODUnity.StudioEventEmitter>().Stop();
        }

        healthBar.SetHealth(currentHealth);
    }


}

