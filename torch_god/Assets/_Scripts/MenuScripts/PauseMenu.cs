using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void GotoMainMenu()
    {
        GameManager.Instance.UpdateScene(0);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        LevelManager.Instance.isPaused = false;
        gameObject.SetActive(false);
    }
}
