using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnim : MonoBehaviour
{
    public GameObject rightHandSword;
    public GameObject thighSword;

    private void Start()
    {
        thighSword.SetActive(true);
        rightHandSword.SetActive(false);
    }
    public void DrawSword()
    {
        thighSword.SetActive(false);
        rightHandSword.SetActive(true);
    }

    public void SheathSword()
    {
        thighSword.SetActive(true);
        rightHandSword.SetActive(false);
    }
}
