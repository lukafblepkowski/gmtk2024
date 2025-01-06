using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButton:Button {
	AudioSource aud;

	protected override float multi => 1f;
	protected override void Start() {
		base.Start();
		aud = GetComponent<AudioSource>();
	}
	protected override bool __isEnabled() {
		if(RoomManager.targetRoom != null) return false;
		if(UIManager.paused) return false;

		Room nextRoom = RoomManager.nextRoom;
		if(nextRoom == null) {
			return false; //This shouldn't happen, though
		}
		return !(nextRoom.condition == RoomCondition.LOCKED);
	}

	protected override void __OnPress() {
		aud.pitch = Random.Range(0.6f,1.3f);
		aud.Play();

		UIManager.paused = false;
		RoomManager.GotoNext();
	}
}
