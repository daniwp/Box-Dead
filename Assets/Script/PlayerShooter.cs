using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    public int damagePerShot = 20;                  
    public float timeBetweenBullets = 0.15f;        
    public float range = 100f;                      
    public int totalAmmo;
    public int startAmmo;
    public int clipSize;
    public float reloadTime;
    public Slider reloadSlider;
    public GameObject player;

    PlayerHealth playerHealth;
    int currentClipAmount;
    static bool reloading = false;
    float timer;                                    
    Ray shootRay;                                   
    RaycastHit shootHit;                            
    int shootableMask;                              
    int obstacleMask;
    ParticleSystem gunParticles;                    
    float effectsDisplayTime = 0.2f;          
    LineRenderer shotLine;
    int score = 0;
    AudioSource[] sounds;
    AudioSource noAmmoSound;
    AudioSource shotSound;
    AudioSource reloadSound;
    float totalSliderTime;

    void Awake()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        totalSliderTime = reloadTime;
        reloadSlider.gameObject.SetActive(false);
        shootableMask = LayerMask.GetMask("Shootable");
        obstacleMask = LayerMask.GetMask("Obstacle");
        gunParticles = GetComponent<ParticleSystem>();
        shotLine = GetComponent<LineRenderer>();
        sounds = GetComponents<AudioSource>();
        noAmmoSound = sounds[0];
        shotSound = sounds[1];
        reloadSound = sounds[2];
        currentClipAmount = clipSize;
        startAmmo = totalAmmo;
        totalAmmo -= clipSize;
    }

    void Update()
    {
        timer += Time.deltaTime;

        shootRay.origin = transform.position;
        shotLine.SetPosition(0, transform.position);
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            shotLine.SetPosition(1, shootHit.point);
        } else if (Physics.Raycast(shootRay, out shootHit, range, obstacleMask)) {
            shotLine.SetPosition(1, shootHit.point);
        } else
        {
            shotLine.SetPosition(1, shootRay.GetPoint(100f));
        }

        if (reloading)
        {
            reloadSlider.gameObject.SetActive(true);
            StartCoroutine(AnimateSliderOverTime(reloadTime));
        }



        if (Input.GetKeyDown(KeyCode.R) && totalAmmo != 0 && currentClipAmount != clipSize && !reloading)
        {
            if (totalAmmo >= clipSize || totalAmmo < 0 )
            {
                totalAmmo -= clipSize;
                currentClipAmount = clipSize;
            }
            else if (totalAmmo < clipSize && totalAmmo > 0)
            {
                currentClipAmount = totalAmmo;
                totalAmmo = 0;
            }
            StartCoroutine(reloadWait());
        }

        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && currentClipAmount != 0 && !reloading)
        {
            // ... shoot the gun.
            Shoot();
        } else if(Input.GetButtonDown("Fire1") && currentClipAmount <= 0 && !reloading)
        {
            noAmmoSound.Stop();
            noAmmoSound.loop = true;
            noAmmoSound.Play();

        } else if (Input.GetButtonUp("Fire1")) {
            noAmmoSound.loop = false;
        }
    }

    IEnumerator AnimateSliderOverTime(float seconds)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            reloadSlider.value = Mathf.Lerp(0f, 100f, lerpValue);
            yield return null;
        }
    }

    public IEnumerator reloadWait()
    {
        reloadSound.Play();
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        reloadSlider.gameObject.SetActive(false);
        reloadSound.Stop();
    }

    void Shoot()
    {

        if (playerHealth.getIsDead()) return;

        currentClipAmount -= 1;
        timer = 0f;

        gunParticles.Stop();
        gunParticles.Play();
        shotSound.Play();

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, obstacleMask))
        {
            shotLine.SetPosition(1, shootHit.point);
        }
        else if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            }

            shotLine.SetPosition(1, shootHit.point);
        }  else
        {
            shotLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }

    public int getCurrentClipAmount()
    {
        return currentClipAmount;
    }
    public int getTotalAmmo()
    {
        return totalAmmo;
    }

    public bool getReloading()
    {
        return reloading;
    }

    public void resetAmmo()
    {
        totalAmmo = startAmmo - clipSize;
        currentClipAmount = clipSize;
    }
}