using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    //Používané proměnné
    public float speed = 10;
    public float enemySpeed = 10;
    
    private static bool fight = false;

    private float startTime;
    private float journeyLength;
    private float distCovered;
    private float fracJourney;
    private float xDistance;
    private float yDistance;
    
    private GameObject enemy;
    private EnemyBehaviour eb;
    private ConnectionScript cs;

    private Vector2 playerPosition;
    private Vector2 enemyPosition;
    private Vector2 distOffset;
    private Vector2 enemyCurrentPosition;
    private Vector2 targetPosition;

    private Rigidbody2D rb2;
    private SpriteRenderer sr;
    private Animator anim;
    
    

	// Use this for initialization
	void Start () {
        rb2 = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        cs = GetComponent<ConnectionScript>();

        gameObject.transform.position = new Vector3(MainCharacter.PlayerPosX, MainCharacter.PlayerPosY, 0);

        if (fight)
        {
            cs.SetProps();
            eb = cs.Eb;
            enemy = GameObject.Find(eb.enemyName);
            

            if (eb.enemyHealth <= 0)
            {
                MainCharacter.Codex.Add(eb);
                MainCharacter.CurrentExperience += eb.expDrop;
                while(MainCharacter.CurrentExperience >= MainCharacter.ExperienceToLevelUp)
                {
                    LevelUp();
                }
            }

            if (MainCharacter.Codex != null)
            {
                foreach (EnemyBehaviour deadEnemy in MainCharacter.Codex)
                {
                    GameObject.Find(deadEnemy.enemyName).SetActive(false);
                }
            }

            gameObject.transform.position = new Vector2(MainCharacter.PlayerPosX, MainCharacter.PlayerPosY);
            fight = false;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        if(fight)
        {
            distCovered = (Time.time - startTime) * enemySpeed;
            fracJourney = distCovered / journeyLength;

            targetPosition = playerPosition - distOffset;

            enemy.transform.position = Vector2.Lerp(enemyPosition, targetPosition, fracJourney);

            enemyCurrentPosition = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
            
            if(enemyCurrentPosition == targetPosition)
                SceneManager.LoadScene("FightScene");
        }
        else
        {
            anim.SetFloat("moveVertical", moveVertical);
            anim.SetFloat("moveHorizontal", Math.Abs(moveHorizontal));

            if (moveHorizontal < 0)
            {
                sr.flipX = true;
            }
            else if (moveHorizontal > 0)
            {
                sr.flipX = false;
            }

            rb2.velocity = new Vector2(moveHorizontal, moveVertical) * speed;
        }
    }

    /// <summary>
    /// Zavolá se, pokud hráč narazí na nepřítelův collider (zorné pole)
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            enemy = collider.gameObject;
            fight = Fight(enemy);
        }
    }

    /// <summary>
    /// metoda pro obsluhu vstupu do souboje
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    private bool Fight(GameObject enemy)
    {
        anim.SetFloat("moveVertical", 0);
        anim.SetFloat("moveHorizontal", 0);
        rb2.velocity = new Vector2(0, 0);

        eb = enemy.GetComponent<EnemyBehaviour>();

        eb.enemyName = enemy.name;

        playerPosition = gameObject.transform.position;
        enemyPosition = enemy.transform.position;

        xDistance = playerPosition.x - enemyPosition.x;
        yDistance = playerPosition.y - enemyPosition.y;

        if (Math.Abs(xDistance) > Math.Abs(yDistance))
        {
            if (xDistance > 0)
            {
                distOffset = new Vector2(1.5f, 0);
            }
            else
            {
                distOffset = new Vector2(-1.5f, 0);
            }
        }
        else
        {
            if (yDistance > 0)
            {
                distOffset = new Vector2(0, 1.5f);
            }
            else
            {
                distOffset = new Vector2(0, -1.5f);
            }
        }

        startTime = Time.time;

        journeyLength = Vector2.Distance(enemyPosition, playerPosition);

        MainCharacter.PlayerPosX = playerPosition.x;
        MainCharacter.PlayerPosY = playerPosition.y;

        cs.Fight( eb, enemy);
        return true;
    }

    /// <summary>
    /// metoda pro obsluhu levelování
    /// </summary>
    public void LevelUp()
    {
        MainCharacter.SkillPoints++;
        MainCharacter.Level++;

        MainCharacter.CurrentExperience -= MainCharacter.ExperienceToLevelUp;

        MainCharacter.ExperienceToLevelUp += 50;
    }
}
