using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;

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
    }

    public void UpdateHealthSetup()
    {
        //turn off the corresponding leaf
    }

    public void TurnOnVictoryPanel()
    {
        victoryPanel.SetActive(true);
    }

    #region remove ability button functions
    public void RemoveRegen()
    {
        regenX.SetActive(true);
        onScreenRegenX.SetActive(true);
        // remove regen for the eplayer
    }

    public void RemoveProjectile()
    {
        projectileX.SetActive(true);
        onScreenProjectileX.SetActive(true);
        // remove projectile for thee player
    }

    public void removeDoubleJump()
    {
        doubleJumpX.SetActive(true);
        onScreenDoubleJumpX.SetActive(true);
        // remove double jump for the eplayer
    }

    public void RemoveRootWave()
    {
        rootWaveX.SetActive(true);
        onScreenRootWaveX.SetActive(true);
        // remove root wave for thee player
    }

    public void RemoveShield()
    {
        shieldX.SetActive(true);
        onScreenShieldX.SetActive(true);
        // remove shield for thee player
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
}
