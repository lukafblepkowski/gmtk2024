using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Lightsource:MonoBehaviour {
	[Header("Lightsource")]
	public float __range = 1f;
	public bool on = true;

	public virtual float range { get { return __range; } }
	public abstract bool vibrant { get; }
	public abstract float vibrance { get; }
	private void Start() {
		foreach(VineControl vine in VineControl.Vines) {
			vine.budMovement.AddLightsource(this);
		}
	}
}
