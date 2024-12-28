using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject playerStatCanvas;
    public GameObject NobolCanvas;
    public GameObject SherylCanvas;
    public GameObject RobertCanvas;


    public NPCFollow Sheryl;
    public NPCFollow Nobol;
    public NPCFollow Robert;

    public bool isPlayerStatCanvas, isNobolCanvas, isSherylCanvas, isRobertCanvas;

    void Start()
    {
        playerStatCanvas.SetActive(false);
        NobolCanvas.SetActive(false);
        SherylCanvas.SetActive(false);
        RobertCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(playerStatCanvas != null)
            {
                isPlayerStatCanvas = !isPlayerStatCanvas;
                playerStatCanvas.SetActive(isPlayerStatCanvas);
            }
        }

        if (Nobol.isInInteractRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isNobolCanvas = !isNobolCanvas;
                NobolCanvas.SetActive(isNobolCanvas);
            }
        }

        if (Sheryl.isInInteractRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isSherylCanvas = !isSherylCanvas;
                SherylCanvas.SetActive(isSherylCanvas);
            }
        }

        if (Robert.isInInteractRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isRobertCanvas = !isRobertCanvas;
                RobertCanvas.SetActive(isRobertCanvas);
            }
        }
    }

    public void CancelButton()
    {
        isNobolCanvas = false;
        NobolCanvas.SetActive(false);
    }

    public void EnterBoss()
    {
        isNobolCanvas = false;
        NobolCanvas.SetActive(false);
    }

    public void CancelSheryl()
    {
        isSherylCanvas = false;
        SherylCanvas.SetActive(false);
    }

    public void CancelRobert()
    {
        isRobertCanvas  = false;
        RobertCanvas.SetActive(false);
    }
}
