using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public PlayerController playerController;
    public HealthBarController healthBarController;
    public Button skipButton;

    [Header("Upgrade Amounts")]
    public int playerAttackDamageUpgradeAmount;
    public int playerMaxHealthUpgradeAmount;
    public int playerRegenerationUpgradeAmount;
    public float playerMovementSpeedUpgradeAmount;
    public float playerFireRateUpgradeAmount;
    public float playerBulletSpeedUpgradeAmount;

    [System.Serializable]
    public class UpgradeOption
    {
        public string name;
        public string description;
        public Sprite icon;
        public int cost;
        public System.Action effect;
    }

    [Header("Upgrade Options")]
    public List<UpgradeOption> allUpgrades;
    public GameObject upgradePanel;

    [Header("Upgrade Slots")]
    public GameObject[] upgradeSlots;
    public TMP_Text[] upgradeNames;
    public Image[] upgradeImages;
    public TMP_Text[] upgradeDescriptions;
    public TMP_Text[] upgradeCosts;
    public Button[] upgradeButtons;


    private void Awake()
    {
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<PlayerStats>();
        }
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }
        if (healthBarController == null)
        {
            healthBarController = FindObjectOfType<HealthBarController>();
        }
    }


    void Start()
    {
        upgradePanel.SetActive(false);

        allUpgrades = new List<UpgradeOption>
        {
            new UpgradeOption
            {
                name = "Attack Damage",
                description = "Increases Player Attack Damage: " + playerAttackDamageUpgradeAmount,
                icon = Resources.Load<Sprite>("Icons/AttackDamageIcon"),
                cost = 75,
                effect = () => playerStats.playerAttackDamage += playerAttackDamageUpgradeAmount
            },
            new UpgradeOption
            {
                name = "Max Health",
                description = "Increases Player Max Health: " + playerMaxHealthUpgradeAmount + " and heals 15 health instantly",
                cost = 50,
                icon = Resources.Load<Sprite>("Icons/MaxHealthIcon"),
                effect = () =>
                {
                playerStats.playerMaxHealth += playerMaxHealthUpgradeAmount;
                playerStats.playerCurrentHealth += 15;
                healthBarController.UpdateBar();
                }
            },
            new UpgradeOption
            {
                name = "Regeneration",
                description = "Regenerate " + playerRegenerationUpgradeAmount + " Health Instantly",
                icon = Resources.Load<Sprite>("Icons/RegenerationIcon"),
                cost = 75,
                effect = () =>
                {
                    if(playerStats.playerCurrentHealth + playerRegenerationUpgradeAmount > playerStats.playerMaxHealth)
                        playerStats.playerCurrentHealth = playerStats.playerMaxHealth;
                    else
                    playerStats.playerCurrentHealth += playerRegenerationUpgradeAmount;
                    healthBarController.UpdateBar();
                }
            },
            new UpgradeOption
            {
                name = "Movement Speed",
                description = "Increases Player Movement Speed: " + playerMovementSpeedUpgradeAmount,
                icon = Resources.Load<Sprite>("Icons/MovementSpeedIcon"),
                cost = 75,
                effect = () => playerStats.playerMovementSpeed += playerMovementSpeedUpgradeAmount
            },
            new UpgradeOption
            {
                name = "Attack Speed",
                description = "Increases Player Attack Speed: " + playerFireRateUpgradeAmount,
                icon = Resources.Load<Sprite>("Icons/AttackSpeedIcon"),
                cost = 50,
                effect = () => playerStats.playerFireRate -= playerFireRateUpgradeAmount
            },
            new UpgradeOption
            {
                name = "Bullet Speed",
                description = "Increases player bullet speed: " + playerBulletSpeedUpgradeAmount,
                icon = Resources.Load<Sprite>("Icons/BulletSpeedIcon"),
                cost = 50,
                effect = () => playerStats.playerBulletSpeed += playerBulletSpeedUpgradeAmount
            },
        };
    }

    public void ShowUpgradeOptions()
    {
        Time.timeScale = 0;
        upgradePanel.SetActive(true);

        List<UpgradeOption> randomUpgrades = GetRandomUpgrades(upgradeSlots.Length);

        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            UpgradeOption upgrade = randomUpgrades[i];

            upgradeNames[i].text = upgrade.name;
            upgradeImages[i].sprite = upgrade.icon;
            upgradeDescriptions[i].text = upgrade.description;
            upgradeCosts[i].text = "Cost: " + upgrade.cost.ToString() + "Score";

            Button button = upgradeButtons[i];
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                if (playerStats.score >= upgrade.cost)
                {
                    playerStats.score -= upgrade.cost;
                    ApplyUpgrade(upgrade);
                    upgradePanel.SetActive(false);
                    Time.timeScale = 1;
                }
            });
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(() =>
            {
                upgradePanel.SetActive(false);
                Time.timeScale = 1;
            });

        }
    }

    public void ApplyUpgrade(UpgradeOption upgrade)
    {
        Debug.Log("Applied upgrade: " + upgrade.name);
        upgrade.effect?.Invoke();
    }

//
    private List<UpgradeOption> GetRandomUpgrades(int count)
    {
        List<UpgradeOption> randomUpgrades = new List<UpgradeOption>();
        List<UpgradeOption> availableUpgrades = new List<UpgradeOption>(allUpgrades);

        for (int i = 0; i < count; i++)
        {
            if (availableUpgrades.Count == 0) break;

            int randomIndex = Random.Range(0, availableUpgrades.Count);
            randomUpgrades.Add(availableUpgrades[randomIndex]);
            availableUpgrades.RemoveAt(randomIndex);
        }

        return randomUpgrades;
    }

}
