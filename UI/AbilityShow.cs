using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityShow : MonoBehaviour
{
    public GameObject horizontalSlash;
    public GameObject meteorShower;
    public GameObject laserRain;
    public GameObject redEnergy;
    public GameObject hollowRed;
    
    public void showAbility(string abilityName)
    {
        horizontalSlash.SetActive(false);
        meteorShower.SetActive(false);
        laserRain.SetActive(false); 
        redEnergy.SetActive(false);
        hollowRed.SetActive(false);

        switch(abilityName)
        {
            case "Horizontal Slash":
                horizontalSlash.SetActive(true);
                break;
            case "Meteor Shower":
                meteorShower.SetActive(true); 
                break;
            case "Laser Rain":
                laserRain.SetActive(true);
                break;
            case "Red Energy":
                redEnergy.SetActive(true);
                break;
            case "Hollow Red":
                hollowRed.SetActive(true);
                break;

        }

    }

}
