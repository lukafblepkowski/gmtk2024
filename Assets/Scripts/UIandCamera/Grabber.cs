using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grabber : MonoBehaviour
{
	private PointerDisplayer displayer;
	private new Camera camera;//THIS "new" is weird. be aware luka !
	private List<Grabbable> canBeGrabbed = new List<Grabbable>();
	private Grabbable grabbed = null;

	bool hoversign = false;
	bool lmb = false;
	private void Start() {
		camera = GetComponentInParent<Camera>();
		displayer = GetComponentInChildren<PointerDisplayer>();

		Cursor.visible = false;
	}
	void Update()
    {
		//Mouse button
		if(Input.GetMouseButtonDown(0)) {
			if(canBeGrabbed.Count > 0) {
				canBeGrabbed.Sort();

				grabbed = canBeGrabbed[0];
				grabbed.Grab();
			}
		}	

		if(Input.GetMouseButtonUp(0)) {
			if(grabbed != null) {
				grabbed.Release();
				grabbed = null;
			}
		}

		if(grabbed != null) {
			if(((Vector2)(grabbed.transform.position - transform.position)).magnitude > 2f) {
				grabbed.Release();
				grabbed = null;
			}
		}

		displayer.selected = (hoversign) || (grabbed != null) || (canBeGrabbed.Count != 0);

		if(grabbed == null) {
			displayer.beamCoord = displayer.transform.position;
		} else {
			displayer.beamCoord = grabbed.transform.position;
		}

		Vector3 screenPosition = Input.mousePosition;

		screenPosition.x /= Screen.width;
		screenPosition.y /= Screen.height;
		screenPosition -= Vector3.one * 0.5f;
		screenPosition *= 2 * camera.orthographicSize;
		screenPosition.x *= camera.aspect;

		screenPosition += camera.transform.position;

		screenPosition.z = -5f;

		transform.position = screenPosition;
	}

	private void FixedUpdate() {
		//Update grabbed
		if(grabbed != null) grabbed.UpdateFromGrabber(transform.position);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		Grabbable comp = collision.gameObject.GetComponent<Grabbable>();
		if(comp == null) comp = collision.gameObject.GetComponentInParent<Grabbable>();
		if(comp == null) comp = collision.gameObject.GetComponentInChildren<Grabbable>();
		if(comp != null) canBeGrabbed.Add(comp);

		if(collision.gameObject.GetComponent<Sign>() != null) hoversign = true;
	}

	private void OnTriggerExit2D(Collider2D collision) {
		Grabbable comp = collision.gameObject.GetComponent<Grabbable>();
		if(comp == null) comp = collision.gameObject.GetComponentInParent<Grabbable>();
		if(comp == null) comp = collision.gameObject.GetComponentInChildren<Grabbable>();
		if(comp != null) canBeGrabbed.Remove(comp);

		if(collision.gameObject.GetComponent<Sign>() != null) hoversign = false;
	}
}
