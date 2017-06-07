using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWeapon : MonoBehaviour {

    public GameObject gun;
    public Image gunImage;
    public GameObject semiautogun;
    public Image semiImage;
    public Text ammoText;

    List<GameObject> activeWeps = new List<GameObject>();

    void Start()
    {
        activeWeps.Add(gun);
        semiautogun.SetActive(false);
        semiImage.enabled = false;
    }

	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q) && activeWeps.Count > 1 && !gun.transform.GetChild(4).GetComponent<PlayerShooter>().getReloading())
        {
            switchWep();
        }

        if (semiautogun.transform.GetChild(4).GetComponent<PlayerShooter>().getTotalAmmo() < 1 && semiautogun.activeSelf && semiautogun.transform.GetChild(4).GetComponent<PlayerShooter>().getCurrentClipAmount() < 1)
        {
            switchWep();
            activeWeps.Remove(semiautogun);
            semiautogun.transform.GetChild(4).GetComponent<PlayerShooter>().resetAmmo();
        }

        if (semiautogun.activeSelf)
        {
            ammoText.text = "Ammo: " + semiautogun.transform.GetChild(4).GetComponent<PlayerShooter>().getTotalAmmo() + "\n Mag: " + semiautogun.transform.GetChild(4).GetComponent<PlayerShooter>().getCurrentClipAmount();
        } else if(gun.activeSelf)
        {
            ammoText.text = "Ammo: Infinite" + "\n Mag: " + gun.transform.GetChild(4).GetComponent<PlayerShooter>().getCurrentClipAmount();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SemiAuto")
        {
            Debug.Log("HESHA");
            activeWeps.Add(semiautogun);
            semiautogun.transform.GetChild(4).GetComponent<PlayerShooter>().resetAmmo();
            Destroy(other.gameObject);
        }
    }

    private void switchWep()
    {
        

        if(gun.activeSelf)
        {
            gun.SetActive(false);
            semiautogun.SetActive(true);
            ammoText.text = "Ammo: " + semiautogun.transform.GetChild(4).GetComponent<PlayerShooter>().getTotalAmmo();
            gunImage.enabled = false;
            semiImage.enabled = true;
        } else
        {
            gun.SetActive(true);
            semiautogun.SetActive(false);
            gunImage.enabled = true;
            semiImage.enabled = false;
            ammoText.text = "Ammo: Infinite" + " Mag: " + gun.transform.GetChild(4).GetComponent<PlayerShooter>().getCurrentClipAmount();
        }
    }
}
