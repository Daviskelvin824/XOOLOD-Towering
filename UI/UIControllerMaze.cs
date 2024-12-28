using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerMaze : MonoBehaviour
{
    public GameObject teleportToSecondFloor;
    public GameObject teleportToFirstFloor;
    public GameObject player;
    private bool isFirstFloor, isSecondFloor;
    private bool playerInside = false;

    private bool ready = true;
    void Start()
    {
        isFirstFloor = true;
        isSecondFloor = false;
        teleportToFirstFloor.SetActive(false);
        teleportToSecondFloor.SetActive(false);
    }

    private void Update()
    {
        // Check for input only if the player is inside the trigger area
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (ready)
            {
                ToggleTeleportUI();
                ready = false;
                StartCoroutine(EnableTP());
            }
        }
    }

    private IEnumerator EnableTP()
    {
        yield return new WaitForSeconds(5);
        ready = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
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

    private void ToggleTeleportUI()
    {
        Debug.Log("Pressed");
        Debug.Log(player.transform.position.ToString());   

        if(player.transform.position.y < 35)
        {
            player.transform.position += Vector3.up * 6;


        }
        else
        {
            player.transform.position -= Vector3.up * 5;
        }
        //if (isSecondFloor == true)
        //{
        //    isFirstFloor = true;
        //    isSecondFloor = false;
        //    playerStartPosition = new Vector3(
        //        transform.position.x,
        //        transform.position.y - 5,
        //        transform.position.z
        //    );
        //    player.transform.position = playerStartPosition;
        //}
        //else
        //{
        //    isFirstFloor = false;
        //    isSecondFloor = true;
        //    playerStartPosition = new Vector3(
        //        transform.position.x,
        //        transform.position.y + 6,
        //        transform.position.z
        //    );
        //    player.transform.position = playerStartPosition;

        //}


        //teleportToSecondFloor.SetActive(true);
        //if (isFirstFloor)
        //{
        //    teleportToFirstFloor.SetActive(false); // Deactivate the other UI
        //    isFirstFloor = false; // Toggle the floor state
        //    isSecondFloor = true;
        //}
        //else if (isSecondFloor)
        //{
        //    teleportToFirstFloor.SetActive(true);
        //    teleportToSecondFloor.SetActive(false); // Deactivate the other UI
        //    isFirstFloor = true; // Toggle the floor state
        //    isSecondFloor = false;
        //}

    }

    public void CancelButton()
    {
        if (isFirstFloor == false)
        {
            isFirstFloor = true;
            teleportToSecondFloor.SetActive(false);

        }
        else if (isSecondFloor == false)
        {
            isSecondFloor = true;
            teleportToFirstFloor.SetActive(false);

        }
    }

    public void Teleport()
    {
        if (isFirstFloor == false)
        {
            isSecondFloor = true;
            Vector3 playerStartPosition = new Vector3(
                transform.position.x,
                transform.position.y + 6,
                transform.position.z
            );
            player.transform.position = playerStartPosition;
        }
        else if (isSecondFloor == false)
        {
            isFirstFloor = true;
            Vector3 playerStartPosition = new Vector3(
                transform.position.x,
                transform.position.y + 6,
                transform.position.z
            );
            player.transform.position = playerStartPosition;
        }
    }

}