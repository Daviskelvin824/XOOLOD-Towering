using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMain : MonoBehaviour
{
    public GameObject endRoomCanvas;
    private bool isCanvasActive;
    private bool playerInside = false;
    private void Start()
    {
        isCanvasActive = false;
        endRoomCanvas.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        // Check for input only if the player is inside the trigger area
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            isCanvasActive=!isCanvasActive;
            endRoomCanvas.SetActive(isCanvasActive);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            playerInside = false;
        }
    }

    public void CancelButton()
    {
        Debug.Log("cancel");
        isCanvasActive = false;
        endRoomCanvas.SetActive(false);
    }

    public void Enter()
    {
        isCanvasActive = false;
        endRoomCanvas.SetActive(false);
    }

}
