using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickerer : MonoBehaviour
{
    public float timeOn = 0.1f;
    public float timeOff = 0.4f;

	bool on = true;
	float timer;

	BoxCollider2D collider2d;

	private void Start() {
		collider2d = GetComponent<BoxCollider2D>();
		timer = timeOn;
	}
	private void Update() {
		timer -= Time.deltaTime;

		if(timer<0) {
			if(on) timer += timeOff;
			else timer += timeOn;

			on = !on;
		}

		collider2d.enabled = !on;
	}
}
