using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
    {
        public int startingHealth = 100;            
        public int currentHealth;                   
        public float sinkSpeed = 2.5f;              
        public int scoreValue = 10;                 
        public GameObject[] drops;    

        ParticleSystem hitParticles;                
        CapsuleCollider capsuleCollider;            
        bool isDead;                                
        bool isSinking;                             
        Text text;


        void Awake()
        {
            hitParticles = GetComponentInChildren<ParticleSystem>();
            capsuleCollider = GetComponent<CapsuleCollider>();

            currentHealth = startingHealth;
        }

        void Update()
        {
            if (isSinking)
            {
                transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
            }
        }


        public void TakeDamage(int amount, Vector3 hitPoint)
        {

        if (isDead)
            return;

            currentHealth -= amount;

            hitParticles.Play();

            if (currentHealth <= 0)
            {
                Death();
            }
        }


        void Death()
        {
            DropRoll();
            isDead = true;
            GetComponent<CapsuleCollider>().enabled = false;
            StartSinking();
            capsuleCollider.isTrigger = true;
            ScoreManager.score += scoreValue;

    }


        public void StartSinking()
        {
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;

            isSinking = true;

            Destroy(gameObject, 2f);
        }

        void DropRoll()
        {

        int roll = Random.Range(0, 100);
        
        if (startingHealth == 500 && roll < 60)
        {
            DropGun();
        } else if (roll <= 7)
        {
            DropHealth();
        } else if (roll >= 95)
        {
            DropGun();
        }

        
    }

    void DropGun()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = 1.42f;
        Instantiate(drops[1], spawnPos, drops[1].transform.rotation);
    }

    void DropHealth()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = 1.42f;
        Instantiate(drops[0], spawnPos, drops[0].transform.rotation);
    }
}

