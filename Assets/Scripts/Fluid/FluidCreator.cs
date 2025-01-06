using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FluidCreator : MonoBehaviour
{
    float timer = 0;
    //Public
    public GameObject fluidPrefab;
    public float spawnRadius = 0.5f;
    public float fluidPerSecond = 15;

    public int sForce_maxParticles = 100;

    public float display_percentage = 0;

    //Statics
    static int maxParticles = 0;
    static List<GameObject> fluidParticles = new List<GameObject>();

    static bool subbed = false;
	private void Start() {
        if(!subbed) {
            subbed = true;
            SceneManager.activeSceneChanged += delfunc_RoomChange;
        }
	}

	private void Update() {
		if(maxParticles < sForce_maxParticles) maxParticles = sForce_maxParticles;

		timer += Time.deltaTime;

        float percentage = ((float)fluidParticles.Count)/((float)maxParticles);
        percentage = Mathf.SmoothStep(0f,1f,1f-percentage);
        display_percentage = percentage;
        float currentFPS = fluidPerSecond * percentage;
        float inv = 1f/currentFPS;
        while(timer > inv) {
            timer -= inv;

            AddParticle(fluidPrefab);
        }
	}
	public void AddParticle(GameObject prefab) {
        if(fluidParticles.Count >= maxParticles) {
            return;
        }

        //Should be out of max limit
        GameObject go = Instantiate(prefab);
        go.transform.position = transform.position;
        Vector3 dir =  Quaternion.Euler(0,0,Random.Range(0f,360f)) * Vector3.one * Random.Range(0f, spawnRadius);
        go.transform.position += dir;

        fluidParticles.Add(go);


    }

    public static void RegisterParticle(GameObject particle) {
        if(!fluidParticles.Contains(particle)) {
            fluidParticles.Add(particle);
        }
    }

    public static void RemoveParticle(GameObject particle) {
        if(particle.tag != "Fluid") return;

        if(fluidParticles.Contains(particle)) {
            fluidParticles.Remove(particle);
        }

		Destroy(particle);
	}

    private void delfunc_RoomChange(Scene s1, Scene s2) {
		maxParticles = 0;
		fluidParticles = new List<GameObject>();
	}
}
