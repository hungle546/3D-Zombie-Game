using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARController : MonoBehaviour
{
    [SerializeField] private Camera fpsCam;
    private GameController game;
    public float range = 100f;
    private float rateOfFire = 5f;
    private float nextTimeToFire = 0f;
    private float damage = 30;
    private float headshotDamage = 60;
    private AudioSource shotAudio;
    private float reloadTime;
    private int maxAmmo = 30;
    private int currentAmmo;
    private int magazines = 5;
    private int currentMagazines;
    private bool isReloading = false;
    private bool isAmmoFull = true;
    private float currentDamage;
    [SerializeField] public ParticleSystem muzzleflashAR;
    // Start is called before the first frame update
    void Start()
    {
        reloadTime = 2f;
        currentAmmo = maxAmmo;
        currentMagazines = magazines;
        GameObject gameObj = GameObject.FindWithTag("GameController");
        game = gameObj.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo != 0 && !isReloading)
        {
            nextTimeToFire = Time.time + 1f/rateOfFire;
            Shooting();
        }

        if (Input.GetKeyDown("r") && currentMagazines != 0 && !isReloading && !isAmmoFull)
        {
            StartCoroutine(Reload());
        }
        if (Input.GetMouseButtonUp(0))
        {
            shotAudio.Stop();
            Debug.Log("sound stopped");
        }
    }
    
    private void Shooting()
    {
        shotAudio = GetComponent<AudioSource>();
        shotAudio.Play();
        
        RaycastHit hit;
        muzzleflashAR.Play();
       
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            if (hit.collider.tag == "Zombie")
            {
                ZombieController enemy = hit.transform.GetComponent<ZombieController>();
                enemy.TakeDamage(damage);
            }
            else if (hit.collider.tag == "Head")
            {
                ZombieController enemy = hit.transform.parent.gameObject.transform.GetComponent<ZombieController>();
                enemy.TakeDamage(headshotDamage);
            }
        }
        currentAmmo--;
        isAmmoFull = false;
        if (currentAmmo == 0)
        {
            shotAudio.Stop();
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
        return currentMagazines * 30;
    }

    public void maximumAmmo()
    {
        //currentAmmo = maxAmmo;
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
