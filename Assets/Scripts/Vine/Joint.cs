using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint {
	public Vector2 pos;
	public float width;
	public Vector2 facing;
	public float uv = 0;
	public float uvprev = 0;
	public float z = 0;
	
	public Joint(Vector2 pos, float width, Vector2 facing) {
		this.pos = pos;
		this.width = width;
		this.facing = facing;
	}

	public static Joint Lerp(Joint a, Joint b, float t) {
		return new Joint(Vector2.Lerp(a.pos, b.pos, t),
			Mathf.Lerp(a.width, b.width, t),
			Vector2.Lerp(a.facing, b.facing, t)
		);
	}
}
