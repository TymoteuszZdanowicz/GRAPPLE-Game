using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance { get { return instance; } }
    [SerializeField] private MovementController player;
    [SerializeField] private Vector3 startPosition = new Vector3(0, 0, 0); // Mo¿esz ustawiæ w Inspectorze

    private string savePath => Application.persistentDataPath + "/save.dat";
    private bool isGameEnded = false;

    void Start()
    {
        int loadGame = PlayerPrefs.GetInt("LoadGame", 0);
        if (loadGame == 1)
            LoadGame();
        else
            NewGame();
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (!isGameEnded && player != null && player.transform.position.y > 220f)
        {
            EndGame();
        }
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.playerPosX = player.transform.position.x;
        data.playerPosY = player.transform.position.y;

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
        Debug.Log("Koniec gry! Gracz przekroczy³ Y=225.");
    }
}