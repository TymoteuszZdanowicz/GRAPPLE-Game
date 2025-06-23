using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    static private GameManager instance;
    static public GameManager Instance { get { return instance; } }

    [SerializeField] private MovementController player; // Reference to player controller
    [SerializeField] private Vector3 startPosition; // Player start position
    [SerializeField] private TextMeshProUGUI playerTimeText; // UI text for in-game timer
    [SerializeField] private GameObject endPanel; // End game panel
    [SerializeField] private TextMeshProUGUI finalTimeText; // UI text for final time

    private string savePath => Application.persistentDataPath + "/save.dat"; // Save file path
    private bool isGameEnded = false; // Game end state
    public bool IsGameEnded => isGameEnded; // Public getter for end state
    private bool showTime = true; // Should show timer
    private float playerTime = 0f; // Player's current time

    // Called on game start, loads or starts new game
    void Start()
    {
        int loadGame = PlayerPrefs.GetInt("LoadGame", 0);
        if (loadGame == 1)
            LoadGame();
        else
            NewGame();

        if (endPanel != null)
            endPanel.SetActive(false);

        bool showTimePref = PlayerPrefs.GetInt("ShowTime", 1) == 1;
        SetShowTime(showTimePref);
    }

    // Ensures singleton instance
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Updates timer and checks for end condition
    void Update()
    {
        if (!isGameEnded)
        {
            playerTime += Time.deltaTime;
            if (playerTimeText != null)
                playerTimeText.text = $"Time: {playerTime:F2}s";

            if (player != null && player.transform.position.y > 218f)
            {
                EndGame();
            }
        }
    }

    // Saves player position and time to file
    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.playerPosX = player.transform.position.x;
        data.playerPosY = player.transform.position.y;
        data.playerTime = playerTime;

        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream file = File.Create(savePath))
        {
            bf.Serialize(file, data);
        }
    }

    // Loads player position and time from file
    public void LoadGame()
    {
        if (!File.Exists(savePath)) return;

        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream file = File.Open(savePath, FileMode.Open))
        {
            SaveData data = (SaveData)bf.Deserialize(file);
            player.transform.position = new Vector3(data.playerPosX, data.playerPosY, player.transform.position.z);
            playerTime = data.playerTime;
        }
    }

    // Starts a new game and resets player position
    public void NewGame()
    {
        player.transform.position = startPosition;
        if (File.Exists(savePath))
            File.Delete(savePath);
    }

    // Handles end of game logic, shows end panel and saves time
    private void EndGame()
    {
        isGameEnded = true;
        Time.timeScale = 0f;
        Debug.Log("Koniec gry! Gracz przekroczy³ Y=218.");

        if (endPanel != null)
            endPanel.SetActive(true);
        if (finalTimeText != null)
            finalTimeText.text = $"Your time: {playerTime:F2}s";

        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
        if (playerTime < bestTime)
            PlayerPrefs.SetFloat("BestTime", playerTime);

        string times = PlayerPrefs.GetString("Times", "");
        times += $"{playerTime:F2};";
        PlayerPrefs.SetString("Times", times);
        PlayerPrefs.Save();
    }

    // Sets visibility of the in-game timer
    public void SetShowTime(bool value)
    {
        showTime = value;
        if (playerTimeText != null)
            playerTimeText.gameObject.SetActive(showTime);
    }
}