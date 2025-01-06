using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton:Button {
	protected override float multi => 1f;

	AudioSource aud;
	protected override void Start() {
		base.Start();
		aud = GetComponent<AudioSource>();
	}
	protected override void __OnPress() {
		aud.pitch = Random.Range(0.6f,1.3f);
		aud.Play();

		RoomManager.Get("LevelMenu").Goto();
		UIManager.paused = false;
	}
	protected override bool __isEnabled() {
		if(RoomManager.targetRoom != null) return false;
		return UIManager.paused;
	}
}
