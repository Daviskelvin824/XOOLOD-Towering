using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerBoss : MonoBehaviour
{
    public GameObject playerStatCanvas;
    public bool isPlayerStatCanvas;
    // Start is called before the first frame update
    void Start()
    {
        playerStatCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (playerStatCanvas != null)
            {
                isPlayerStatCanvas = !isPlayerStatCanvas;
                playerStatCanvas.SetActive(isPlayerStatCanvas);
            }
        }
    }
}
