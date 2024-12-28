using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatController : MonoBehaviour
{
    public TextMeshProUGUI levelAmount;
    public TextMeshProUGUI healthAmount;
    public TextMeshProUGUI goldAmount;
    public TextMeshProUGUI attackAmount;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        levelAmount.text = CharacterStats.Instance.GetLevel().ToString();
        healthAmount.text = CharacterStats.Instance.getMaxHealth().ToString();
        goldAmount.text = CharacterStats.Instance.getGold().ToString();
        attackAmount.text = CharacterStats.Instance.getPlayerAttack().ToString();
    }
}
