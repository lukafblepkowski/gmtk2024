using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : ISaveDataSource
{
	static Settings __settings;
	public static Settings settings { get { 
		if(__settings == null) {
			__settings = new Settings();
		}

		return __settings;
	} }

	public void LoadData(GameData data) {
		__settings = data.settings;
	}

	public void SaveData(ref GameData data) {
		data.settings = settings;
	}
}
public class Settings {
	public bool MusicEnabled;
	public Settings() {
		//Default settings
		MusicEnabled = false;
	} 
}