using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsPanelManager : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject mainMenuPanel;
    public TMP_Dropdown resolutionDropdown;
    public TextMeshProUGUI timesText;

    private Resolution[] resolutions;

    void Start()
    {
        optionsPanel.SetActive(false);

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        var options = new System.Collections.Generic.List<string>();
        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        ShowTimes();
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        ShowTimes();
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        optionsPanel.SetActive(false);
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    private void ShowTimes()
    {
        string times = PlayerPrefs.GetString("Times", "");
        string[] timesArr = times.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        timesText.text = "Times:\n";
        foreach (var t in timesArr)
        {
            if (float.TryParse(t, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float timeValue))
            {
                timesText.text += $"{timeValue:F2} s\n";
            }
        }
    }
}