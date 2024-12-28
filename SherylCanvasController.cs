using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SherylCanvasController : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI priceText;
    public Image AbilityImgSelected;

    public Image horizontalImg;
    public Image redEnergyImg;
    public Image meteorShowerImg;
    public Image laserRainImg;
    public Image hollowRedImg;
    CharacterStats charStats;

    public Ability selectedAbility;

    private List<Ability> boughtAbility;
    
    void Start()
    {
        var horizontal = new HorizontalSlash();
        ShowAbilityInfo(horizontal, horizontalImg);
        charStats = CharacterStats.Instance.GetComponent<CharacterStats>();
        selectedAbility = horizontal;
    }

    public void ShowAbilityInfo(Ability ability, Image abilityImage)
    {
        selectedAbility = ability;
        titleText.text = ability.Name;
        descriptionText.text = ability.Description;
        AbilityImgSelected.sprite = abilityImage.sprite;
        priceText.text = ability.Price.ToString();
    }

    public void horizontalAbilitySelected()
    {
        ShowAbilityInfo(new HorizontalSlash(), horizontalImg);
    }

    public void redEnergyImgSelected()
    {
        ShowAbilityInfo(new RedEnergy(), redEnergyImg);
    }

    public void meterShowerImgSelected()
    {
        ShowAbilityInfo(new MeteorShower(), meteorShowerImg);
    }

    public void laserRainImgSelected()
    {
        ShowAbilityInfo(new LaserRain(), laserRainImg);
    }

    public void hollowRedImgSelected()
    {
        ShowAbilityInfo(new HollowRed(), hollowRedImg);
    }

    public void BuyAbility()
    {
        int price;
        if (int.TryParse(priceText.text, out price))
        {
            if (price > charStats.getGold())
            {
                return;
            }
            if (CharacterStats.ownedAbility.Select(ability => ability.Name == selectedAbility.Name).Aggregate(false, (x, y) => x || y))
            {
                return;
            }
            charStats.SetGold(price);
            CharacterStats.ownedAbility.Add(selectedAbility);

            if(CharacterStats.ownedAbility.Count == 4) {
                return;
            }
            CharacterStats.equippedAbility.Add(selectedAbility);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        int price;
        if (int.TryParse(priceText.text, out price))
        {
            if (price > charStats.getGold())
            {
                priceText.color = Color.red;
            }
            else
            {
                priceText.color = Color.yellow;
            }
        }
    }
}
