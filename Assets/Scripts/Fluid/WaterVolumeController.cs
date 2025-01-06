using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVolumeController : MonoBehaviour
{
    AudioSource aud;
    Rigidbody2D rb;
    public float mult = 0.02f;
    public LayerMask slapLayer;
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        aud.volume = mult * Mathf.Clamp(rb.velocity.magnitude,0,1f);
    }
}
