using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Třída pro udávání vlastností nepřítele
/// </summary>
public class EnemyBehaviour : MonoBehaviour{
    public int enemyMaxHealth = 100;
    public int enemyHealth = 100;
    public int enemyDamage = 10;
    public string enemyName { get; set; }
    public int enemyDefendHeal = 5;
    public int enemyMaxStamina = 50;
    public int enemyStamina = 50;
    public int enemyStaminaDrain = 5;
    public int expDrop = 51;
    public string fileName = "";
}


