using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFileHandler
{
    string saveDir = "";
    string saveFileName = "";

    public SaveFileHandler(string saveDir, string saveFileName) {
        this.saveDir = saveDir;
        this.saveFileName = saveFileName;
    }

    public GameData Load() {
		string fullPath = Path.Combine(saveDir,saveFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath)) {
            try {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath,FileMode.Open)) {
                    using(StreamReader reader = new StreamReader(stream)) {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            } catch(Exception e) {
                Debug.LogError("Error occured when trying to load data from file: "+fullPath);
            }
        }
        return loadedData;
	}

    public void Save(GameData data) {
        string fullPath = Path.Combine(saveDir,saveFileName);
        try {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string DataToStore = JsonUtility.ToJson(data, true);

            using(FileStream stream = new FileStream(fullPath,FileMode.Create)) {
                using(StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(DataToStore);
                }
            }

        } catch(Exception e) {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath);
        }
    }
}
