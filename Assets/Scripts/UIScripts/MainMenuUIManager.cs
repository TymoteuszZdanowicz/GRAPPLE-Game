using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles main menu logic, including new game, continue, options, and quit actions.
/// </summary>
public class MainMenuUIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;         // Reference to the main menu panel
    public GameObject optionsPanel;          // Reference to the options panel
    public Button continueButton;            // Continue game button
    public Button optionsButton;             // Options button

    private OptionsPanelManager optionsPanelManager; // Reference to options panel manager

    // Initializes menu and checks for save file
    void Start()
    {
        if (!SaveExists())
            continueButton.interactable = false;

        mainMenuPanel.SetActive(true);

        if (optionsPanel != null)
            optionsPanelManager = optionsPanel.GetComponent<OptionsPanelManager>();
    }

    // Starts a new game and loads the level scene
    public void StartNewGame()
    {
        PlayerPrefs.SetInt("LoadGame", 0);
        SceneManager.LoadScene("Level");
    }

    // Continues a saved game and loads the level scene
    public void ContinueGame()
    {
        PlayerPrefs.SetInt("LoadGame", 1);
        SceneManager.LoadScene("Level");
    }

    // Opens the options panel and hides the main menu
    public void OpenOptions()
    {
        if (optionsPanelManager != null)
        {
            mainMenuPanel.SetActive(false);
            optionsPanelManager.OpenOptions();
        }
    }

    // Closes the options panel and shows the main menu
    public void CloseOptions()
    {
        if (optionsPanelManager != null)
        {
            optionsPanelManager.CloseOptions();
            mainMenuPanel.SetActive(true);
        }
    }

    // Quits the application
    public void QuitGame()
    {
        Application.Quit();
    }

    // Checks if a save file exists
    private bool SaveExists()
    {
        return System.IO.File.Exists(Application.persistentDataPath + "/save.dat");
    }
}