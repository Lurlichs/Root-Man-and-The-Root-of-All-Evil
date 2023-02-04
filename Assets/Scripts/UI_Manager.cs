using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;
    [SerializeField] private Player player;
    [SerializeField] private GameManager gameManager;

    [Header("Health")]
    [SerializeField] private GameObject healthLeaf1;
    [SerializeField] private GameObject healthLeaf2;
    [SerializeField] private GameObject healthLeaf3;
    [SerializeField] private GameObject healthLeaf4;
    [SerializeField] private GameObject healthLeaf5;

    [Header("On Screen Abilities")]
    [SerializeField] private GameObject onScreenRegenX;
    [SerializeField] private GameObject onScreenProjectileX;
    [SerializeField] private GameObject onScreenDoubleJumpX;
    [SerializeField] private GameObject onScreenRootWaveX;
    [SerializeField] private GameObject onScreenShieldX;

    [Header("Boss Bar")]
    [SerializeField] private CanvasGroup bossBarCG;
    [SerializeField] private Text bossTitle;
    [SerializeField] private Slider bossHealthSlider;
    private bool fadeBossBarNow = false;

    [Header("Victory panel")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject regenX;
    [SerializeField] private GameObject projectileX;
    [SerializeField] private GameObject doubleJumpX;
    [SerializeField] private GameObject rootWaveX;
    [SerializeField] private GameObject shieldX;

    [Header("Pause panel")]
    private bool isPaused;
    [SerializeField] private GameObject pausePanel;

    [Header("Title Screen")]
    [SerializeField] private GameObject titleUI;

    [Header("Death Panel")]
    public GameObject deathPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (fadeBossBarNow == true && bossBarCG.alpha < 1)
        {
            bossBarCG.alpha += 0.6f * Time.deltaTime;
        }

        if (fadeBossBarNow == false && bossBarCG.alpha >= 0)
        {
            bossBarCG.alpha -= 0.5f * Time.deltaTime;
        }

        PauseResumeToggle();
    }

    #region helth
    public void UpdateHealthSetup()
    {
        if (player.currentHealth == 5)
        {
            healthLeaf5.SetActive(true);
            healthLeaf4.SetActive(true);
            healthLeaf3.SetActive(true);
            healthLeaf2.SetActive(true);
            healthLeaf1.SetActive(true);
        }
        if (player.currentHealth == 4)
        {
            healthLeaf5.SetActive(false);
            healthLeaf4.SetActive(true);
            healthLeaf3.SetActive(true);
            healthLeaf2.SetActive(true);
            healthLeaf1.SetActive(true);
        }
        if (player.currentHealth == 3)
        {
            healthLeaf5.SetActive(false);
            healthLeaf4.SetActive(false);
            healthLeaf3.SetActive(true);
            healthLeaf2.SetActive(true);
            healthLeaf1.SetActive(true);
        }
        if (player.currentHealth == 2)
        {
            healthLeaf5.SetActive(false);
            healthLeaf4.SetActive(false);
            healthLeaf3.SetActive(false);
            healthLeaf2.SetActive(true);
            healthLeaf1.SetActive(true);
        }
        if (player.currentHealth == 1)
        {
            healthLeaf5.SetActive(false);
            healthLeaf4.SetActive(false);
            healthLeaf3.SetActive(false);
            healthLeaf2.SetActive(false);
            healthLeaf1.SetActive(true);
        }
        if (player.currentHealth == 0)
        {
            healthLeaf5.SetActive(false);
            healthLeaf4.SetActive(false);
            healthLeaf3.SetActive(false);
            healthLeaf2.SetActive(false);
            healthLeaf1.SetActive(false);
        }
    }
    #endregion

    public void TurnOnVictoryPanel()
    {
        victoryPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void TurnOffVictoryPanel()
    {
        victoryPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        gameManager.StartNextStage();
    }

    #region remove ability button functions
    public void RemoveRegen()
    {
        regenX.SetActive(true);
        onScreenRegenX.SetActive(true);
        player.DisablePower(0);
        TurnOffVictoryPanel();
    }

    public void RemoveProjectile()
    {
        projectileX.SetActive(true);
        onScreenProjectileX.SetActive(true);
        player.DisablePower(1);
        TurnOffVictoryPanel();
    }

    public void RemoveDoubleJump()
    {
        doubleJumpX.SetActive(true);
        onScreenDoubleJumpX.SetActive(true);
        player.DisablePower(2);
        TurnOffVictoryPanel();
    }

    public void RemoveRootWave()
    {
        rootWaveX.SetActive(true);
        onScreenRootWaveX.SetActive(true);
        player.DisablePower(3);
        TurnOffVictoryPanel();
    }

    public void RemoveShield()
    {
        shieldX.SetActive(true);
        onScreenShieldX.SetActive(true);
        player.DisablePower(4);
        TurnOffVictoryPanel();
    }
    #endregion

    #region Boss Health bar display
    public void SetBossHealthBar(Bosses_ScriptableObj boss)
    {
        bossTitle.text = boss.bossName;
        bossHealthSlider.maxValue = boss.baseHealth;
        bossHealthSlider.value = boss.baseHealth;
        fadeBossBarNow = true;
    }

    public void UpdateBossHealth(float cHealth)
    {
        bossHealthSlider.value = cHealth;
    }

    public void FadeOutBossBar()
    {
        fadeBossBarNow = false;
    }
    #endregion

    public void PauseResumeToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                isPaused = true;
                Pause();
            }
            else
            {
                isPaused = false;
                Resume();
            }
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;

    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        titleUI.SetActive(false);
    }


    public void TurnOnDeathScreen()
    {
        
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        deathPanel.SetActive(true);
    }

    public void Reload()
    {
        Time.timeScale = 1f;
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }
}
