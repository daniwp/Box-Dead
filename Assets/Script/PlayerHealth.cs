using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;                      
    public int currentHealth;                                 
    public Slider healthSlider;                              
    public Image damageImage;                                  
    //public AudioClip deathClip;                               
    public float flashSpeed = 5f;                               
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     
    public Text deathText;


    AudioSource playerAudio;                      
    Movement playerMovement;                             
    PlayerShooter playerShooting;                         
    bool isDead;                                       
    bool damaged;                                   


    void Awake()
    {
        // Setting up the references.
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<Movement>();
        playerShooting = GetComponentInChildren<PlayerShooter>();

        // Set the initial health of the player.
        currentHealth = startingHealth;
    }


    void Update()
    {
        if (isDead)
        {
            damageImage.color = flashColour;
        }
        else if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;
    }


    public void TakeDamage(int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }


    void Death()
    {
        isDead = true;
        deathText.text = "You died!";

        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

    public void HealBuff(int health, GameObject obj)
    {

        if (currentHealth + health > startingHealth && currentHealth != 100)
        {
            currentHealth = startingHealth;
            Destroy(obj);
        } else if (currentHealth < startingHealth )
        {
            currentHealth += health;
            healthSlider.value = currentHealth;
            Destroy(obj);
        }
    }

    public bool getIsDead()
    {
        return isDead;
    }
}
