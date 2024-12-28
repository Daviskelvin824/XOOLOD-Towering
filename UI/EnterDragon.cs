using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDragon : MonoBehaviour
{
    public GameObject dragonCanvas;
    // Start is called before the first frame update
    void Start()
    {
        dragonCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MoveChar>(out MoveChar mc))
        {
            dragonCanvas.SetActive(true);
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<MoveChar>(out MoveChar mc))
        {
            dragonCanvas.SetActive(false);
        }
    }

    public void disableCanvas()
    {
        dragonCanvas.SetActive(false);
    }
}
