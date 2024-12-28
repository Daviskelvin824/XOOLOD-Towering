using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enterportal : MonoBehaviour
{
    public GameObject EnterportalCanvas;
    private void Start()
    {
        EnterportalCanvas.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<MoveChar>(out MoveChar mc))
        {
            EnterportalCanvas.SetActive(true);
        }
        

    }

    private void OnTriggerExit(Collider other)
    {
        EnterportalCanvas.SetActive(false);
    }

    public void CancelButton()
    {
        EnterportalCanvas.SetActive(false);
    }

    public void EnterMaze()
    {
        EnterportalCanvas.SetActive(false);
    }
}
