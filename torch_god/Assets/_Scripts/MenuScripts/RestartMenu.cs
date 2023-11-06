using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartMenu : MonoBehaviour
{
    public void GotoMainMenu()
    {
        GameManager.Instance.UpdateScene(0);
    }

    public void RestartGame()
    {
        ResourceSystem.Instance.ResetPlayerData();
        GameManager.Instance.UpdateScene(1);
    }

    
    
}
