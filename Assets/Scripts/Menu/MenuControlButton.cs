using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControlButton:Button {

	public int value;
	AudioSource aud;

	protected void Start() {
		base.Start();
		aud = GetComponent<AudioSource>();
	}
	protected override float multi { get => 2f; }
	protected override bool __isEnabled() {
		return MenuCameraController.onMainPage != value;
	}

	protected override void __OnPress() {
		aud.pitch = Random.Range(0.6f,1.3f);
		aud.Play();

		MenuCameraController.onMainPage = value;
	}
}
