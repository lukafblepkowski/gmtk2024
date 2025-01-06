using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blightbulb:Lightbulb {
	public override float vibrance {
		get {
			return vibrant ? -1.4f : -0.7f;
		}
	}
}
