using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelLoader : MonoBehaviour
{
    [field:SerializeField]private GameObject loadingScreen;
    [field: SerializeField]private Slider slider;
    [field: SerializeField] private GameObject playerCanvas;
    private void Awake()
    {
        slider.value = 0;
        loadingScreen.SetActive(false);
    }
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex)); 
    }   

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        UnityEngine.AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);
        playerCanvas.SetActive(false);
        while (!operation.isDone)
        {
            Debug.Log(operation.progress);
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Adjust progress calculation
            slider.value = progress;
            yield return null;
        }
    }

}
