using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    static float currentFade = 1f;

    void Update()
    {
        if(RoomManager.targetRoom != null) {
            currentFade = Mathf.Max(RoomManager.transitionLerper, currentFade);
        } else {
            currentFade = Mathf.Lerp(currentFade, UIManager.paused ? 0.75f : 0f, Time.deltaTime * 5f);
        }

        GetComponent<SpriteRenderer>().color = new Color(0,0,0,currentFade * 1.1f);
    }
}
