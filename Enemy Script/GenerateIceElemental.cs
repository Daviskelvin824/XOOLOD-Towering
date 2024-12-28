using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateIceElemental : MonoBehaviour
{
    public GameObject iceElemental;
    public int xPos, zPos;
    public int enemyCount;
    TextMeshProUGUI levelIceElem;
    CharacterStats charStats;
    int playerLevel;
    void Start()
    {
        levelIceElem = iceElemental.GetComponentInChildren<TextMeshProUGUI>();
        charStats = CharacterStats.Instance.GetComponent<CharacterStats>();
        playerLevel = charStats.GetLevel();
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 10)
        {
            int randLevel = Random.Range(35, 46);
            levelIceElem.SetText("Lv. " + randLevel.ToString());
            int levelDifference = randLevel - playerLevel;
            if (levelDifference >= 10)
            {
                levelIceElem.color = Color.red;
            }
            else if (levelDifference >= 4)
            {
                levelIceElem.color = Color.yellow;
            }
            else if (levelDifference <= -4)
            {
                levelIceElem.color = Color.white;
            }
            else
            {
                levelIceElem.color = Color.green;
            }
            iceElemental.SetActive(true);
            xPos = Random.Range(640, 760);
            zPos = Random.Range(660, 740);
            Instantiate(iceElemental, new Vector3(xPos, (float)0.5, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }
}
