using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Button : MonoBehaviour
{
    bool hoveredByGrabber = false;

	float enabledLerper;

	protected virtual float multi { get => 0.5f; }
	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.GetComponent<Grabber>() != null) {
			hoveredByGrabber =true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if(collision.GetComponent<Grabber>() != null) {
			hoveredByGrabber =false;
		}
	}

	protected virtual void Start() {
		enabledLerper = __isEnabled() ? 1f : 0f;
	}
	protected void Update() {
		enabledLerper = Mathf.Lerp(enabledLerper,__isEnabled() ? 1f : 0f,Time.deltaTime * 5f);
		transform.localScale = Vector3.one * multi * enabledLerper;
		if(Input.GetMouseButtonDown(0) && hoveredByGrabber && __isEnabled()) __OnPress();
	}
	protected abstract void __OnPress();
	protected abstract bool __isEnabled();
}
