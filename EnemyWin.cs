using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyWin : MonoBehaviour
{
    public GameObject winCanvas;
    public Slider dragonHealth;

    // Update is called once per frame
    private void Start()
    {
        winCanvas.SetActive(false);
    }
    void Update()
    {

        if (dragonHealth.value <= 0)
        {
            winCanvas.SetActive(true);
        }
    }



}
