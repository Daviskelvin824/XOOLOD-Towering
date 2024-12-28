using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheat : MonoBehaviour
{
    private string cheatCode = "23-1"; // Define your cheat code here
    private string inputString = "";
    public PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckCheatCode();
    }

    void CheckCheatCode()
    {
        // Update inputString with the most recent input
        if (Input.inputString.Length > 0)
        {
            char c = Input.inputString[0];
            inputString += c;
        }

        // Check if the inputString contains the cheat code
        if (inputString.Contains(cheatCode))
        {
            SceneManager.LoadScene(1); // Load scene if cheat code is found
        }
        if(inputString.ToLower().Contains("depdepdep")) 
        {
            playerData.baseSpeed = 50f;
            Debug.Log("Success cheat");
            inputString = "";
        }
    }
}
