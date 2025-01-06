using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidCollisionModeController : MonoBehaviour
{
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool fast = rb.velocity.magnitude > 1.0f;
        bool continuous = rb.collisionDetectionMode == CollisionDetectionMode2D.Continuous;

		if(fast && !continuous) {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        } 

        if(!fast && continuous) {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        }
    }
}
