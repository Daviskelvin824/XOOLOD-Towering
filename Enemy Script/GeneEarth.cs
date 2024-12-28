using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeneEarth : MonoBehaviour
{
    public GameObject earthElem;
    public int xPos, zPos;
    public int enemyCount;
    TextMeshProUGUI levelEartheElem;
    CharacterStats charStats;
    int playerLevel;
    void Start()
    {
        levelEartheElem = earthElem.GetComponentInChildren<TextMeshProUGUI>();
        charStats = CharacterStats.Instance.GetComponent<CharacterStats>();
        playerLevel = charStats.GetLevel();
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 10)
        {
            int randLevel = Random.Range(40, 51);
            levelEartheElem.SetText("Lv. " + randLevel.ToString());
            int levelDifference = randLevel - playerLevel;
            if (levelDifference >= 10)
            {
                levelEartheElem.color = Color.red;
            }
            else if (levelDifference >= 4)
            {
                levelEartheElem.color = Color.yellow;
            }
            else if (levelDifference <= -4)
            {
                levelEartheElem.color = Color.white;
            }
            else
            {
                levelEartheElem.color = Color.green;
            }
            earthElem.SetActive(true);
            xPos = Random.Range(270, 380);
            zPos = Random.Range(500, 620);
            Instantiate(earthElem, new Vector3(xPos, (float)0.5, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }
}
