using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {
    [SerializeField] string fileName = "File";
    public static SaveManager instance { get; private set; }
	private void Awake() {
        if(instance != null) {
            Debug.LogError("Found more than one saveManager in the scene. Destroying...");
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);

		saveFileHandler = new SaveFileHandler(Application.persistentDataPath,fileName);
        saveDataSources = GetSaveDataSources();

		LoadGame();
	}

	void OnApplicationQuit() {
        SaveGame();
    }

    static List<ISaveDataSource> saveDataSources;

    static SaveFileHandler saveFileHandler;

	static GameData gameData;
    public static void NewGame() {
        gameData = new GameData();
	}
    public static void LoadGame() {
		saveDataSources = GetSaveDataSources();

		gameData = saveFileHandler.Load();

        if(gameData == null) {
            NewGame();
        }

        foreach(ISaveDataSource ds in saveDataSources) {
            ds.LoadData(gameData);
        }
    }

    public static void SaveGame() {
		saveDataSources = GetSaveDataSources();

		foreach(ISaveDataSource ds in saveDataSources) {
            ds.SaveData(ref gameData);
        }

        saveFileHandler.Save(gameData);
    }

    static List<ISaveDataSource> GetSaveDataSources() {
        IEnumerable<ISaveDataSource> saveDataSources = FindObjectsOfType<MonoBehaviour>().OfType<ISaveDataSource>();

        return new List<ISaveDataSource>(saveDataSources);
    }
}

public class GameData {
    public List<RoomCondition> roomConditions;

    public Settings settings; 
    public GameData() {
        roomConditions = new List<RoomCondition>();
        settings = new Settings();
    }
}
