
using UnityEngine;
using UnityEngine.UI;

//This method handles menu and methods for going to different menu screens.
//later will add a way to more efficiently and cleanly switch between menus
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menu, characterSelect, settings, pickGame;

    [SerializeField]
    private Button continueGameButton;

    private int sceneToStart;

    void Start()
    {
        if (ResourceSystem.Instance.saveData.playerData.currentGameLevel > 1 || 
            (ResourceSystem.Instance.saveData.playerData.currentGameLevel <= 1 && ResourceSystem.Instance.saveData.playerData.currentFloor > 1))
        {
            continueGameButton.interactable = true;
            continueGameButton.GetComponent<Image>().color = Color.white;

        }
        else 
        {

            continueGameButton.interactable = false;
            continueGameButton.GetComponent<Image>().color = Color.gray;
        }

        menu.SetActive(true);
        characterSelect.SetActive(false);
        settings.SetActive(false);
        pickGame.SetActive(false);
    }

    public void ChooseTorchCharacter()
    {
        GameManager.Instance.currentCharacter = Character.TorchCharacter;
        ResourceSystem.Instance.saveData.playerData.character = Character.TorchCharacter;
        GameManager.Instance.UpdateScene(sceneToStart);
    }

    public void ChooseLanternCharacter()
    {
        GameManager.Instance.currentCharacter = Character.LanternCharacter;
        ResourceSystem.Instance.saveData.playerData.character = Character.LanternCharacter;
        GameManager.Instance.UpdateScene(sceneToStart);
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
        pickGame.SetActive(false);
    }

    public void OnSettingsMenu()
    {
        menu.SetActive(false);
        settings.SetActive(true);
    }

    public void OnPickGame()
    {
        menu.SetActive(false);
        pickGame.SetActive(true);

    }

    public void OnNewGame()
    {
        ResourceSystem.Instance.ResetPlayerData();
        sceneToStart = 1; //start on first level, first floor
        OnCharacterSelect();
    }

    public void OnContinueGame()
    {
        sceneToStart = ResourceSystem.Instance.saveData.playerData.currentGameLevel;
        GameManager.Instance.UpdateScene(sceneToStart);

    }

    public void OnBack()
    {
        menu.SetActive(true);
        characterSelect.SetActive(false);
        settings.SetActive(false);
        pickGame.SetActive(false);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }

}
