using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private PistolController pistol;
    [SerializeField] private SMGController smg;
    [SerializeField] private ARController ar;
    [SerializeField] private TextMeshProUGUI Ammo;
    [SerializeField] private TextMeshProUGUI Currency;
    [SerializeField] private GameObject reloadTextObj;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject tryAgainButton;
    [SerializeField] private GameObject ResumeButton;
    [SerializeField] private GameObject PauseText;
    [SerializeField] private GameObject MenuButton;
    [SerializeField] private GameObject ExitButton;
    [SerializeField] private GameObject VolumeSlider;
    [SerializeField] private GameObject VolumeName;
    [SerializeField] private GameObject VolumeNumber;
    [SerializeField] private GameObject VolumeSaveButton;
    [SerializeField] private GameObject machine;
    private TextMeshProUGUI reloadText;
    public Slider slider;
    private CurrencyController Points;
    private string reloadString = "R - Reload";
    private string reloadingString = "Reloading ...";
    public static bool gameIsPaused;
    private int upgradeCollected;
    private bool isCollected;
    [SerializeField] private TextMeshProUGUI upgradeText;
    private string msgPrefix = "Upgrade Parts: ";
    private int gunState = 0;
    private bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        GameObject currencyObj = GameObject.FindGameObjectWithTag("Currency");
        player = playerObj.GetComponent<PlayerController>();
        Points = currencyObj.GetComponent<CurrencyController>();
        reloadText = reloadTextObj.GetComponent<TextMeshProUGUI>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isCollected = false;
        upgradeCollected = 0;
        upgradeText.SetText(msgPrefix);
        gunState = 0;
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        SetHealth();
        Ammo.SetText(GetAmmo());
        Currency.SetText("Points: " + Points.getPoints());
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            gameIsPaused = true;
            PauseGame();
        }
    }

    public void SetHealth()
    {
       slider.value = player.getHealth();
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void CollectedItem()
    {
        isCollected = true; 
        upgradeCollected += 1;
        upgradeText.SetText(msgPrefix+upgradeCollected);
        if (upgradeCollected == 3)
        {
            SpawnUpgrader();
            upgradeText.SetText("Upgrade Machine Completed");
            Debug.Log("upgrade machien spawned");
        }
        
    }

    public void SetReload()
    {
        reloadTextObj.SetActive(true);
        reloadText.SetText(reloadString);
    }

    public void SetReloading()
    {
        reloadTextObj.SetActive(true);
        reloadText.SetText(reloadingString);
    }

    public void CloseReload()
    {
        reloadTextObj.SetActive(false);
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        gameOverText.SetActive(true);
        tryAgainButton.SetActive(true);
        isGameOver = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void PauseGame ()
        {
            if(gameIsPaused)
            {
                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                AudioListener.pause = true;
                ResumeButton.SetActive(true);
                PauseText.SetActive(true);
                MenuButton.SetActive(true);
                ExitButton.SetActive(true);
                VolumeName.SetActive(true);
                VolumeNumber.SetActive(true);
                VolumeSlider.SetActive(true);
                VolumeSaveButton.SetActive(true);
            }
            else 
            {
                Time.timeScale = 1;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                AudioListener.pause = false;
                ResumeButton.SetActive(false);
                PauseText.SetActive(false);
                MenuButton.SetActive(false);
                ExitButton.SetActive(false);
                VolumeName.SetActive(false);
                VolumeNumber.SetActive(false);
                VolumeSlider.SetActive(false);
                VolumeSaveButton.SetActive(false);
            }
        }

    private void SpawnUpgrader()
    {
        Instantiate(machine, new Vector3(82,36,-42), Quaternion.identity);
    }

    public void GunStates(int state)
    {
        if (state == 0)
        {
            gunState = 0;
        }
        else if (state == 1)
        {
            gunState = 1;
        }
        else if (state == 2)
        {
            gunState = 2;
        }
    }

    private string GetAmmo()
    {
        if (gunState == 0)
        {
            return pistol.getAmmo() + "/" + pistol.getMag();
        }
        else if (gunState == 1)
        {
            return smg.getAmmo() + "/" + smg.getMag();
        }
        else
        {
            return ar.getAmmo() + "/" + ar.getMag();
        }
    }
    
    public void MaxAmmo()
    {
        if (gunState == 0)
        {
            pistol.maximumAmmo();
        }
        else if (gunState == 1)
        {
            smg.maximumAmmo();
        }
        else
        {
            ar.maximumAmmo();
        }
    }

    public void InstantKill()
    {
        if (gunState == 0)
        {
            pistol.InstantKill();
        }
        else if (gunState == 1)
        {
            smg.InstantKill();
        }
        else
        {
            ar.InstantKill();
        }
    }

    public void Resume()
    {
        gameIsPaused = false;
        PauseGame();
    }
   
}
