using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the options panel logic, including resolution, fullscreen, show time toggle, and displaying run times.
/// </summary>
public class OptionsPanelManager : MonoBehaviour
{
    public GameObject optionsPanel;                // Reference to the options panel
    public GameObject mainMenuPanel;               // Reference to the main menu panel
    public TMP_Dropdown resolutionDropdown;        // Dropdown for screen resolutions
    public Toggle fullscreenToggle;                // Toggle for fullscreen mode
    public Toggle showTimeToggle;                  // Toggle for showing in-game timer
    public TextMeshProUGUI timesText;              // Text field for displaying run times

    private Resolution[] resolutions;              // List of available resolutions

    // Initializes options panel, dropdowns, toggles, and times display
    void Start()
    {
        optionsPanel.SetActive(false);

        var commonResolutions = new[]
        {
            new Resolution { width = 1920, height = 1080 },
            new Resolution { width = 1600, height = 900 },
            new Resolution { width = 1366, height = 768 },
            new Resolution { width = 1280, height = 720 },
            new Resolution { width = 1024, height = 576 }
        };

        resolutionDropdown.ClearOptions();
        var options = new System.Collections.Generic.List<string>();
        int currentResIndex = 0;

        for (int i = 0; i < commonResolutions.Length; i++)
        {
            var res = commonResolutions[i];
            string option = res.width + " x " + res.height;
            options.Add(option);

            if (Screen.currentResolution.width == res.width && Screen.currentResolution.height == res.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        resolutions = commonResolutions;

        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }

        if (showTimeToggle != null)
        {
            bool showTime = PlayerPrefs.GetInt("ShowTime", 1) == 1;
            showTimeToggle.isOn = showTime;
            showTimeToggle.onValueChanged.AddListener(OnShowTimeChanged);
        }

        ShowTimes();
    }

    // Opens the options panel and updates times display
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        ShowTimes();
    }

    // Closes the options panel
    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    // Returns to the main menu panel
    public void ReturnToMainMenu()
    {
        optionsPanel.SetActive(false);
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    // Sets the screen resolution
    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    // Sets fullscreen mode
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // Displays only completed run times in the timesText field
    private void ShowTimes()
    {
        string times = PlayerPrefs.GetString("Times", "");
        string[] timesArr = times.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        timesText.text = "Times:\n";
        int count = 1;
        foreach (var t in timesArr)
        {
            if (float.TryParse(t, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float timeValue))
            {
                if (timeValue > 0.01f && timeValue < 3600f)
                    timesText.text += $"{count++}. {timeValue:F2} s\n";
            }
        }
        if (count == 1)
            timesText.text += "No completed runs yet. (Currently unavailable)";
    }

    // Handles the show time toggle and updates GameManager
    private void OnShowTimeChanged(bool value)
    {
        PlayerPrefs.SetInt("ShowTime", value ? 1 : 0);
        PlayerPrefs.Save();
        if (GameManager.Instance != null)
            GameManager.Instance.SetShowTime(value);
    }
}