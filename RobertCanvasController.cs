using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RobertCanvasController : MonoBehaviour
{
    public List<AbilityShow> loadoutPanel;
    public List<AbilityShow> ownedPanel;
    public AbilityShow abilityToChange;
    public AbilityShow selectedAbility;
    public TextMeshProUGUI priceText;
    private int equiptIndex;
    private int ownedIndex;

    void Start()
    {
        equiptIndex = -1;
        ownedIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
    
        

        for (int i = 0;i<CharacterStats.equippedAbility.Count;i++)
        {
            loadoutPanel[i].showAbility(CharacterStats.equippedAbility[i].Name);
        }

        for(int i = 0;i<CharacterStats.ownedAbility.Count;i++)
        {
            ownedPanel[i].showAbility(CharacterStats.ownedAbility[i].Name);

        }
        
    }

    public void SelectOwned(int index)
    {
        if (index < CharacterStats.ownedAbility.Count)
        {

            abilityToChange.showAbility(CharacterStats.ownedAbility[index].Name);
            ownedIndex = index;
            priceText.text = CharacterStats.ownedAbility[index].Price.ToString();
            if (CharacterStats.ownedAbility[ownedIndex].Price > CharacterStats.Instance.getGold())
            {
                priceText.color = Color.red;
            }
            else
            {
                priceText.color = Color.yellow;
            }
        }

    }

    public void SelectEquipped(int index)
    {
        if (index < CharacterStats.equippedAbility.Count)
        {
            selectedAbility.showAbility(CharacterStats.equippedAbility[index].Name);
            equiptIndex = index;
        }
    }

    public void ChangeAbility()
    {
        if(equiptIndex==-1 || ownedIndex == -1)
        {
            return;
        }
        if(CharacterStats.ownedAbility[ownedIndex].Price > CharacterStats.Instance.getGold())
        {

            return;
        }
        for(int i = 0;i< CharacterStats.equippedAbility.Count;i++)
        {
            if (CharacterStats.equippedAbility[i].Name == CharacterStats.ownedAbility[ownedIndex].Name)
            {
                return;
            }
          
        }
        CharacterStats.equippedAbility[equiptIndex] = CharacterStats.ownedAbility[ownedIndex];
    }

}
