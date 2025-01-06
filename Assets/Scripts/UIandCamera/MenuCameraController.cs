using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraController : MonoBehaviour
{
    public static int onMainPage = 0;
    public static float lerp = 0;
    void Start()
    {
        onMainPage = 0;
        lerp = 0;
    }

	private void Update() {
        lerp = Mathf.Lerp(lerp,onMainPage * 4f,Time.deltaTime * 5f);

        transform.position = new Vector3(0,lerp,0);
	}
}
