using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GeneFireElem : MonoBehaviour
{
    public GameObject fireElem;
    public int xPos, zPos;
    public int enemyCount;
    TextMeshProUGUI levelFireElem;
    CharacterStats charStats;
    int playerLevel;
    void Start()
    {
        levelFireElem = fireElem.GetComponentInChildren<TextMeshProUGUI>();
        charStats = CharacterStats.Instance.GetComponent<CharacterStats>();
        playerLevel = charStats.GetLevel();
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 10)
        {
            int randLevel = Random.Range(40, 51);
            levelFireElem.SetText("Lv. " + randLevel.ToString());
            int levelDifference = randLevel - playerLevel;
            if (levelDifference >= 10)
            {
                levelFireElem.color = Color.red;
            }
            else if (levelDifference >= 4)
            {
                levelFireElem.color = Color.yellow;
            }
            else if (levelDifference <= -4)
            {
                levelFireElem.color = Color.white;
            }
            else
            {
                levelFireElem.color = Color.green;
            }
            fireElem.SetActive(true);
            xPos = Random.Range(270, 380);
            zPos = Random.Range(500, 620);
            Instantiate(fireElem, new Vector3(xPos, (float)0.5, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }
}
