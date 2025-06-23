using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles in-game UI logic such as pause menu and save/load actions.
/// </summary>
public class GameUIManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;   // Reference to the pause menu panel

    private bool isPaused = false;      // Is the game currently paused

    // Initializes pause menu and ensures game is unpaused
    void Start()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Checks for pause input and toggles pause menu if needed
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.IsGameEnded)
        {
            TogglePauseMenu();
        }
    }

    // Toggles the pause menu and game time
    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    // Calls GameManager to save the game
    public void SaveGame()
    {
        GameManager.Instance.SaveGame();
    }

    // Calls GameManager to load the game
    public void LoadGame()
    {
        GameManager.Instance.LoadGame();
    }

    // Returns to main menu and unpauses the game
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // Resumes the game from pause
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}