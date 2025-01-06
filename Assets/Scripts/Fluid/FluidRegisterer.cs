using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidRegisterer : MonoBehaviour
{
    void Start()
    {
        FluidCreator.RegisterParticle(gameObject);
    }
}
