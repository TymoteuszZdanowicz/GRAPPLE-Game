using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public Button continueButton;

    void Start()
    {
        if (!SaveExists())
            continueButton.interactable = false;

        mainMenuPanel.SetActive(true);
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
