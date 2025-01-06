using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beehive : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D collision) {
		VineControl control = collision.GetComponentInParent<VineControl>();

		if(control != null) {
			control.state = VineState.Bloom;
		}
	}
}
