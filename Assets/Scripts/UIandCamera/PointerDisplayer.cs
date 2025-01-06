using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerDisplayer : MonoBehaviour
{
    public GameObject pup;
    public GameObject pdown;
    public GameObject pleft;
    public GameObject pright;
    public GameObject pnone;
    public GameObject beam;

    public bool selected;

    public Vector2 beamCoord;
    Vector3 beamScale;
    SpriteRenderer beamRenderer;

    public float highSpinSpeed = 2.5f;
    float lerper = 0f;

    Vector3 scale;

    float beamdamptime = 1f;

	private void Start() {
        scale = transform.localScale;
		beamScale = beam.transform.localScale;
		transform.localScale = Vector3.one;
        beamRenderer = beam.GetComponent<SpriteRenderer>();
	}
	void Update()
    {
        beamdamptime -= Time.deltaTime;
        if(beamdamptime < 0f) beamdamptime = 0f;

        lerper = Mathf.Lerp(lerper,selected ? 1 : 0, Time.deltaTime * 10f);
        transform.rotation *= Quaternion.Euler(0,0, highSpinSpeed * lerper * Time.deltaTime);

        pup.transform.localScale = scale * lerper;
        pdown.transform.localScale = scale * lerper;
        pleft.transform.localScale = scale * lerper;
        pright.transform.localScale = scale * lerper;
        pnone.transform.localScale = scale * (1.0f - lerper);
        beam.transform.localScale = beamScale * scale.x;

        Vector2 dir = (1-beamdamptime) * (beam.transform.position - transform.position);

        beam.transform.position = Vector2.Lerp(transform.position, beamCoord, 0.5f);

        float angle;
        if(dir.x == 0) angle = 0;
        else angle = 180.0f*Mathf.Atan(dir.y/dir.x)/Mathf.PI;
		float beamsize = dir.magnitude / scale.x * beamScale.x;

		beam.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        beamRenderer.size = new Vector2(beamsize, 0.7f);
    }
}
