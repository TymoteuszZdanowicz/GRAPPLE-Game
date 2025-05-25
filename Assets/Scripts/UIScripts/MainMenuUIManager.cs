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
        SceneManager.LoadScene("Level1");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Level1");
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
