using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicController : MonoBehaviour {
    AudioSource aud;
	void Start() {
        aud = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(SettingsManager.settings.MusicEnabled && !aud.isPlaying) aud.Play();
        if(!SettingsManager.settings.MusicEnabled && aud.isPlaying) aud.Stop();

        if(RoomManager.targetRoom != null) {
            aud.volume = Mathf.Lerp(aud.volume,0f,Time.deltaTime * 5f);
        }       
    }
}
