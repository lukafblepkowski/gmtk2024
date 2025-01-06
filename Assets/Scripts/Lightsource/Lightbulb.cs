using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[RequireComponent(typeof(Grabbable))]

public class Lightbulb : Lightsource
{
	[Header("Bulb")]
	public GameObject circle;
    public GameObject glow;
	public bool invertVibrance;

	AudioSource audUP;
	AudioSource audDOWN;

    public Color color = Color.white;
	public Color vibrantColor = Color.white;

	float radlerper = 1f;

	Grabbable grabbable;

	bool audon = false;

	float vibrantlerp = 0;

	float grabRadLerp = 0;
	public override bool vibrant { get => invertVibrance ^ (grabbable.grabbed && Input.GetMouseButton(1)); }
	public override float vibrance { get {
		return vibrant ? 2f : 1f;
	} }

	public override float range => (__range * grabRadLerp);

	private void Start() {
		grabbable = GetComponent<Grabbable>();

		AudioSource[] auds = GetComponents<AudioSource>();

		audUP = auds[0];
		audDOWN = auds[1];
	}
	private void Update() {
		on = grabbable.grabbed;

		if(vibrant != audon) {
			if(vibrant) {
				audUP.Play();
			} else {
				audDOWN.Play();
			}

			audon = vibrant;
		}

		grabRadLerp = Mathf.Lerp(grabRadLerp,grabbable.grabbed ? 1f : 0f,Time.deltaTime * 6f);
		vibrantlerp = Mathf.Lerp(vibrantlerp, vibrant ? 1f : 0f, Time.deltaTime * 3.5f);

		radlerper = Mathf.Lerp(radlerper,on ? 1f : 0f,3f * Time.deltaTime);

		Color currentColor = Color.Lerp(color,vibrantColor,vibrantlerp);
		Color bulbHue = Color.Lerp(color * new Color(0.8f,0.8f,0.8f), currentColor, radlerper);
		Color glowHue = currentColor;
		glowHue.a = Mathf.Lerp(0.35f,0.85f,vibrantlerp);

		circle.GetComponent<SpriteRenderer>().material.color = bulbHue;
		glow.GetComponent<SpriteRenderer>().material.color = glowHue;

		glow.transform.localScale = Vector3.one * range * 2 * radlerper * grabRadLerp;
	}
}
