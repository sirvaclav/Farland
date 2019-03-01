using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour {

    //základní proměnné a linkování věcí ze scény
    public static bool GameIsPaused = false;

    Item clickedItem = new Item();
    EnemyBehaviour clickedEnemy;

    public GameObject PauseMenuUI;
    public GameObject CharacterSheet;
    public GameObject InventorySheet;
    public GameObject InventoryGrid;
    public GameObject CodexSheet;
    public GameObject CodexGrid;

    public Button ItemPrefab;
    public Button EnemyPrefab;

    public Text CategoryText;

    public Text LevelText;
    public Text ExperienceText;
    public Text HealthText;
    public Text StaminaText;
    public Text StaminaDrainText;
    public Text DamageText;
    public Text DefendHealText;

    public Text SkillPointsText;
    public Text HPText;
    public Text DMGText;
    public Text INTText;
    public Text STMText;

    public Button HPButton;
    public Button DMGButton;
    public Button INTButton;
    public Button STMButton;

    public Text itemBonusHealthText;
    public Text itemBonusStaminaText;
    public Text itemBonusDamageText;
    public Text itemDefendHealText;

    public Text enemyHealthText;
    public Text enemyStaminaText;
    public Text enemyDamageText;
    public Text enemyDefendHealText;

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
	}


    /// <summary>
    /// metoda pro znovuspuštění scény
    /// </summary>
    void Resume()
    {
        clickedItem = new Item();
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    /// <summary>
    /// metoda pro pozastavení scény
    /// </summary>
    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        CharacterButton();
    }

    /// <summary>
    /// Metoda pro zobrazení informací o hrdinovi
    /// </summary>
    public void CharacterButton()
    {
        CategoryText.text = "Character";

        CharacterSheet.SetActive(true);
        InventorySheet.SetActive(false);
        CodexSheet.SetActive(false);

        LevelText.text = MainCharacter.Level.ToString();
        ExperienceText.text = MainCharacter.CurrentExperience + "/" + MainCharacter.ExperienceToLevelUp;
        HealthText.text = MainCharacter.PlayerHealth + "/" + MainCharacter.PlayerMaxHealth;
        StaminaText.text = MainCharacter.PlayerMaxStamina.ToString();
        StaminaDrainText.text = MainCharacter.StaminaDrain.ToString();
        DamageText.text = MainCharacter.PlayerDamage.ToString();
        DefendHealText.text = MainCharacter.PlayerDefendHeal.ToString();

        SkillPointsText.text = MainCharacter.SkillPoints.ToString();
        HPText.text = MainCharacter.HealthStat.ToString();
        DMGText.text = MainCharacter.DamageStat.ToString();
        INTText.text = MainCharacter.IntelligenceStat.ToString();
        STMText.text = MainCharacter.StaminaStat.ToString();

        if(MainCharacter.SkillPoints > 0)
        {
            HPButton.interactable = true;
            DMGButton.interactable = true;
            INTButton.interactable = true;
            STMButton.interactable = true;
        }
        else
        {
            HPButton.interactable = false;
            DMGButton.interactable = false;
            INTButton.interactable = false;
            STMButton.interactable = false;
        }
    }

    /// <summary>
    /// Metoda pro vylepšování hráčových vlastností
    /// </summary>
    /// <param name="text"></param>
    public void UpgradeStat(Text text)
    {
        switch(text.text)
        {
            case "HP:": MainCharacter.PlayerMaxHealth+=5; MainCharacter.HealthStat++; break;
            case "DMG:": MainCharacter.PlayerDamage+=2; MainCharacter.DamageStat++; break;
            case "INT:": MainCharacter.IntelligenceStat += 1; break;
            case "STM:": MainCharacter.PlayerMaxStamina+=5; MainCharacter.StaminaStat++; break;
            default:break;
        }

        MainCharacter.SkillPoints--;
        CharacterButton();
    }


    /// <summary>
    /// Metoda pro zobrazení hráčova inventáře
    /// </summary>
    public void InventoryButton()
    {
        CategoryText.text = "Inventory";

        CharacterSheet.SetActive(false);
        InventorySheet.SetActive(true);
        CodexSheet.SetActive(false);

        foreach (Transform child in InventoryGrid.transform)
        {
            Destroy(child.gameObject);
        }

        if(MainCharacter.Inventory != null)
        {
            foreach(Item item in MainCharacter.Inventory)
            {
                Button newItem = Instantiate(ItemPrefab);
                newItem.GetComponentInChildren<Text>().text = item.Name;
                newItem.transform.SetParent(InventoryGrid.transform);
                newItem.onClick.AddListener(() => ItemClick(item.Name));
            }
        }
    }


    /// <summary>
    /// metoda pro zobrazení informací o itemu
    /// </summary>
    /// <param name="itemName"></param>
    public void ItemClick(string itemName)
    {
        foreach(Item item in MainCharacter.Inventory)
        {
            if(item.Name == itemName)
            {
                clickedItem = item;
                break;
            }
        }

        itemBonusHealthText.text = clickedItem.BonusHealth.ToString();
        itemBonusStaminaText.text = clickedItem.BonusStamina.ToString();
        itemBonusDamageText.text = clickedItem.BonusDamage.ToString();
        itemDefendHealText.text = clickedItem.DefendHeal.ToString();
    }

    /// <summary>
    /// metoda pro vyzbrojení se itemem
    /// </summary>
    public void EquipItem()
    {
        if (clickedItem != null)
        {
            switch (clickedItem.Category)
            {
                case "helmet":
                    RemoveEquipmentBonuses();
                    MainCharacter.Equipment.Helm = clickedItem;
                    break;
                case "chestplate":
                    RemoveEquipmentBonuses();
                    MainCharacter.Equipment.Armour = clickedItem;
                    break;
                case "gauntlet":
                    RemoveEquipmentBonuses();
                    MainCharacter.Equipment.Gauntlets = clickedItem;
                    break;
                case "weapon":
                    RemoveEquipmentBonuses();
                    MainCharacter.Equipment.Weapon = clickedItem;
                    break;
                default: break;
            }

            CalculateEquipmentBonuses();
        }
    }

    /// <summary>
    /// Metoda pro odstranění bonusů z vybavení
    /// </summary>
    private void RemoveEquipmentBonuses()
    {
        Item helmet = MainCharacter.Equipment.Helm;
        Item armour = MainCharacter.Equipment.Armour;
        Item greaves = MainCharacter.Equipment.Gauntlets;
        Item weapon = MainCharacter.Equipment.Weapon;

        MainCharacter.PlayerMaxHealth -= (helmet.BonusHealth + armour.BonusHealth + greaves.BonusHealth + weapon.BonusHealth);
        MainCharacter.PlayerDefendHeal -= (helmet.DefendHeal + armour.DefendHeal + greaves.DefendHeal + weapon.DefendHeal);
        MainCharacter.PlayerMaxStamina -= (helmet.BonusStamina + armour.BonusStamina + greaves.BonusStamina + weapon.BonusStamina);
        MainCharacter.PlayerDamage -= (helmet.BonusDamage + armour.BonusDamage + greaves.BonusDamage + weapon.BonusDamage);
    }

    /// <summary>
    /// Metoda pro vypočítávání bonusů z vybavení
    /// </summary>
    private void CalculateEquipmentBonuses()
    {
        Item helmet = MainCharacter.Equipment.Helm;
        Item armour = MainCharacter.Equipment.Armour;
        Item greaves = MainCharacter.Equipment.Gauntlets;
        Item weapon = MainCharacter.Equipment.Weapon;

        MainCharacter.PlayerMaxHealth += helmet.BonusHealth + armour.BonusHealth + greaves.BonusHealth + weapon.BonusHealth;
        MainCharacter.PlayerDefendHeal += helmet.DefendHeal + armour.DefendHeal + greaves.DefendHeal + weapon.DefendHeal;
        MainCharacter.PlayerMaxStamina += helmet.BonusStamina + armour.BonusStamina + greaves.BonusStamina + weapon.BonusStamina;
        MainCharacter.PlayerDamage += helmet.BonusDamage + armour.BonusDamage + greaves.BonusDamage + weapon.BonusDamage;
    }

    /// <summary>
    /// Metoda pro vyobrazení zabitých nestvůr
    /// </summary>
    public void CodexButton()
    {
        CategoryText.text = "Codex";

        CharacterSheet.SetActive(false);
        InventorySheet.SetActive(false);
        CodexSheet.SetActive(true);

        foreach (Transform child in CodexGrid.transform)
        {
            Destroy(child.gameObject);
        }

        if (MainCharacter.Codex != null)
        {
            foreach (EnemyBehaviour enemy in MainCharacter.Codex)
            {
                Button newEnemy = Instantiate(EnemyPrefab);

                string eName = TrimGameObjectName(enemy.enemyName);

                newEnemy.GetComponentInChildren<Text>().text = eName;
                newEnemy.transform.SetParent(CodexGrid.transform);
                newEnemy.onClick.AddListener(() => EnemyClick(eName));
            }
        }
    }

    /// <summary>
    /// Metoda pro zobrazení informací o nepříteli
    /// </summary>
    /// <param name="enemyName"></param>
    public void EnemyClick(string enemyName)
    {
        foreach (EnemyBehaviour enemy in MainCharacter.Codex)
        {
            string eName = TrimGameObjectName(enemy.enemyName);

            if (eName == enemyName)
            {
                clickedEnemy = enemy;
                break;
            }
        }

        enemyHealthText.text = clickedEnemy.enemyMaxHealth.ToString();
        enemyStaminaText.text = clickedEnemy.enemyMaxStamina.ToString();
        enemyDamageText.text = clickedEnemy.enemyDamage.ToString();
        enemyDefendHealText.text = clickedEnemy.enemyDefendHeal.ToString();
    }

    /// <summary>
    /// Metoda pro odstranění počitadla ze jména
    /// </summary>
    /// <param name="enemyName"></param>
    /// <returns></returns>
    private string TrimGameObjectName(string enemyName)
    {
        int index = enemyName.IndexOf('(');
        if (index > 0)
        {
            enemyName = enemyName.Remove(index - 1);
        }

        return enemyName;
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
