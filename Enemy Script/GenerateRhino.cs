using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GenerateRhino : MonoBehaviour
{
    public GameObject rhinoPrefab;
    TextMeshProUGUI levelRhino;
    public int enemyCountLimit = 15;
    CharacterStats charStats;
    int playerLevel;

    void Start()
    {
        levelRhino = rhinoPrefab.GetComponentInChildren<TextMeshProUGUI>();
        charStats = CharacterStats.Instance.GetComponent<CharacterStats>();
        playerLevel = charStats.GetLevel();
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        for (int i = 0; i < enemyCountLimit; i++)
        {
            int randLevel = Random.Range(1, 21);
            levelRhino.SetText("Lv. " + randLevel.ToString());
            int levelDifference = randLevel - playerLevel;
            if (levelDifference >= 10)
            {
                levelRhino.color = Color.red; 
            }
            else if (levelDifference >= 4)
            {
                levelRhino.color = Color.yellow;
            }
            else if (levelDifference <= -4)
            {
                levelRhino.color = Color.white;
            }
            else
            {
                levelRhino.color = Color.green;
            }
            Vector3 spawnPosition = new Vector3(Random.Range(640, 760), 1, Random.Range(660, 740));
            GameObject newRhino = Instantiate(rhinoPrefab, spawnPosition, Quaternion.identity);
            NavMeshAgent agent = newRhino.GetComponent<NavMeshAgent>();
            if (agent != null)
                agent.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

}
