using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public string traceTarget;
    Transform trace;

	public TextMeshProUGUI title;
	public TextMeshProUGUI description;
	public TextMeshProUGUI nopause;
	public TextMeshProUGUI fps;
	public static bool paused { get; set; }

	List<float> fpss = new List<float>();
	int fpsrange = 20;

	private void Start() {
		trace = GameObject.Find(traceTarget).transform;
	}
	private void Update() {
		if(!RoomManager.started) return;

		gameObject.transform.position = trace.position;
		gameObject.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,-8f);

		fpss.Add(1f/Time.deltaTime);
		if(fpss.Count > fpsrange) fpss.RemoveAt(0);
		float avg = 0; foreach(float f in fpss) avg += f; avg /= fpss.Count;

		title.enabled = paused;
		description.enabled = paused;
		nopause.enabled = paused;

		title.text = RoomManager.currentRoom.name;
		description.text = RoomManager.currentRoom.description;
		fps.text = Mathf.Round(avg) + " fps";

		if(RoomManager.currentRoom.fileLocation != "LevelMenu" && Input.GetKeyDown(KeyCode.Escape)) paused = !paused;
	}
}
