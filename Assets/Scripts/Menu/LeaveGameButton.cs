using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveGameButton : Button
{
	protected override float multi { get => 2f; }
	public AudioSource aud;

	protected override void Start() {
		base.Start();
		aud = GetComponent<AudioSource>();
	}
	protected override bool __isEnabled() {
		return MenuCameraController.onMainPage == 0;
	}

	protected override void __OnPress() {
		aud.pitch = Random.Range(0.6f,1.3f);
		aud.Play();

		Application.Quit();
	}
}
