using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class CharacterStats : MonoBehaviour
{
    public static CharacterStats Instance { get; private set; }
    public static int maxHealth = 1000;
    public static int currHealth;
    public static int attack = 50;
    public static int gold;
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI levelText;
    public UnityEngine.UI.Image healthOrb;
    public GameObject text;
    //public TextMeshProUGUI playerLevelText;
    public static List<Ability> ownedAbility = new ();
    public static List<Ability> equippedAbility = new();
    public List<AbilityShow> abilityHotbar;
    private static int level=1;
    private static int xp;
    //public Stats damage;
    //public Stats armor;
    public static bool isGameOver;
    private static bool isAttacked;
    private GameObject player;
    private void Awake()
    {
        xp = 0;
        level = 1;
        gold = 10000;
        currHealth = maxHealth;
        Instance = this;
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
    }
    private void Update()
    {
        playerHPText.text = currHealth.ToString();
        float xpPercentage = (float)xp / 1000;
        xpText.text = xp.ToString() + "("+xpPercentage*100+"%"+")";
        levelText.text = level.ToString();
        for(int i=0;i<equippedAbility.Count;i++)
        {
            abilityHotbar[i].showAbility(equippedAbility[i].Name);
        }
    }
    public void TakeDamage(int damage)
    {
        isAttacked = true;
        currHealth -= damage;
        float healthPercentage = (float)currHealth / maxHealth;
        healthOrb.fillAmount = healthPercentage;
        Debug.Log(healthOrb.fillAmount.ToString());
        DamageIndicator indicator = Instantiate(text, player.transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetTextColorRed();
        indicator.SetDamageText(damage);
        if (currHealth <= 0)
        {
            currHealth = 0;
            isGameOver = true;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }

    public void LevelUp()
    {
        level++;
        //DamageIndicator indicator = Instantiate(text, player.transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        //indicator.levelUpText("LEVEL UP!");
        //indicator.SetTextColorGreen();
        maxHealth += 100; 
        currHealth = maxHealth;
        attack += 10;
    }

    public int getPlayerAttack()
    {
        return attack;
    }

    public void setXp(int num)
    {
        xp += num;
        while (xp >= 1000) // Assuming 1000 XP is needed for each level up
        {
            xp -= 1000; // Subtract XP needed for level up
            LevelUp(); // Call your LevelUp method
        }
        Debug.Log(xp);
    }

    public int GetLevel()
    {
        return level;
    }

    public int getHealth()
    {
        return currHealth;
    }

    public int getMaxHealth()
    {
        return maxHealth;   
    }

    public int getGold()
    {
        return gold;
    }

    public void SetGold(int num)
    {
        gold -= num;
    }

    public void addGold(int num)
    {
        gold += num;
    }
}
