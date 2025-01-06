using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : Button
{
	SpriteRenderer spriteRenderer;
	Room togoto;

	AudioSource audbad;
	AudioSource audgood;

	public string filelocation;
	protected override float multi { get => 2f; }

	public Sprite locked;
	public Sprite unlocked;
	public Sprite complete;

	private void Start() {
		audbad = GetComponents<AudioSource>()[0]; 
		audgood = GetComponents<AudioSource>()[1]; 
		spriteRenderer = GetComponent<SpriteRenderer>();

		togoto = RoomManager.Get(filelocation);

		Sprite sprite = locked;
		if(togoto.condition == RoomCondition.UNLOCKED) sprite = unlocked;
		if(togoto.condition == RoomCondition.COMPLETE) sprite = complete;

		spriteRenderer.sprite = sprite;
	}

	protected void Update() {
		base.Update();
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 10f);
	}
	protected override bool __isEnabled() {
		return MenuCameraController.onMainPage == -1;
	}

	protected override void __OnPress() {
		if(togoto == null) return;

		if(togoto.condition != RoomCondition.LOCKED) {
			audgood.pitch = Random.Range(0.6f,1.3f);
			audgood.Play();
			RoomManager.Goto(togoto);
		}
		else {
			audbad.pitch = Random.Range(0.6f,1.3f);
			audbad.Play();
			transform.rotation *= Quaternion.Euler(0,0,Mathf.Sign(Random.Range(-1f,1f)) * Random.Range(10f,30f));
		}
	}
}
