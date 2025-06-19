using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance { get { return instance; } }
    [SerializeField] private MovementController player;
    [SerializeField] private Vector3 startPosition = new Vector3(0, 0, 0);
    [SerializeField] private TextMeshProUGUI playerTimeText;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI finalTimeText;

    private string savePath => Application.persistentDataPath + "/save.dat";
    private bool isGameEnded = false;
    public bool IsGameEnded => isGameEnded;
    private float playerTime = 0f;

    void Start()
    {
        int loadGame = PlayerPrefs.GetInt("LoadGame", 0);
        if (loadGame == 1)
            LoadGame();
        else
            NewGame();

        if (endPanel != null)
            endPanel.SetActive(false);
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

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

    public void NewGame()
    {
        player.transform.position = startPosition;
        if (File.Exists(savePath))
            File.Delete(savePath);
    }

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

}