using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MainCharacter{

    //default things
    public static int PlayerHealth { get; set; }
    public static int PlayerMaxHealth { get; set; }
    public static int PlayerDamage { get; set; }
    public static int PlayerDefendHeal { get; set; }
    public static int PlayerStamina { get; set; }
    public static int PlayerMaxStamina { get; set; }
    public static int StaminaDrain { get; set; }
    public static float PlayerPosX { get; set; }
    public static float PlayerPosY { get; set; }
    public static List<Item> Inventory { get; set; }
    public static List<EnemyBehaviour> Codex { get; set; }
    public static Equipment Equipment { get; set; }
    
    //Level things
    public static int SkillPoints { get; set; }
    public static int CurrentExperience { get; set; }
    public static int ExperienceToLevelUp { get; set; }
    public static int Level { get; set; }

    //Levelable stats
    public static int HealthStat { get; set; }
    public static int DamageStat { get; set; }
    public static int StaminaStat { get; set; }
    public static int IntelligenceStat { get; set; }
}
