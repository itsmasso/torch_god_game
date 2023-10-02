using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This method handles menu and methods for going to different menu screens.
//later will add a way to more efficiently and cleanly switch between menus
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu, characterSelect, settings;

    void Start()
    {
        menu.SetActive(true);
        characterSelect.SetActive(false);
        settings.SetActive(false);
    }

    public void ChooseTorchCharacter()
    {
        GameManager.Instance.currentCharacter = Character.TorchCharacter;
        GameManager.Instance.UpdateScene(1);
    }

    public void ChooseLanternCharacter()
    {
        GameManager.Instance.currentCharacter = Character.LanternCharacter;
        GameManager.Instance.UpdateScene(1);
    }

    public void OnMainMenu()
    {
        menu.SetActive(true);
        characterSelect.SetActive(false);
    }

    public void OnCharacterSelect()
    {
        menu.SetActive(false);
        characterSelect.SetActive(true);
    }

    public void OnSettingsMenu()
    {
        menu.SetActive(false);
        settings.SetActive(true);
    }

    public void OnBack()
    {
        menu.SetActive(true);
        characterSelect.SetActive(false);
        settings.SetActive(false);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }

}
