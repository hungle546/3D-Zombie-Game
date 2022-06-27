using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera fpsCam;
    [SerializeField] private RawImage Blood;
    [SerializeField] private CurrencyController currency;
    [SerializeField] private PistolController pistol;
    [SerializeField] private SMGController smg;
    [SerializeField] private ARController ar;
    [SerializeField] private GameObject BuyPrompt;
    [SerializeField] private GameObject pistolObject;
    [SerializeField] private GameObject umpObject;
    [SerializeField] private GameObject arObject;
    private enum weapons {hg, smg, ar};
    private weapons weaponState;
    private TextMeshProUGUI buyPromptText;
    private string buyAmmo = "Press E to buy ammo [Cost: 750]";
    private string buyDoor = "Press E to unlock door [Cost: 1500]";
    private string buyUpgrade = "Press E to upgrade gun [Cost: 5000]";
    private string buyWeapon1 = "Press E to buy weapon [Cost: 2500]";
    private string buyWeapon2 = "Press E to buy weapon [Cost: 3000]";
    private GameController game;
    private float playerHealth = 100;
    private float playerDamage;
    private float currentHealth;
    private GameController healthBar;
    private float hpRegen = 10;
    private bool healed = false;
    private float range = 2f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = playerHealth;
        Blood.enabled = false;
        GameObject gameObj = GameObject.FindGameObjectWithTag("GameController");
        game = gameObj.GetComponent<GameController>();
        buyPromptText = BuyPrompt.GetComponent<TextMeshProUGUI>();
        weaponState = weapons.hg;
        // healthBar.SetMaxHealth(playerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth < 100 && !healed)
        {
            StartCoroutine(healthRegen());
        }
        HoverCheck();
    }

    private void HoverCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            if (hit.transform.tag == "AmmoBox")
            {
                BuyPrompt.SetActive(true);
                buyPromptText.SetText(buyAmmo);
                if (Input.GetKeyDown(KeyCode.E) && currency.getPoints() >= 750)
                {
                    currency.decreasePoints(750);
                    MaxAmmo();
                    Debug.Log("buying ammo");
                }
            }
            else if (hit.transform.tag == "Upgrade")
            {
                BuyPrompt.SetActive(true);
                buyPromptText.SetText(buyUpgrade);
                if (Input.GetKeyDown(KeyCode.E) && currency.getPoints() >= 5000)
                {
                    currency.decreasePoints(5000);
                    UpgradeWeapon();
                    Debug.Log("Upgrading Gun");
                }
            }
            else if (hit.transform.tag == "Door")
            {
                BuyPrompt.SetActive(true);
                buyPromptText.SetText(buyDoor);
                if (Input.GetKeyDown(KeyCode.E) && currency.getPoints() >= 1500)
                {
                    currency.decreasePoints(1500);
                    Destroy(hit.transform.gameObject);
                }
            }
            else if (hit.transform.tag == "UMP")
            {
                BuyPrompt.SetActive(true);
                buyPromptText.SetText(buyWeapon1);
                if (Input.GetKeyDown(KeyCode.E) && currency.getPoints() >= 2500)
                {
                    currency.decreasePoints(2500);
                    Destroy(hit.transform.gameObject);
                    SwitchGuns(1);
                }
            }
            else if (hit.transform.tag == "AK")
            {
                BuyPrompt.SetActive(true);
                buyPromptText.SetText(buyWeapon2);
                if (Input.GetKeyDown(KeyCode.E) && currency.getPoints() >= 3000)
                {
                    currency.decreasePoints(3000);
                    Destroy(hit.transform.gameObject);
                    SwitchGuns(2);
                }
            }
            else
            {
                BuyPrompt.SetActive(false);
            }
        }
    }

    public void takeDamage(int amount)
    {
        currentHealth -= amount;
        Blood.enabled = true;
        //Debug.Log("You got hit health: " + currentHealth);
        if (currentHealth <= 0)
        {
            game.EndGame();
        }
        
    }

    public IEnumerator healthRegen()
    {
        healed = true;
        yield return new WaitForSeconds(3);
        addHealth();
    }

    private void addHealth()
    {
        if (currentHealth <= 90)
        {
            currentHealth += hpRegen;
            Debug.Log("health plus 10");
            healed = false;
        }

        if (currentHealth == 100)
        {
            Debug.Log("fully healed");
            Blood.enabled = false;
        }
    }

    public float getHealth()
    {
        return currentHealth;
    }

    private void SwitchGuns(int type)
    {
        if (type == 1)
        {
            pistolObject.SetActive(false);
            arObject.SetActive(false);
            umpObject.SetActive(true);
            weaponState = weapons.smg;
            game.GunStates(1);
        }
        else if (type == 2)
        {
            pistolObject.SetActive(false);
            umpObject.SetActive(false);
            arObject.SetActive(true);
            weaponState = weapons.ar;
            game.GunStates(2);
        }
    }

    private void MaxAmmo()
    {
        if (weaponState == weapons.hg)
        {
            pistol.maximumAmmo();
        }
        else if (weaponState == weapons.smg)
        {
            smg.maximumAmmo();
        }
        else if (weaponState == weapons.ar)
        {
            ar.maximumAmmo();
        }
    }

    private void UpgradeWeapon()
    {
        if (weaponState == weapons.hg)
        {
            pistol.upgradeGun();
        }
        else if (weaponState == weapons.smg)
        {
            smg.upgradeGun();
        }
        else if (weaponState == weapons.ar)
        {
            ar.upgradeGun();
        }
    }
}
