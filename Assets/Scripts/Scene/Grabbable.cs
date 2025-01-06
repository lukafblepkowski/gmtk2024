using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Grabbable:MonoBehaviour, IComparable<Grabbable> {
	PointerDisplayer displayer;

	public LayerMask collisionLayermask;
	public float maxColliderRadius;

	[HideInInspector] public bool grabbed = false;
	float gravityCache = 0f;
	float dragCache = 0f;

	Rigidbody2D rb;

	Vector3 posprev;
	float prevdtime;
	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}
	public void Grab() {
		gravityCache = rb.gravityScale;
		rb.gravityScale = 0;
		dragCache = rb.drag;
		rb.drag = 10f;
		grabbed = true;
	}

	public void Release() {
		rb.gravityScale = gravityCache;
		gravityCache = 0;
		rb.drag = dragCache;
		dragCache = 0;
		grabbed = false;

		rb.velocity += (Vector2)((transform.position - posprev)) / prevdtime;
	}

	public void UpdateFromGrabber(Vector2 pos) {
		pos = Vector2.Lerp(transform.position,pos,0.2f);
		Vector2 dir = pos - (Vector2)transform.position;
		//dir += dir.normalized * maxColliderRadius;
		RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, dir, dir.magnitude, collisionLayermask);

		if(hitInfo.collider == null) {
			//Just do it innit

			rb.velocity = Vector3.zero;
			rb.MovePosition(pos);
		} else {
			if(hitInfo.collider.GetComponent<Rigidbody2D>() != null) {
				rb.AddForce(dir.normalized * 0.1f,ForceMode2D.Force);
			}
			else {
				//Phsyics based
				rb.AddForce(dir.normalized*5,ForceMode2D.Force);
			}
		}

		posprev = transform.position;
		prevdtime = Time.deltaTime;
	}

	public int CompareTo(Grabbable g) {
		float v1 = g.transform.position.z;
		float v2 = transform.position.z;

		if(v1 < v2) return -1;
		if(v1 > v2) return 1;

		return 0;
	}
}
