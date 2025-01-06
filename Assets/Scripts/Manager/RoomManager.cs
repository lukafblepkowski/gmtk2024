using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class RoomManager:MonoBehaviour, ISaveDataSource {
    public List<Room> __inputRooms;
    public static bool started = false;
    public static bool loadGameDelayed = false;

    public static RoomManager instance = null;

	#region Dynamic
	void Start()
    {
        if(started) {
            Destroy(gameObject);
            return;
        } else {
            instance = this;
            started = true;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Room room in __inputRooms) {
            rooms.Add(room);
        }

        if(loadGameDelayed) {
			Debug.LogAssertion("Delayed Load has been called.");
			SaveManager.LoadGame();
            loadGameDelayed = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentRoom == null) return;

        bool inCorrectRoom = targetRoom == null;
        float goalLerp = inCorrectRoom ? 0.0f : 1.0f;

        transitionLerper = Mathf.Lerp(transitionLerper, goalLerp, Time.deltaTime);

        if(transitionLerper >= 0.95f && !inCorrectRoom) {
            //Change rooms

            SceneManager.LoadScene("Scenes/"+targetRoom.fileLocation);
            targetRoom = null;
        }
    }
    #endregion

    #region Static

    public static List<Room> rooms = new List<Room>();
    public static Room targetRoom = currentRoom;
    public static Room currentRoom { get {
		if(!started) { Debug.Log("Room Manager not started yet!"); return null; }
		foreach(Room r in rooms) {
            if(r.fileLocation == SceneManager.GetActiveScene().name) return r;
        }

        return null;
    } }
    
    public static Room nextRoom { get {
		if(!started) { Debug.Log("Room Manager not started yet!"); return null; }
		Room current = currentRoom;
        for(int i = 0;i < rooms.Count-1;i++) {
            if(rooms[i] == current) {
                return rooms[i+1];
            }
        }

        return null;
    } }

    public static float transitionLerper;
	public static Room Get(string name) {
		foreach(Room room in rooms) {
			if(room.fileLocation == name) return room;
		}

		return null;
	}

    public static void Goto(Room room, bool force = false) {
        targetRoom = room;

        if(force == true) {
            transitionLerper = 1.0f;
            instance.Update();
            transitionLerper = 0.0f;
        }
    }

	public static void Restart() {
		if(!started) { Debug.Log("Room Manager not started yet!"); return; }

		Goto(currentRoom);
	}

	public static void GotoNext() {
		if(!started) { Debug.Log("Room Manager not started yet!"); return; }

		Goto(nextRoom);
	}

	public static void UnlockNext() {
		if(!started) { Debug.Log("Room Manager not started yet!"); return; }

		if(nextRoom == null) return;

        if(nextRoom.condition == RoomCondition.LOCKED) nextRoom.condition = RoomCondition.UNLOCKED;
	}

	public static void CompleteCurrent() {
		if(!started) { Debug.Log("Room Manager not started yet!"); return; }

		currentRoom.condition = RoomCondition.COMPLETE;
	}

	public void LoadData(GameData data) {
        if(!started) {
            Debug.LogAssertion("Early call to RoomManagers LoadData function. It's not on yet. Delayed until startup.");
            loadGameDelayed = true;
            return;
        }

        if(data.roomConditions.Count != rooms.Count) {
            Debug.LogError("Save data has invalid room conditions");
            return;
        }

        for(int i = 0; i < rooms.Count; i++) {
            rooms[i].condition = data.roomConditions[i];
        }
	}
	public void SaveData(ref GameData data) {
		if(!started) { Debug.Log("Room Manager not started yet!"); return; }

		for(int i = 0; i < rooms.Count; i++) {
            if(i >= data.roomConditions.Count) {
                data.roomConditions.Add(rooms[i].condition);
            }
            else {
                data.roomConditions[i] = rooms[i].condition;
            }
        }
	}

	#endregion
}

[Serializable]
public class Room {
    public string fileLocation;
    public string name;
    public string description;
    public RoomCondition condition;

    public Room(string fileLocation, string name, string description) {
        this.fileLocation = fileLocation;
        this.name = name;
        this.description = description;

        condition = RoomCondition.LOCKED;
    }
    public void Goto() {
        RoomManager.Goto(this);
    }
}

public enum RoomCondition {
    LOCKED,
    UNLOCKED,
    COMPLETE
}
