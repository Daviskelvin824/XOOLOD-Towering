using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public int enemyHP = 100;
    public int goldDrop = 100;
    public int xpDrop = 100;
    public Slider healthBar;
    public GameObject text;
    public bool isDragon,isMinion;
    Vector2 xPos = new Vector2();
    Vector2 zPos = new Vector2();
    public void TakeDamage(int damage)
    {
        enemyHP -= damage;
        healthBar.value -= ((float)damage/100);
        DamageIndicator indicator = Instantiate(text, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetTextColorWhite();
        indicator.SetDamageText(damage);
        Debug.Log("health bar value : " + healthBar.value);
        if(enemyHP <= 0)
        {
            animator.SetTrigger("death");
            CharacterStats.Instance.addGold(goldDrop);
            CharacterStats.Instance.setXp(xpDrop);
            StartCoroutine(DisplayRewardsAfterDelay());
            GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(DestroyAfterAnimation());
        }
        else
        {
            animator.SetTrigger("damage");
        }
        IEnumerator DestroyAfterAnimation()
        {
            yield return new WaitForSeconds(3);
            Die();
        }
    }

    IEnumerator DisplayRewardsAfterDelay()
    {
        // Display gold drop
        DamageIndicator goldIndicator = Instantiate(text, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        goldIndicator.SetTextColorYellow();
        goldIndicator.SetDamageText(goldDrop);
        yield return new WaitForSeconds(0.5f); // Wait for half a second

        // Then display XP gain
        DamageIndicator xpIndicator = Instantiate(text, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        xpIndicator.SetTextColorGreen();
        xpIndicator.SetDamageText(xpDrop);

        // Add any additional logic if needed
    }
    private void Die()
    {
        gameObject.SetActive(false);
        if (isDragon)
        {
            LevelLoader levelLoader = FindObjectOfType<LevelLoader>();
            EnterDragon enter = FindAnyObjectByType<EnterDragon>();
            if (levelLoader != null)
            {
                enter.disableCanvas();
                levelLoader.LoadLevel(1); // Load the desired scene index (1 in this case)
            }
        }
        if (isMinion)
        {
            return;
        }
        Invoke("Respawn", .1f);
    }

    void Respawn()
    {
        gameObject.SetActive(true); 
        enemyHP = 100;
        healthBar.value = 1f;
        xPos = gameObject.GetComponent<EnemyRespawnPos>().xPos;
        zPos = gameObject.GetComponent <EnemyRespawnPos>().zPos;
        Vector3 spawnPosition = new Vector3(Random.Range(xPos.x, xPos.y), 1, Random.Range(zPos.x, zPos.y));
        transform.position = spawnPosition;
    }

}
