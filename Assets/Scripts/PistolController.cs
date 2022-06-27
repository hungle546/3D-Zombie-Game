using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PistolController : MonoBehaviour
{
    [SerializeField] private Camera fpsCam;
    private GameController game;
    public float range = 100f;
    private float rateOfFire = 0.55f;
    private float nextTimeToFire = 0f;
    private float damage = 25;
    private float headshotDamage = 50;
    private string prefix = "Damage increased to ";
    private AudioSource shotAudio;
    private SimpleShoot gunAnimation;
    private float reloadTime;
    public int maxAmmo = 10;
    public int currentAmmo;
    private int magazines = 10;
    private int currentMagazines;
    private bool isReloading = false;
    private bool isAmmoFull = true;

    private float currentDamage;
    // Start is called before the first frame update
    void Start()
    {
        reloadTime = 2f;
        gunAnimation = this.GetComponent<SimpleShoot>();
        currentAmmo = maxAmmo;
        currentMagazines = magazines;
        GameObject gameObj = GameObject.FindWithTag("GameController");
        game = gameObj.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && currentAmmo != 0 && !isReloading)
        {
            //nextTimeToFire = Time.time + 1f / rateOfFire;
            nextTimeToFire = Time.time + rateOfFire;
            Shooting();
            gunAnimation.StartAnimation();
        }

        if (Input.GetKeyDown("r") && currentMagazines != 0 && !isReloading && !isAmmoFull)
        {
            StartCoroutine(Reload());
        }
    }
    
    private void Shooting()
    {
        shotAudio = GetComponent<AudioSource>();
        shotAudio.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            if (hit.collider.tag == "Zombie")
            {
                //Debug.Log("hit the zombie");
                ZombieController enemy = hit.transform.GetComponent<ZombieController>();
                enemy.TakeDamage(damage);
            }
            else if (hit.collider.tag == "Head")
            {
                Debug.Log("HEADSHOT");
                ZombieController enemy = hit.transform.parent.gameObject.transform.GetComponent<ZombieController>();
                enemy.TakeDamage(headshotDamage);
            }
        }
        currentAmmo--;
        isAmmoFull = false;
        if (currentAmmo == 0)
        {
            game.SetReload();
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        game.SetReloading();
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        isAmmoFull = true;
        currentMagazines--;
        game.CloseReload();
    }

    public int getAmmo()
    {
        return currentAmmo;
    }

    public int getMag()
    {
        return currentMagazines * 10;
    }

    public void maximumAmmo()
    {
        currentMagazines = magazines;
    }

    public void upgradeGun()
    {
        damage = damage * 2;
        headshotDamage = headshotDamage * 2;
        magazines = magazines * 2;
        maximumAmmo();
    }

    public void InstantKill()
    {
        var tempDamage = damage;
        damage = 999;
        StartCoroutine(InstantKillDelay(tempDamage));
    }

    private IEnumerator InstantKillDelay(float dam)
    {
        yield return new WaitForSeconds(20f);
        damage = dam;
    }
}


