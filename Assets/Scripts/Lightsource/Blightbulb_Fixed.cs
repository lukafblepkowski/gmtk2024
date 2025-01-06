using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blightbulb_Fixed:Blightbulb {
	public override bool vibrant => true;
	public override float vibrance {
		get {
			return vibrant ? -1.4f : -0.7f;
		}
	}

	private void Start() {
		transform.localScale = Vector3.one * range * 2;
	}
}
