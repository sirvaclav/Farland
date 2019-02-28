using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Třída používaná pro přeposílání informací o nepříteli mezi scénami. (V Unity, když se přepne scéna, tak se maže vše z paměťi kromě statických věcí)
/// </summary>
public class ConnectionScript : MonoBehaviour {
    
    static EnemyBehaviour eb;
    
    public EnemyBehaviour Eb { get { return _eb; } private set { _eb = value; } }

    private EnemyBehaviour _eb;
    
    public void Fight(EnemyBehaviour enemyBehaviour, GameObject enemy)
    {
        eb = enemyBehaviour;
        DontDestroyOnLoad(enemy);
    }

    public void EndFight(EnemyBehaviour enemyBehaviour)
    {
        eb = enemyBehaviour;
    }

    public void SetProps()
    {
        _eb = eb;
    }
}
