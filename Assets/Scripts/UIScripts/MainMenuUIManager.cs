using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public Button continueButton;
    public Button optionsButton;

    private OptionsPanelManager optionsPanelManager;

    void Start()
    {
        if (!SaveExists())
            continueButton.interactable = false;

        mainMenuPanel.SetActive(true);

        if (optionsPanel != null)
            optionsPanelManager = optionsPanel.GetComponent < OptionsPanelManager >();
    }

    public void StartNewGame()
    {
        PlayerPrefs.SetInt("LoadGame", 0);
        SceneManager.LoadScene("Level");
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("LoadGame", 1);
        SceneManager.LoadScene("Level");
    }

    public void OpenOptions()
    {
        if (optionsPanelManager != null)
        {
            mainMenuPanel.SetActive(false);
            optionsPanelManager.OpenOptions();
        }
    }

    public void CloseOptions()
    {
        if (optionsPanelManager != null)
        {
            optionsPanelManager.CloseOptions();
            mainMenuPanel.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private bool SaveExists()
    {
        return System.IO.File.Exists(Application.persistentDataPath + "/save.dat");
    }
}
