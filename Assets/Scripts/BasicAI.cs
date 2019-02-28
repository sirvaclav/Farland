using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Třída pro jednoduchou inteligenci nepřátel, podle podmínek přidává prioritu bránění nebo útočení
/// </summary>
public class BasicAI {
    public int Attack(int playerHealth, int enemyHealth, int enemyStamina, int enemyStaminaDrain, int enemyDamage)
    {
        //max priority = 10;
        int priority = 5;

        if (enemyStamina < enemyStaminaDrain)
        {
            priority = 0;
            return priority;
        }
        if (playerHealth - enemyDamage <= 0)
        {
            priority = 10;
            return priority;
        }
        if (enemyHealth > 0.4 * enemyHealth)
            priority++;
        else
            priority--;
        return priority;
    }

    public int Defend(int atcPriority)
    {
        //max priority = 10;
        int priority = 5;
        
        if(atcPriority == 0)
        {
            priority = 10;
            return priority;
        }
        
        return priority;
    }
}
