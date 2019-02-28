using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FightController : MonoBehaviour {
    
    //základní proměnné a linkování Objektů ze scény
    private int playerHealth;
    private int playerDamage;
    private int playerDefendHeal;
    private bool playerPhase = true;
    private List<string> castedElements = new List<string>(1); //Abychom nemuseli implementovat tolik kouzel do hry, tak je velikost pole malá. Počet kouzel = 4^velikost pole
    private List<Spell> castableSpells = new List<Spell>();
    private int playerMaxHealth;
    private int playerMaxStamina;
    private int playerStamina;
    private int playerStaminaDrain;


    private int enemyHealth;
    private int enemyMaxHealth;
    private int enemyStamina;
    private int enemyMaxStamina;
    private int enemyStaminaDrain;
    private int enemyDamage;
    private int enemyDefendHeal;
    private string enemyDropList;

    public Button attackButton;
    public Button defendButton;
    public Button magicButton;
    public Button escapeButton;
    public Button castMagicButton;
    public Button cancelMagicButton;
    public Button fireMagicButton;
    public Button waterMagicButton;
    public Button earthMagicButton;
    public Button windMagicButton;
    public Button endTurnButton;

    public Image ElementImage1;
    public Image ElementImage2;
    public Image ElementImage3;
    public Image ElementImage4;
    
    public Image playerHealthBarContent;
    public Image playerStaminaBarContent;
    public Image enemyHealthBarContent;
    public Image enemyStaminaBarContent;

    private ConnectionScript cs;

    private GameObject enemy;
    
    private EnemyBehaviour eb;
    private BasicAI basicAI;

    private XmlLoader xmlLoader;


    //Spustí se při renderu gameObjectu
    void Start () {
        cs = GetComponent<ConnectionScript>();
        basicAI = new BasicAI();
        xmlLoader = new XmlLoader();

        cs.SetProps();
              
        eb = cs.Eb;
        
        playerHealth = MainCharacter.PlayerHealth;
        playerMaxHealth = MainCharacter.PlayerMaxHealth;
        playerStamina = MainCharacter.PlayerStamina;
        playerStaminaDrain = MainCharacter.StaminaDrain;
        playerMaxStamina = MainCharacter.PlayerMaxStamina;
        playerDamage = MainCharacter.PlayerDamage;
        playerDefendHeal = MainCharacter.PlayerDefendHeal;

        enemyHealth = eb.enemyHealth;
        enemyMaxHealth = eb.enemyMaxHealth;
        enemyStamina = eb.enemyStamina;
        enemyMaxStamina = eb.enemyMaxStamina;
        enemyStaminaDrain = eb.enemyStaminaDrain;
        enemyDamage = eb.enemyDamage;
        enemyDropList = eb.fileName;
        enemyDefendHeal = eb.enemyDefendHeal;

        UpdateBars();

        enemy = GameObject.Find(cs.Eb.enemyName);
        enemy.transform.position = new Vector3(4f, 3.5f, -1);
        enemy.transform.localScale = new Vector3 (0.1f, 0.1f, 1);
    }

    /// <summary>
    /// metoda pro aktualizování počítadel
    /// </summary>
    private void UpdateBars()
    {
        playerHealthBarContent.fillAmount = MapValuesToBars(playerHealth, playerMaxHealth);
        playerStaminaBarContent.fillAmount = MapValuesToBars(playerStamina, playerMaxStamina);

        enemyHealthBarContent.fillAmount = MapValuesToBars(enemyHealth, enemyMaxHealth);
        enemyStaminaBarContent.fillAmount = MapValuesToBars(enemyStamina, enemyMaxStamina);
    }

    /// <summary>
    /// Metoda pro linkování dat do počítadel
    /// </summary>
    /// <param name="value">minimální hodnota vlastnosti</param>
    /// <param name="inMax">maximální hodnota vlastnosti</param>
    /// <returns></returns>
    private float MapValuesToBars(float value, float inMax)
    {
        return value / inMax;
    }

    /// <summary>
    /// Metoda pro útok
    /// </summary>
    /// <param name="playerPhase">parametr jestli je na tahu hráč nebo nepřítel</param>
    private void Attack(bool playerPhase)
    {
        if(playerPhase)
        {
            enemyHealth -= playerDamage;

            playerStamina -= playerStaminaDrain;

            UpdateBars();
                        
            if (enemyHealth <= 0)
                EnemyDeath();
            else
            {
                EnemyPhase();
            }
        }
        else
        {
            playerHealth -= enemyDamage;

            enemyStamina -= enemyStaminaDrain;

            UpdateBars();
            
            if (playerHealth <= 0)
                PlayerDeath();
        }
    }

    /// <summary>
    /// Metoda pro bránění se
    /// </summary>
    /// <param name="playerPhase">parametr jestli je na tahu hráč nebo nepřítel</param>
    private void Defend(bool playerPhase)
    {
        if(playerPhase)
        {
            playerHealth += playerDefendHeal;
            playerStamina += 2 * playerStaminaDrain;

            UpdateBars();

            if (playerHealth > playerMaxHealth)
                playerHealth = playerMaxHealth;
            if (playerStamina > playerMaxStamina)
                playerStamina = playerMaxStamina;
            EnemyPhase();
        }
        else
        {
            enemyHealth += enemyDefendHeal;
            enemyStamina += 2 * enemyStaminaDrain;

            UpdateBars();
        }
    }

    /// <summary>
    /// metoda pro vykouzlení elementu kouzel
    /// </summary>
    /// <param name="element">druh elementu</param>
    private void CastElement(string element)
    {
        castedElements.Add(element);

        switch(castedElements.Count)
        {
            case 1:
                DetermineAnimation(element, ElementImage1);
                break;
            case 2:
                DetermineAnimation(element, ElementImage2);
                break;
            case 3:
                DetermineAnimation(element, ElementImage3);
                break;
            case 4:
                DetermineAnimation(element, ElementImage4);
                break;
        }

        if(castedElements.Count == 1)
        {
            castableSpells = xmlLoader.LoadSpellsFromXML(element);
        }
    }

    /// <summary>
    /// Podle vykouzleného elementu určí, co se zobrazí za animaci
    /// </summary>
    /// <param name="element"></param>
    /// <param name="elementImage"></param>
    private void DetermineAnimation(string element, Image elementImage)
    {
        elementImage.gameObject.SetActive(true);

        switch(element)
        {
            case "earth":
                elementImage.GetComponent<Animator>().SetBool("IsEarth", true);
                elementImage.GetComponent<Animator>().SetBool("IsWind", false);
                elementImage.GetComponent<Animator>().SetBool("IsWater", false);
                elementImage.GetComponent<Animator>().SetBool("IsFire", false);
                break;
            case "fire":
                elementImage.GetComponent<Animator>().SetBool("IsEarth", false);
                elementImage.GetComponent<Animator>().SetBool("IsWind", false);
                elementImage.GetComponent<Animator>().SetBool("IsWater", false);
                elementImage.GetComponent<Animator>().SetBool("IsFire", true);
                break;
            case "wind":
                elementImage.GetComponent<Animator>().SetBool("IsEarth", false);
                elementImage.GetComponent<Animator>().SetBool("IsWind", true);
                elementImage.GetComponent<Animator>().SetBool("IsWater", false);
                elementImage.GetComponent<Animator>().SetBool("IsFire", false);
                break;
            case "water":
                elementImage.GetComponent<Animator>().SetBool("IsEarth", false);
                elementImage.GetComponent<Animator>().SetBool("IsWind", false);
                elementImage.GetComponent<Animator>().SetBool("IsWater", true);
                elementImage.GetComponent<Animator>().SetBool("IsFire", false);
                break;
            default: break;
        }
    }


    /// <summary>
    /// Metoda pro seslání kouzla
    /// </summary>
    private void CastSpell()
    {
        IEnumerable<string> findingSpell;

        foreach (Spell spell in castableSpells)
        {
            findingSpell = spell.Elements.Except(castedElements);

            if(findingSpell.Count() == 0)
            {
                Debug.Log(spell.Name);

                enemyHealth -= spell.Damage;

                UpdateBars();

                castedElements.Clear();
                castableSpells.Clear();

                ElementImage1.gameObject.SetActive(false);
                ElementImage2.gameObject.SetActive(false);
                ElementImage3.gameObject.SetActive(false);
                ElementImage4.gameObject.SetActive(false);

                EnemyPhase();
            }
        }
    }

    /// <summary>
    /// Metoda pro únik
    /// </summary>
    private void Escape()
    {
        Destroy(enemy);

        MainCharacter.PlayerHealth = playerHealth;
        MainCharacter.PlayerPosX = 0;
        MainCharacter.PlayerPosY = 0;

        eb.enemyHealth = enemyHealth;

        cs.EndFight( eb);

        SceneManager.LoadScene("World");
    }

    /// <summary>
    /// Metoda pro ukončení kola
    /// </summary>
    private void EndTurn()
    {
        EnemyPhase();
    }

    /// <summary>
    /// Metoda pro obsluhu nepřítelova kola
    /// </summary>
    private void EnemyPhase()
    {
        playerPhase = false;
        DisableButtons();

        int atcPriority = basicAI.Attack(playerHealth, enemyHealth, enemyStamina, enemyStaminaDrain, enemyDamage);
        int defPriority = basicAI.Defend(atcPriority);
        
        if (atcPriority > defPriority)
            Attack(playerPhase);
        else
            Defend(playerPhase);

        EnableButtons();
        playerPhase = true;
    }

    //Funkce pro obsluhu smrtí (hráče/protivníka)

    private void PlayerDeath()
    {
        Destroy(enemy);

        MainCharacter.PlayerPosX = 0;
        MainCharacter.PlayerPosY = 0;

        SceneManager.LoadScene("DeathScene");
    }

    private void EnemyDeath()
    {
        Destroy(enemy);
        
        string[] item = xmlLoader.LoadRandomItemIndexFromXml(enemyDropList);
        
        int index = 0;
        int.TryParse(item[0], out index);
        string category = item[1];
        
        Item loadedItem = xmlLoader.LoadItemFromXml(category, index);
        
        MainCharacter.PlayerHealth = playerHealth;
        MainCharacter.Inventory.Add(loadedItem);

        eb.enemyHealth = enemyHealth;

        cs.EndFight( eb);
        
        SceneManager.LoadScene("World");
    }

    //Funkce pro povolení tlačítek pro jednotlivé akce

    private void EnableButtons()
    {
        cancelMagicButton.gameObject.SetActive(false);
        fireMagicButton.gameObject.SetActive(false);
        waterMagicButton.gameObject.SetActive(false);
        earthMagicButton.gameObject.SetActive(false);
        windMagicButton.gameObject.SetActive(false);
        castMagicButton.gameObject.SetActive(false);

        cancelMagicButton.interactable = true;
        fireMagicButton.interactable = true;
        waterMagicButton.interactable = true;
        earthMagicButton.interactable = true;
        windMagicButton.interactable = true;

        attackButton.gameObject.SetActive(true);
        defendButton.gameObject.SetActive(true);
        magicButton.gameObject.SetActive(true);
        escapeButton.gameObject.SetActive(true);
        endTurnButton.gameObject.SetActive(true);

        if(playerStamina >= playerStaminaDrain)
            attackButton.interactable = true;
        else
            attackButton.interactable = false;
        defendButton.interactable = true;
        magicButton.interactable = true;
        escapeButton.interactable = true;
    }

    private void DisableButtons()
    {
        attackButton.interactable = false;
        defendButton.interactable = false;
        magicButton.interactable = false;
    }

    //Funkce bindováné na tlačítka

    public void AttackButton()
    {
        Attack(playerPhase);
    }

    public void DefendButton()
    {
        Defend(playerPhase);
    }

    public void MagicButton()
    {
        cancelMagicButton.gameObject.SetActive(true);
        fireMagicButton.gameObject.SetActive(true);
        waterMagicButton.gameObject.SetActive(true);
        earthMagicButton.gameObject.SetActive(true);
        windMagicButton.gameObject.SetActive(true);
        castMagicButton.gameObject.SetActive(true);

        attackButton.gameObject.SetActive(false);
        defendButton.gameObject.SetActive(false);
        magicButton.gameObject.SetActive(false);
        escapeButton.gameObject.SetActive(false);
        endTurnButton.gameObject.SetActive(false);
    }

    public void CancelMagicButton()
    {
        cancelMagicButton.gameObject.SetActive(false);
        fireMagicButton.gameObject.SetActive(false);
        waterMagicButton.gameObject.SetActive(false);
        earthMagicButton.gameObject.SetActive(false);
        windMagicButton.gameObject.SetActive(false);
        castMagicButton.gameObject.SetActive(false);

        attackButton.gameObject.SetActive(true);
        defendButton.gameObject.SetActive(true);
        magicButton.gameObject.SetActive(true);
        escapeButton.gameObject.SetActive(true);
        endTurnButton.gameObject.SetActive(true);
    }

    public void CastElementButton(string element)
    {
        cancelMagicButton.gameObject.SetActive(false);
        fireMagicButton.interactable = false;
        waterMagicButton.interactable = false;
        earthMagicButton.interactable = false;
        windMagicButton.interactable = false;
        endTurnButton.gameObject.SetActive(true);
        CastElement(element);
    }

    public void CastSpellButon()
    {
        CastSpell();
    }

    public void EndTurnButton()
    {
        EndTurn();
    }

    public void EscapeButton()
    {
        Escape();
    }
}
