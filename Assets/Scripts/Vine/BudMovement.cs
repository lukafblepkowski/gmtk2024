using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class BudMovement : MonoBehaviour
{
	public float lightFollowRange = 3f;
	public float lightFollowRotSpeed = 5f;
	public float lightFollowMoveSpeed = 2f;
	public float lightFollowMoveSpeedVibrant = 2f;
	public LayerMask lightFollowSightLayerMask;
	// Start is called before the first frame update

	[HideInInspector]
	public bool blighted = false;

	bool playedBloomSfx = false;
	List<Lightsource> lightsources = new List<Lightsource>();
	Rigidbody2D rb;

	SpriteRenderer headRenderer;

	AudioSource aud;

	public VineControl vine { get; set; }
	public bool isTooClose(float dist) {
		return dist < 0.2f;
	}
	public float getMoveSpeed(float dist, float vibrance) {
		return (isTooClose(dist)) ? 0f : (vibrance * lightFollowMoveSpeed); 
	}

	public void UpdateDrag(float dist) {
		rb.drag = (isTooClose(dist)) ? 28f : 6f;
	}

	public Vector2 facing { get => transform.right; }
	
	public Material material { get {
		return headRenderer.material;
	} }
	float goDistance2(GameObject g1,GameObject g2) {
		return Mathf.Pow(g1.transform.position.x - g2.transform.position.x,2) + Mathf.Pow(g1.transform.position.y - g2.transform.position.y,2);
	}

	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		headRenderer = GetComponentInChildren<SpriteRenderer>();
		aud = GetComponent<AudioSource>();

		//Get lightsources
		Lightsource[] lss = FindObjectsByType<Lightsource>(FindObjectsSortMode.None);

		foreach(Lightsource ls in lss) {
			AddLightsource(ls);
		}
	}

	private void Update() {
		if(vine.state == VineState.Bloom && !playedBloomSfx) {
			aud.Play();

			playedBloomSfx = true;
		}
	}
	private void FixedUpdate() {
		FollowLight();
	}

	public void AddLightsource(Lightsource ls) {
		if(!lightsources.Contains(ls)) lightsources.Add(ls);
	}

	void FollowLight() {
		if(vine.state == VineState.Dry) return;
		if(vine.state == VineState.Bloom) return;
		if(lightsources.Count == 0) return;

		//Find best 
		Lightsource best = null;
		float best_dist2 = float.MaxValue;
		for(int i = 0;i < lightsources.Count;i++) {
			if(!lightsources[i].on) continue;

			Vector2 hitVec = lightsources[i].transform.position - transform.position;

			RaycastHit2D hit = Physics2D.Raycast(transform.position,hitVec,hitVec.magnitude,lightFollowSightLayerMask);
			if(hit.collider != null) continue;

			float pot_dist2 = goDistance2(gameObject,lightsources[i].gameObject);

			//Evaluate pot
			if(pot_dist2 > Mathf.Pow(lightsources[i].range+0.2f,2)) continue;

			if(best_dist2 > pot_dist2) {
				best = lightsources[i];
				best_dist2 = pot_dist2;
			}
		}

		if(best == null) return;

		//Evaluate best
		if(best_dist2 > Mathf.Pow(best.range+0.2f,2)) return;

		blighted=(best.vibrance < 0);

		if(!blighted) {
			//Rotate!!
			Vector2 vecToLight = best.transform.position - transform.position;
			Quaternion toRotate = Quaternion.FromToRotation(facing,vecToLight);
			toRotate = Quaternion.Slerp(Quaternion.identity,toRotate,lightFollowRotSpeed * Time.deltaTime);
			transform.rotation *= toRotate;


			//Move forward!
	
			float best_dist = Mathf.Sqrt(best_dist2);

			Vector3 force = (Vector3)(facing * Mathf.Pow(Vector2.Dot(vecToLight.normalized,facing.normalized),4f)).normalized;

			force *= getMoveSpeed(best_dist,best.vibrance);

			rb.AddForce(force,ForceMode2D.Force);

			UpdateDrag(best_dist);
		} else {
			if(vine.joints.Count > 1) {
				//Rotate but evil
				Vector2 vecToLight = (Vector2)transform.position-vine.jointHead.pos;
				Quaternion toRotate = Quaternion.FromToRotation(facing,vecToLight);
				toRotate = Quaternion.Slerp(Quaternion.identity,toRotate,lightFollowRotSpeed * Time.deltaTime);
				transform.rotation *= toRotate;

				//Move forward but evil

				float best_dist = Mathf.Sqrt(best_dist2);
				Vector3 force = vecToLight.normalized;
				force *= getMoveSpeed(best_dist + 1f,best.vibrance);
				rb.AddForce(force,ForceMode2D.Force);
				UpdateDrag(best_dist);
			}
		}
	}
}
