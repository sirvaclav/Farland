using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {
    public void StartGame()
    {
        //character inicialization
        MainCharacter.PlayerMaxHealth = 100;
        MainCharacter.PlayerHealth = 100;
        MainCharacter.PlayerDamage = 10;
        MainCharacter.PlayerDefendHeal = 5;
        MainCharacter.PlayerMaxStamina = 100;
        MainCharacter.PlayerStamina = 100;
        MainCharacter.StaminaDrain = 10;
        MainCharacter.Inventory = new List<Item>();
        MainCharacter.Codex = new List<EnemyBehaviour>();

        MainCharacter.CurrentExperience = 0;
        MainCharacter.ExperienceToLevelUp = 100;
        MainCharacter.Level = 0;
        MainCharacter.SkillPoints = 0;

        MainCharacter.HealthStat = 0;
        MainCharacter.DamageStat = 0;
        MainCharacter.StaminaStat = 0;
        MainCharacter.IntelligenceStat = 0;

        MainCharacter.Equipment = new Equipment
        {
            Helm = new Item(),
            Gauntlets = new Item(),
            Armour = new Item(),
            Weapon = new Item()
        };


        SceneManager.LoadScene("World");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
