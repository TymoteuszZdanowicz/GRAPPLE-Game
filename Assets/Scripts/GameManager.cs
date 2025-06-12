using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance { get { return instance; } }
    [SerializeField] private MovementController player;

    private string savePath => Application.persistentDataPath + "/save.dat";

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
        player.transform.position = Vector3.zero;
        if (File.Exists(savePath))
            File.Delete(savePath);
    }
}
