using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability:MonoBehaviour
{
    public Ability(string name,string description, int price)
    {
        Name = name;
        Description = description;
        Price = price;
    }

    private Ability()
    {
    }

    public string Name
    {
        get;set;
    }
    public string Description {
        get; set;
    }
    public int Price {
        get; set;
    }

    public Sprite Image
    {
        get; set;
    }


}
