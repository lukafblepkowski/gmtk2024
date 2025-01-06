using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float force;
	public bool locked = false;
	private void FixedUpdate() {
		if(locked) return;

        Vector2 coords = Input.mousePosition;
		coords.x /= Screen.width;
		coords.y /= Screen.height;
		coords -= new Vector2(0.5f, 0.5f);
		coords *= 2f; //Now going from -1 to 1

		if(Mathf.Abs(coords.x) < 0.75f) coords.x = 0f;
		else coords.x -= Mathf.Sign(coords.x) * 0.75f;
		if(Mathf.Abs(coords.y) < 0.75f) coords.y = 0f;
		else coords.y -= Mathf.Sign(coords.y) * 0.75f;

		coords *= 3f;

		Vector2 vec = Vector2.up * coords.y + Vector2.right * coords.x;

		GetComponent<Rigidbody2D>().AddForce(vec * force, ForceMode2D.Force);
	}
}
