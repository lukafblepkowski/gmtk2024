using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank:MonoBehaviour {
    public float water = 0f;

    List<GameObject> availableFluid = new List<GameObject>();
    AudioSource aud;

	private void Start() {
		aud = GetComponent<AudioSource>();
	}
	public bool Siphon() {
        if(availableFluid.Count == 0) return false;

        aud.pitch = Random.Range(0.6f,1.5f); 
        aud.Play();

        FluidCreator.RemoveParticle(availableFluid[Random.Range(0, availableFluid.Count - 1)]);

        return true;
    }

	public void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.tag == "Fluid") {
            availableFluid.Add(collision.gameObject);
        }
	}

	public void OnTriggerExit2D(Collider2D collision) {
        if(availableFluid.Contains(collision.gameObject)) {
            availableFluid.Remove(collision.gameObject);
        }
	}
}
