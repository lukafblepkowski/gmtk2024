using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidDestructor : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other) {
		FluidCreator.RemoveParticle(other.gameObject);
	}
}
